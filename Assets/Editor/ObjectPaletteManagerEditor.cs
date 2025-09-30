using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static log4net.Appender.ColoredConsoleAppender;
[CustomEditor(typeof(ObjectPaletteManager))]
public class ObjectPaletteManagerEditor : UnityEditor.Editor
{
    private const int borderWidth = 2;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ObjectPaletteManager _target = (ObjectPaletteManager)target;
        if (_target == null) return;
        Undo.RecordObject(_target, "Object Palette Manager Change");
        if (ColorManager.isValid())
        {
            float height = _target.displaySize * ColorManager.instance.size.y;

            float width = _target.displaySize * ColorManager.instance.size.x;

            Rect rect = GUILayoutUtility.GetRect(100, 1000, height + (borderWidth*2), height + (borderWidth * 2));

            if (Event.current.type == EventType.Repaint)
            {
                GUI.BeginClip(rect);
                Rect edge1 = new Rect(0, 0, borderWidth, height + borderWidth);
                Rect edge2 = new Rect(width + borderWidth, 0, borderWidth, height + borderWidth);

                Rect edge3 = new Rect(0, 0, width + borderWidth, borderWidth);
                Rect edge4 = new Rect(0, height + borderWidth, width + (2*borderWidth), borderWidth);

                EditorGUI.DrawRect(edge1, Color.white);
                EditorGUI.DrawRect(edge2, Color.white);
                EditorGUI.DrawRect(edge3, Color.white);
                EditorGUI.DrawRect(edge4, Color.white);

                for (int i = 0; i < _target.colorPos.Length; i++)
                {
                    Rect si = new Rect(_target.displaySize * _target.colorPos[i].x + borderWidth, _target.displaySize * (ColorManager.instance.size.y - _target.colorPos[i].y - 1) + borderWidth, _target.displaySize, _target.displaySize);
                    EditorGUI.DrawRect(si, _target.GetColor(i));

                }

                GUI.EndClip();
            }
        }

    }
}
