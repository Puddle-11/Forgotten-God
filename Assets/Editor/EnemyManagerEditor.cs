using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : UnityEditor.Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ObjectPaletteManager _target = (ObjectPaletteManager)target;
        if (_target == null) return;
        Undo.RecordObject(_target, "Object Palette Manager Change");

    }
}
