//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using Aeon.Library;
using AeonVoice;
using System.Diagnostics;
using System.Text;

namespace Aeon.Runtime
{
    /// <summary>Configurable speech backends supported by the runtime.</summary>
    internal enum SpeechBackend
    {
        WindowsSapi,
        AeonVoice
    }

    /// <summary>
    /// Non-blocking voice adapter. Windows uses the built-in SAPI COM voice; Linux uses the bundled AeonVoice package
    /// to synthesize a temporary WAV file and a local player to present it.
    /// </summary>
    internal sealed class SpeechOutputAdapter : IOutputAdapter
    {
        private const int MaximumUtteranceLength = 4096;
        private const string SapiScript = "$text=[Console]::In.ReadToEnd();if(-not [string]::IsNullOrWhiteSpace($text)){$voice=New-Object -ComObject SAPI.SpVoice;$null=$voice.Speak($text)}";
        private readonly SpeechBackend _backend;
        private readonly string _voiceProfile;
        private readonly string _linuxAudioPlayer;
        private readonly TimeSpan _timeout;
        private readonly Action<string> _logWarning;
        private readonly object _speechSync = new object();

        public SpeechOutputAdapter(SpeechBackend backend, string voiceProfile, string linuxAudioPlayer, TimeSpan timeout, Action<string> logWarning)
        {
            _backend = backend;
            _voiceProfile = string.IsNullOrWhiteSpace(voiceProfile) ? "Toptygin" : voiceProfile.Trim();
            _linuxAudioPlayer = string.IsNullOrWhiteSpace(linuxAudioPlayer) ? "aplay" : linuxAudioPlayer.Trim();
            _timeout = timeout <= TimeSpan.Zero ? TimeSpan.FromSeconds(30) : timeout;
            _logWarning = logWarning;
        }

        public OutputModality SupportedModalities => OutputModality.Voice;

        public void Present(OutputPresentation presentation)
        {
            ArgumentNullException.ThrowIfNull(presentation);
            string utterance = presentation.Text?.Trim();
            if (string.IsNullOrEmpty(utterance))
            {
                return;
            }

            if (utterance.Length > MaximumUtteranceLength)
            {
                utterance = utterance[..MaximumUtteranceLength];
            }

            _ = Task.Run(() => Speak(utterance));
        }

        private void Speak(string utterance)
        {
            lock (_speechSync)
            {
                try
                {
                    if (_backend == SpeechBackend.WindowsSapi)
                    {
                        SpeakWithWindowsSapi(utterance);
                    }
                    else
                    {
                        SpeakWithAeonVoice(utterance);
                    }
                }
                catch (Exception exception)
                {
                    _logWarning?.Invoke("Speech output is unavailable: " + exception.Message);
                }
            }
        }

        private void SpeakWithWindowsSapi(string utterance)
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                StandardInputEncoding = Encoding.UTF8,
                CreateNoWindow = true
            };

            startInfo.FileName = "powershell.exe";
            startInfo.ArgumentList.Add("-NoLogo");
            startInfo.ArgumentList.Add("-NoProfile");
            startInfo.ArgumentList.Add("-NonInteractive");
            startInfo.ArgumentList.Add("-Command");
            startInfo.ArgumentList.Add(SapiScript);
            using Process process = Process.Start(startInfo)
                ?? throw new InvalidOperationException("The Windows SAPI process could not be started.");
            process.StandardInput.Write(utterance);
            process.StandardInput.Close();
            WaitForPlayer(process, "Windows SAPI");
        }

        private void SpeakWithAeonVoice(string utterance)
        {
            string wavPath = Path.Combine(Path.GetTempPath(), "aeonvoice-" + Guid.NewGuid().ToString("N") + ".wav");
            try
            {
                using (var engine = new AeonVoiceEngine())
                {
                    SynthesisResult result = engine.SynthesizeToPcm16(utterance, _voiceProfile);
                    if (result.SampleRate <= 0 || result.Samples.Length == 0)
                    {
                        throw new InvalidOperationException("AeonVoice did not produce PCM audio.");
                    }
                    result.WriteWave(wavPath);
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = _linuxAudioPlayer,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                startInfo.ArgumentList.Add("--quiet");
                startInfo.ArgumentList.Add(wavPath);
                using Process process = Process.Start(startInfo)
                    ?? throw new InvalidOperationException("The Linux audio player could not be started.");
                WaitForPlayer(process, "Linux audio playback");
            }
            finally
            {
                if (File.Exists(wavPath))
                {
                    File.Delete(wavPath);
                }
            }
        }

        private void WaitForPlayer(Process process, string operation)
        {
            if (!process.WaitForExit((int)_timeout.TotalMilliseconds))
            {
                process.Kill(entireProcessTree: true);
                _logWarning?.Invoke(operation + " exceeded its configured timeout and was stopped.");
            }
            else if (process.ExitCode != 0)
            {
                _logWarning?.Invoke(operation + " exited with code " + process.ExitCode + ".");
            }
        }
    }
}
