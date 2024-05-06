using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;
 
    // Update is called once per frame
    void Update()
    {
        if(transform.eulerAngles != rotation)
        {
            transform.eulerAngles = rotation;
        }
    }
}
