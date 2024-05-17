using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BobbingWave : MonoBehaviour
{
    [SerializeField] private float magnitude = 1;
    [SerializeField] private float speed = 1;
    private Vector2 originalPos;
    private float elapsedTime;
    [SerializeField] private Transform anchor;
    [SerializeField] private Vector2 anchorOffset;
    [SerializeField] private bool lockRotation;
    [SerializeField] private Vector2 Floatdir;
    private void Start()
    {
        originalPos = transform.position;
    }
    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (lockRotation)
        {
            transform.eulerAngles = Vector3.zero;
        }
        if (anchor == null)
        {
            transform.position = new Vector2(originalPos.x, originalPos.y ) + (Floatdir * Mathf.Sin(elapsedTime * speed) * magnitude);
        }
        else
        {
            transform.position = new Vector2(anchor.position.x, anchor.position.y) + anchorOffset + (Floatdir * Mathf.Sin(elapsedTime * speed) * magnitude);

        }
    }
}
