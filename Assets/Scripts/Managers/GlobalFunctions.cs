using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;

public class GlobalFunctions
{
    public static bool GetRange(Vector3 _Target, Vector3 _Self, float Distance)
    {
        //3   Dimensional

        float X = Vector3.Distance(_Target, _Self);
        return X > Distance ? false : true;
    }
    public static bool GetRange(Vector2 _Target, Vector2 _Self, float Distance)
    {
        //2 Dimensional
        float X = Vector2.Distance(_Target, _Self);
        return X > Distance ? false : true;
    }
    public static bool GetRange(float _Target, float _Self, float Distance)
    {
        //1 Dimensional
        float X = Mathf.Abs(_Target - _Self);
        return X > Distance ? false : true;
    }
    public static void RandomRot(Vector3 Range, ref UnityEngine.Transform _Trans)
    {
        _Trans.localEulerAngles = new Vector3(_Trans.localEulerAngles.x, _Trans.localEulerAngles.y, UnityEngine.Random.Range(Range.x, Range.y));
    }



    public static RaycastHit2D Raycast(Vector3 _Origin, Vector3 _Direction, float Distance)
    {
        _Direction = _Direction.normalized;
        Debug.DrawRay(_Origin, _Direction * Distance, Color.blue);
        return Physics2D.Raycast(_Origin, _Direction, Distance);
    }
    public static RaycastHit2D Raycast(Vector3 _Origin, Vector3 _Direction, float Distance, LayerMask _Mask)
    {
        _Direction = _Direction.normalized;

        RaycastHit2D res = Physics2D.Raycast(_Origin, _Direction, Distance, _Mask);

        Debug.DrawRay(_Origin, _Direction * Distance, res.collider != null ? Color.red : Color.green);
      
        return res;
    }
}
