using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XSystem {
    public class XUnityDispatcher : MonoBehaviour {
        public static XUnityDispatcher s_currentDispatcher;
        public static XUnityDispatcher CurrentDispatcher() {
            if (s_currentDispatcher == null || !mWasInitialized) {
                Initialize();
            }
            return s_currentDispatcher;
        }

        private static bool mWasInitialized = false;
        private static bool dispatcherSpawned = false;
        /// <summary>
        /// Call this function in the first running script
        /// </summary>
        public static void Initialize() {

            GameObject newDispatcherObject = new GameObject("__XSystemDispatcher__");
            if (dispatcherSpawned == false) {
                dispatcherSpawned = true;
                DontDestroyOnLoad(newDispatcherObject);
                s_currentDispatcher = newDispatcherObject.AddComponent<XUnityDispatcher>();
                mWasInitialized = true;
            } else {
                DestroyImmediate(newDispatcherObject);
            }
        }
    }
}

