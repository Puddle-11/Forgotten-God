using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{

    public delegate void ParallaxCameraDelegate(Vector2 deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    private Vector2 oldPosition;
    void Start()
    {
        oldPosition = transform.position;

    }
    void Update()
    {
        if (transform.position.x != oldPosition.x || transform.position.y != oldPosition.y)
        {
            if (onCameraTranslate != null)
            {
                Vector2 delta = transform.position;
                onCameraTranslate(delta);
            }

            oldPosition = transform.position;
        }

    }
}
