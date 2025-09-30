using NUnit.Framework;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PathFinder))]
public class PathFinderEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        PathFinder _target = (PathFinder)target;
        if (_target == null) return;
        Undo.RecordObject(_target, "Path Finder Editor Change");

        if (GUILayout.Button("Start Search"))
        {
            _target.StartFind();
        }
        if (GUILayout.Button("Clear"))
        {
            _target.ClearAll();
        }
        SceneView.RepaintAll();
    }
}
