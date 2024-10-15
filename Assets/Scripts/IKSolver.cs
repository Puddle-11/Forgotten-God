using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
[ExecuteInEditMode]
public class IKSolver : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform target;
    [SerializeField] private float margin;
    private Vector2[] points;
    [SerializeField] private Arm[] arms;
    private float totalLength;
    public bool preferUp;
    public float UpMargin;
    #region Custm Structs and Enums

    [System.Serializable]
    public struct Arm
    {
        public Transform worldObj;
        public float segmentLength;
    }
    #endregion
    private void Awake() { CreateArm(); }
    private void Update() { Solve(); }
    public void CreateArm()
    {
        points = new Vector2[arms.Length + 1];
        Solve();
    }
    public float GetSegmentLength(int _index)
    {
        if (_index < 0 || _index >= arms.Length) return 0;
        return arms[_index].segmentLength;
    }
    public float GetTotalLength()
    {
        float res = 0;
        for (int i = 0; i < arms.Length; i++)
        {
            res += arms[i].segmentLength;
        }
        return res;
    }
    public void Solve()
    {
       
        if (points == null || points.Length != arms.Length + 1)
        {
            CreateArm();
            return;
        }
        if (Vector2.Distance(target.position, start.position) > GetTotalLength())
        {
            points = OutofRangeSolve(start.position, target.position, points);

        }
        else
        {
            for (int i = 0; i < 32; i++)
            {
                points = Iterate(start.position, points);
                Array.Reverse(points);
                Array.Reverse(arms);
                points = Iterate(target.position, points);
                Array.Reverse(points);
                Array.Reverse(arms);
                if (Vector2.Distance(points[0], start.position) < margin && Vector2.Distance(points[points.Length - 1], target.position) < margin)
                {
                    break;
                }
            }
        }
        for (int i = 0; i < points.Length - 1; i++)
        {
            if (arms == null || i >= arms.Length) break;

            if (arms[i].worldObj == null) continue;
            arms[i].worldObj.position = points[i];
            Quaternion rot = Quaternion.LookRotation(points[i + 1] - points[i]);

            arms[i].worldObj.rotation = rot;
        }
    }
    private Vector2[] Iterate(Vector2 _target, Vector2[] _points)
    {

        Vector2[] result = new Vector2[_points.Length];
        Array.Copy(_points, result, _points.Length);
        result[0] = _target;
        for (int i = 1; i < result.Length; i++)
        {
          
            Vector2 dir = (result[i] - result[i - 1]).normalized;
           
            Vector2 point = dir * GetSegmentLength(i - 1);

            result[i] = result[i - 1] + point;
        }

        return result;
    }
    public Vector2[] OutofRangeSolve(Vector2 _start, Vector2 _target, Vector2[] _points)
    {
        Vector2[] result = new Vector2[_points.Length];
        Array.Copy(_points, result, _points.Length);
        result[0] = _start;
        Vector2 dir = (_target - _start).normalized;

        for (int i = 1; i < result.Length; i++)
        {
            result[i] = result[i - 1] + dir * GetSegmentLength(i - 1);
        }
        return result;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(points[0], 0.1f);
        for (int i = 1; i < points.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(points[i], 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(points[i], points[i - 1]);
        }
    }

}
