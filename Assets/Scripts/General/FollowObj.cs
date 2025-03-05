using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObj : MonoBehaviour
{
    public Vector2 offset;
    public Transform Target;
    public bool fixRotation;
    [SerializeField] private MirrorActive mirror;
    public void Update()
    {
        if (transform != null && Target != null)
        {

            if (mirror != null && mirror.target == null) mirror.target = Target.gameObject;

            transform.position = Target.position + (Vector3)offset;
        }
        if (Target == null && this != null)
        {
            Destroy(gameObject);
        }


    }
}
