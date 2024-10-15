using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LockTransform : MonoBehaviour
{
    [SerializeField] private bool LockRotation;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private bool LockPosition;
    [SerializeField] private Transform _parent;
    [SerializeField] private Vector3 offset;
    private void Awake()
    {
        if(_parent == null)
        {
            _parent = transform.parent;
        }
    }
    private void LateUpdate()
    {
        if(transform.eulerAngles != rotation && LockRotation)
        {
            transform.eulerAngles = rotation;
        }
        if(transform.position != _parent.transform.position + offset && LockPosition)
        {
            if (_parent == null)
            {
                transform.position = offset;

            }
            else
            {
                transform.position = _parent.transform.position + offset;
            }
        }
    }
    
}
