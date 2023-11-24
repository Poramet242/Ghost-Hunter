using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XPlayerPrefUtilityEditor : EditorWindow {

    [MenuItem("Window/XSystem/PlayerPref Utility/Delete All")]
    public static void DeleteAllPlayerPrefs() {
        var confirm = EditorUtility.DisplayDialog("Are you sure", "Delete all data stored in playerpref? This action cannot undo, Confirm?", "Delete");
        if (!confirm) {
            return;
        }
        PlayerPrefs.DeleteAll();
    }
}
