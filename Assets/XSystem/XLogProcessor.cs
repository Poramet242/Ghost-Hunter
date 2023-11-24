using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SysDiag = System.Diagnostics;
using UnityEngine;

namespace XSystem.Log {

    [Serializable]
    public struct LogEntry {
        public DateTime logTime;
        public string message;
        public LogStackFrame[] stackTrace;
    }

    [Serializable]
    public struct LogStackFrame {
        public string methodName;
        public string fileName;
        public int lineNo;
        public static LogStackFrame FromSystemStackFrame(SysDiag.StackFrame frame) {
            return new LogStackFrame() {
                methodName = frame.GetMethod().Name,
                fileName = frame.GetFileName(),
                lineNo = frame.GetFileLineNumber()
            };
        }
    }

    public class XLogProcessor : MonoBehaviour {
        static XLogProcessor mInstance = null;
        public static XLogProcessor GetInstance() {
            return mInstance;
        }

        XLogHandler mLogHandler;
        List<LogEntry> mLogEntries;
        
        void OnApplicationQuit() {
            if (mLogHandler != null) {
                mLogHandler.Dispose();
            }
        }

        void Awake() {
            if (mInstance != null) {
                Destroy(this.gameObject);
                return;
            }

            mLogEntries = new List<LogEntry>();
            mLogHandler = new XLogHandler();

            mInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public class XLogHandler : ILogHandler {
        private FileStream mFileStream;
        private StreamWriter mStreamWriter;
        private ILogHandler mDefaultLogHandler;
        
        public XLogHandler() {
            try {
                string filePath = Application.persistentDataPath + "/xlog.log";
                mFileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                mStreamWriter = new StreamWriter(mFileStream);
            }
            catch (Exception ex) {
                Debug.LogErrorFormat("[XLogHandler] Initial XLogHandler failed: cannot create log file stream: {0}", ex.Message);
                return;
            }
            
            mDefaultLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = this;
        }

        public void LogException(Exception exception, UnityEngine.Object context) {
            mDefaultLogHandler.LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) {
            string message = string.Format(format, args);
            SysDiag.StackTrace st = new System.Diagnostics.StackTrace(2, true);
            SysDiag.StackFrame[] frames = st.GetFrames();
            LogStackFrame[] logStackFrames = new LogStackFrame[frames.Length];
            for (int i = 0; i < frames.Length; i++) {
                logStackFrames[i] = LogStackFrame.FromSystemStackFrame(frames[i]);
            }

            var logEntry = new LogEntry() {
                logTime = DateTime.Now,
                message = message,
                stackTrace = logStackFrames
            };
            string json = JsonUtility.ToJson(logEntry);
            mStreamWriter.WriteLine(json);
            mStreamWriter.Flush();

            mDefaultLogHandler.LogFormat(logType, context, format, args);
        }

        public void Dispose() {
            mFileStream.Close();
            mStreamWriter.Close();
        }
    }

}