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
        AeonVoice
    }

    /// <summary>
    /// Non-blocking voice adapter. Windows uses the built-in SAPI COM voice; Linux launches an AeonVoice-compatible
    /// executable that reads the utterance from standard input.
    /// </summary>
    internal sealed class SpeechOutputAdapter : IOutputAdapter
    {
        private const int MaximumUtteranceLength = 4096;
        private const string SapiScript = "$text=[Console]::In.ReadToEnd();if(-not [string]::IsNullOrWhiteSpace($text)){$voice=New-Object -ComObject SAPI.SpVoice;$null=$voice.Speak($text)}";
        private readonly SpeechBackend _backend;
        private readonly string _aeonVoiceCommand;
        private readonly TimeSpan _timeout;
        private readonly Action<string> _logWarning;
        private readonly object _speechSync = new object();

        public SpeechOutputAdapter(SpeechBackend backend, string aeonVoiceCommand, TimeSpan timeout, Action<string> logWarning)
        {
            _backend = backend;
            _aeonVoiceCommand = string.IsNullOrWhiteSpace(aeonVoiceCommand) ? "aeonvoice" : aeonVoiceCommand.Trim();
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
                    using Process process = Process.Start(CreateStartInfo())
                        ?? throw new InvalidOperationException("The speech process could not be started.");
                    process.StandardInput.Write(utterance);
                    process.StandardInput.Close();
                    if (!process.WaitForExit((int)_timeout.TotalMilliseconds))
                    {
                        process.Kill(entireProcessTree: true);
                        _logWarning?.Invoke("Speech output exceeded its configured timeout and was stopped.");
                    }
                    else if (process.ExitCode != 0)
                    {
                        _logWarning?.Invoke("Speech output exited with code " + process.ExitCode + ".");
                    }
                }
                catch (Exception exception)
                {
                    _logWarning?.Invoke("Speech output is unavailable: " + exception.Message);
                }
            }
        }

        private ProcessStartInfo CreateStartInfo()
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                StandardInputEncoding = Encoding.UTF8,
                CreateNoWindow = true
            };

            if (_backend == SpeechBackend.WindowsSapi)
            {
                startInfo.FileName = "powershell.exe";
                startInfo.ArgumentList.Add("-NoLogo");
                startInfo.ArgumentList.Add("-NoProfile");
                startInfo.ArgumentList.Add("-NonInteractive");
                startInfo.ArgumentList.Add("-Command");
                startInfo.ArgumentList.Add(SapiScript);
            }
            else
            {
                startInfo.FileName = _aeonVoiceCommand;
            }

            return startInfo;
        }
    }
}
