using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SysDiag = System.Diagnostics;

using UnityEngine;
using UnityEditor;
using System.Text;
using XSystem.Log;

namespace XSystem.Editor {

    public class XLogConsoleWindow : EditorWindow {
        
        private Dictionary<string, ScrollViewData> mScrollViewDataByName;
        private List<LogEntry> mLogs;

        [MenuItem("Window/XSystem/Log window")]
        public static void ShowWindow() {
            var window = EditorWindow.GetWindow<XLogConsoleWindow>();
            window.Init();
            window.Show();
        }

        private void Init() {
            mScrollViewDataByName = new Dictionary<string, ScrollViewData>();
            mLogs = new List<LogEntry>();
        }

        private string Logdirectory() {
            return Application.persistentDataPath;
        }
        private string LogFilePath() {
            return Path.Combine(Logdirectory(), "xlog.log");
        }
        
        private void OnGUI() {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("XLog console");
                    GUILayout.FlexibleSpace();

                    Button("Refresh", () => {
                        string filePath = LogFilePath();
                        mLogs.Clear();
                        using (StreamReader r = new StreamReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))) {
                            while (!r.EndOfStream) {
                                string l = r.ReadLine();
                                mLogs.Add(JsonUtility.FromJson<LogEntry>(l));
                            }
                        }
                    });

                    Button("Show log file", () => {
                        SysDiag.Process.Start("explorer.exe", @Logdirectory());
                    });
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();
                {
                    ScrollView("LogContent", () => {
                        for (int i = 0; i < mLogs.Count; i++) {
                            LogEntryRender(mLogs[i], i % 2 == 0);
                        }
                    });
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        
        private void Button(string content, Action func) {
            if (GUILayout.Button(content)) {
                func();
            }
        }


        private class ScrollViewData {
            public Vector2 scrollViewPosition;
        }
        private void ScrollView(string name, Action renderFunc) {
            if (!mScrollViewDataByName.ContainsKey(name)) {
                mScrollViewDataByName.Add(name, new ScrollViewData());
            }
            var sd = mScrollViewDataByName[name];
            sd.scrollViewPosition = GUILayout.BeginScrollView(sd.scrollViewPosition);
            renderFunc();
            GUILayout.EndScrollView();
        }


        private void LogEntryRender(LogEntry log, bool oddLine) {
            EditorGUILayout.BeginHorizontal(listItemStyle(oddLine));
            {
                GUILayout.Label(log.message);
                var e = UnityEngine.Event.current;
                if (GUILayoutUtility.GetLastRect().Contains(e.mousePosition)) {
                    if (e.isMouse && e.button == 0) {
                        Debug.Log("Clicked at " + log.message);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }



        #region Style

        static Texture2D s_backgroundOddLine = MakeTex(10, 10, Color.white);
        static Texture2D s_backgroundEvenLine = MakeTex(10, 10, new Color(0.75f, 0.75f, 0.75f));

        private static GUIStyle listItemStyle(bool oddItem) {
            GUIStyle style = new GUIStyle();
            style.normal.background = oddItem ? s_backgroundOddLine : s_backgroundEvenLine;
            return style;
        }

        private static Texture2D MakeTex(int width, int height, Color col) {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        #endregion

    }

}