using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bullet))]
public class BulletEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        Bullet _target = (Bullet)target;
        if (_target == null) return;
        Undo.RecordObject(_target, "Bullet Component Changed");

        switch (_target.m_bulletType)
        {
            case Bullet.BulletType.Curved:
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("---Curved Variables---");
                _target.m_curveAmount = EditorGUILayout.FloatField("Curve Amount", _target.m_curveAmount);
                break;
            case Bullet.BulletType.Firework:

                break;
            case Bullet.BulletType.Tracking:

                break;
        
        }


    }


}
