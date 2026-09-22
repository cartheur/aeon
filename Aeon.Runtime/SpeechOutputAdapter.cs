//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using Aeon.Library;
using System.Diagnostics;
using System.Text;

namespace Aeon.Runtime
{
    /// <summary>Configurable speech backends supported by the runtime.</summary>
    internal enum SpeechBackend
    {
        WindowsSapi,
        AeonVoice,
        Espeak
    }

    /// <summary>
    /// Non-blocking voice adapter. Windows uses the built-in SAPI COM voice; Linux uses AeonVoice when the optional
    /// private package is deployed and otherwise falls back to eSpeak NG.
    /// </summary>
    internal sealed class SpeechOutputAdapter : IOutputAdapter
    {
        private const int MaximumUtteranceLength = 4096;
        private const string SapiScript = "$text=[Console]::In.ReadToEnd();if(-not [string]::IsNullOrWhiteSpace($text)){$voice=New-Object -ComObject SAPI.SpVoice;$null=$voice.Speak($text)}";
        private readonly SpeechBackend _backend;
        private readonly string _voiceProfile;
        private readonly string _linuxAudioPlayer;
        private readonly string _espeakCommand;
        private readonly string _espeakVoice;
        private readonly TimeSpan _timeout;
        private readonly Action<string> _logWarning;
        private readonly object _speechSync = new object();

        public SpeechOutputAdapter(SpeechBackend backend, string voiceProfile, string linuxAudioPlayer, string espeakCommand, string espeakVoice, TimeSpan timeout, Action<string> logWarning)
        {
            _backend = backend;
            _voiceProfile = string.IsNullOrWhiteSpace(voiceProfile) ? "Toptygin" : voiceProfile.Trim();
            _linuxAudioPlayer = string.IsNullOrWhiteSpace(linuxAudioPlayer) ? "aplay" : linuxAudioPlayer.Trim();
            _espeakCommand = string.IsNullOrWhiteSpace(espeakCommand) ? "espeak-ng" : espeakCommand.Trim();
            _espeakVoice = string.IsNullOrWhiteSpace(espeakVoice) ? "en-us" : espeakVoice.Trim();
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
                    else if (_backend == SpeechBackend.AeonVoice && TrySpeakWithAeonVoice(utterance))
                    {
                        return;
                    }
                    else
                    {
                        SpeakWithEspeak(utterance);
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

        private bool TrySpeakWithAeonVoice(string utterance)
        {
            Type engineType = Type.GetType("AeonVoice.AeonVoiceEngine, AeonVoice", throwOnError: false);
            if (engineType is null)
            {
                return false;
            }

            string wavPath = Path.Combine(Path.GetTempPath(), "aeonvoice-" + Guid.NewGuid().ToString("N") + ".wav");
            try
            {
                object engine = Activator.CreateInstance(engineType)
                    ?? throw new InvalidOperationException("AeonVoice could not create its synthesis engine.");
                try
                {
                    object result = engineType.GetMethod("SynthesizeToPcm16", new[] { typeof(string), typeof(string) })?.Invoke(engine, new object[] { utterance, _voiceProfile })
                        ?? throw new InvalidOperationException("AeonVoice did not return a synthesis result.");
                    int sampleRate = (int)(result.GetType().GetProperty("SampleRate")?.GetValue(result) ?? 0);
                    Array samples = result.GetType().GetProperty("Samples")?.GetValue(result) as Array;
                    if (sampleRate <= 0 || samples is null || samples.Length == 0)
                    {
                        throw new InvalidOperationException("AeonVoice did not produce PCM audio.");
                    }
                    result.GetType().GetMethod("WriteWave", new[] { typeof(string) })?.Invoke(result, new object[] { wavPath });
                }
                finally
                {
                    (engine as IDisposable)?.Dispose();
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
                return true;
            }
            catch (Exception exception)
            {
                _logWarning?.Invoke("AeonVoice is unavailable; falling back to eSpeak NG: " + exception.GetBaseException().Message);
                return false;
            }
            finally
            {
                if (File.Exists(wavPath))
                {
                    File.Delete(wavPath);
                }
            }
        }

        private void SpeakWithEspeak(string utterance)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = _espeakCommand,
                UseShellExecute = false,
                RedirectStandardInput = true,
                StandardInputEncoding = Encoding.UTF8,
                CreateNoWindow = true
            };
            startInfo.ArgumentList.Add("--stdin");
            startInfo.ArgumentList.Add("-v");
            startInfo.ArgumentList.Add(_espeakVoice);
            using Process process = Process.Start(startInfo)
                ?? throw new InvalidOperationException("The eSpeak NG process could not be started.");
            process.StandardInput.Write(utterance);
            process.StandardInput.Close();
            WaitForPlayer(process, "eSpeak NG");
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
