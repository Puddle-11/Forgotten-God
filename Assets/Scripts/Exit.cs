using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public Side exitSide;
    public Exit link;
    public static Vector2[] directionMatrix = {
    new Vector2(-1, 0),
    new Vector2(1, 0),
    new Vector2(0, 1),
    new Vector2(0, -1),
    }; public static Vector3[] rotationMatrix = {
    new Vector3(0, 0,90),
    new Vector3(0, 0,-90),
    new Vector3(0, 0,90),
    new Vector3(0, 0,180),
    };
    private void Start()
    {
        transform.localEulerAngles = rotationMatrix[(int)exitSide];
    }
    public enum Side
    {
        left = 0,
        right = 1,
        top = 2,
        bottom = 3,
    }

    public static void Teleport(Exit _endExit)
    {
        Debug.Log("Side: " + _endExit.exitSide + " Int: " +(int)_endExit.exitSide) ;
        GlobalManager.Player.transform.position = _endExit.transform.position + (Vector3)(directionMatrix[(int)_endExit.exitSide] * GlobalManager.globalManagerRef.GetPlayerManager().entranceOffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GlobalManager.Player)
        {
            Teleport(link);
        }
    }
}
