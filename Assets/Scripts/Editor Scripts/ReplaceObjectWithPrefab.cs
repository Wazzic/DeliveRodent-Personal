using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceObjectWithPrefab : EditorWindow
{
    [SerializeField] private GameObject preFab;

    [MenuItem("Tools/ReplaceWithPreFab")]
    static void CreateReplaceObjectWithPreFab()
    {
        EditorWindow.GetWindow<ReplaceObjectWithPrefab>();
    }

    private void OnGUI()
    {
        preFab = (GameObject)EditorGUILayout.ObjectField("PreFab", preFab, typeof(GameObject), false);

        if (GUILayout.Button("Replace"))
        {
            var selection = Selection.gameObjects;

            for (int i = selection.Length - 1; i >= 0; i--)
            {
                var selected = selection[i];
                var preFabType = PrefabUtility.GetPrefabType(preFab);
                GameObject newObject;

                if (preFabType == PrefabType.Prefab)
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(preFab);
                }
                else
                {
                    newObject = Instantiate(preFab);
                    newObject.name = preFab.name;
                }

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }
                Undo.RegisterCreatedObjectUndo(newObject, "Replace with PreFabs");
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition = selected.transform.localPosition;
                newObject.transform.localRotation = selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                Undo.DestroyObjectImmediate(selected);
            }
        }
        GUI.enabled = false;
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }
}
