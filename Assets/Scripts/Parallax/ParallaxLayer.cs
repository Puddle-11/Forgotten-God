using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;


    public void Move(Vector2 delta, Transform Origin)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x = (Origin.position.x - delta.x) * parallaxFactor;
        newPos.y = (Origin.position.y - delta.y) * parallaxFactor;


        //  newPos.x -= delta.x * parallaxFactor;
        // newPos.y -= delta.y * parallaxFactor;

        transform.localPosition = newPos;
    }

}