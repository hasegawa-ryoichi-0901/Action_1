using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Nekomimi.Daimao {
    public class LogWriter {
        /// <summary>
        /// LogFilePath
        /// </summary>
        public readonly string LogPath;

        private static StreamWriter streamWriter = null;
        private LogType _logType;

        public LogWriter(string logPath, LogType filterLogType, CancellationToken cancellationToken) {
            this.LogPath = logPath;
            this._logType = filterLogType;
            Application.logMessageReceivedThreaded += OnLogMessageReceivedThreaded;
            LogWriteLoop(cancellationToken).Forget();
        }

        private readonly Dictionary<int, string> logLevelCase = new Dictionary<int, string>() {
            { (int)LogType.Error, "E" },
            { (int)LogType.Assert, "A" },
            { (int)LogType.Warning, "W" },
            { (int)LogType.Log, "D" },
            { (int)LogType.Exception, "X" },
        };

        private const string DateTimeFormat = "HHmmss.fff";

        private void OnLogMessageReceivedThreaded(string condition, string stacktrace, LogType type) {
            switch (this._logType) {
                case LogType.Error:
                    switch (type) {
                        case LogType.Error:
                        case LogType.Assert:
                        case LogType.Exception:
                            break;
                        default:
                            return;
                    }
                    break;
                case LogType.Assert:
                    switch (type) {
                        case LogType.Assert:
                        case LogType.Exception:
                            break;
                        default:
                            return;
                    }
                    break;
                case LogType.Warning:
                    switch (type) {
                        case LogType.Warning:
                        case LogType.Error:
                        case LogType.Assert:
                        case LogType.Exception:
                            break;
                        default:
                            return;
                    }
                    break;
                case LogType.Exception:
                    switch (type) {
                        case LogType.Exception:
                            break;
                        default:
                            return;
                    }
                    break;
            }
            logQueue.Add($"{DateTimeOffset.Now.ToString(DateTimeFormat)} [{logLevelCase[(int)type]}] {condition}");
        }

        private readonly BlockingCollection<string> logQueue =
            new BlockingCollection<string>(new ConcurrentQueue<string>());

        private async UniTaskVoid LogWriteLoop(CancellationToken cancellationToken) {
            try {
                var parentDir = new FileInfo(LogPath).Directory;
                if (parentDir != null && !parentDir.Exists) {
                    parentDir.Create();
                }

                streamWriter = new StreamWriter(LogPath, true, Encoding.UTF8);
                streamWriter.AutoFlush = false;

                while (true) {
                    if (cancellationToken.IsCancellationRequested) {
                        break;
                    }

                    await UniTask.SwitchToThreadPool();
                    var log = logQueue.Take(cancellationToken);
                    await streamWriter.WriteLineAsync(log);
                }
            }
            finally {
                if (streamWriter != null) {
                    await streamWriter.FlushAsync();
                    streamWriter.Close();
                    await streamWriter.DisposeAsync();
                    streamWriter = null;
                }
            }
        }

        public static async UniTask FlushAsync() {
            if (streamWriter != null) {
                await streamWriter.FlushAsync();
            }
        }
    }
}