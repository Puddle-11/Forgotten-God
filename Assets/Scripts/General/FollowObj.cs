using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    public Vector2 offset;
    public Transform Target;
    public bool fixRotation;
    public void Update()
    {
        transform.position = Target.position + (Vector3)offset;
        
    }
}
