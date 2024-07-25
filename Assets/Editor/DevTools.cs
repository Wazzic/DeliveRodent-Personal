using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using Unity.VisualScripting;
using Lofelt.NiceVibrations;

public class DevTools : EditorWindow
{
    [SerializeField] int numberOfPlayers = 1;
    [SerializeField] PlayerConfigs playerConfigs;

    [SerializeField] AnalogueClock clock;
    bool infiniteTimeToggle;

    bool hapticFeedback = true;
    

    [MenuItem("Tools/DevTools")]
    static void CreateDevTools()
    {
        EditorWindow.GetWindow<DevTools>();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(20);
        infiniteTimeToggle = GUILayout.Toggle(infiniteTimeToggle, "InfiniteTime");
        EditorGUILayout.Space();

        clock = (AnalogueClock)EditorGUILayout.ObjectField("GameClock", clock, typeof(AnalogueClock), true);
        clock = FindObjectOfType<AnalogueClock>();
        EditorGUILayout.Space(30);

        if (infiniteTimeToggle && clock != null)
        {
            clock.infiniteTime = true;
        }
        else if (!infiniteTimeToggle && clock != null)
        {
            clock.infiniteTime = false;
        }
        
        if (GUILayout.Button("Start Analytics"))
        {
            AnalyticsManager.instance.EnableAnalytics();
        }

        EditorGUILayout.Space(20);
        hapticFeedback = GUILayout.Toggle(hapticFeedback, "Toggle HapticFeedback");
        HapticController.hapticsEnabled = hapticFeedback;        
    }
}
