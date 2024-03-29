using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LayerManager : MonoBehaviour
{

    private GameObject playerRef;
    private PlayerManager playerManagerRef;
    [SerializeField] private LayerMask groundLayerOne, groundLayerTwo;
    [SerializeField] private float inGroundSafeDist;
    [SerializeField] private float coyoteChangeTime;
    private float coyoteChangeTimer;

    private Vector2[] raycastDir = new Vector2[]
    {
        new Vector2(0,1),
        new Vector2(1f,1f),
        new Vector2(1,0),
        new Vector2(1f,-1f),
        new Vector2(0,-1),
        new Vector2(-1f,-1f),
        new Vector2(-1,0),
        new Vector2(-1f,1f),

    };
    public static int CurrentLayer;

    private void Start()
    {
        playerRef = GlobalManager.Player;
        if(!playerRef.TryGetComponent<PlayerManager>(out playerManagerRef)) Debug.LogWarning("Player does not contain PlayerManager");
    }
    void Update()
    {
        if (coyoteChangeTimer > 0)
        {
            bool inblock = false;
            for (int i = 0; i < raycastDir.Length; i++)
            {
                float dist = i % 2 != 0 ? inGroundSafeDist * Mathf.Sqrt(2) : inGroundSafeDist;
                if (GlobalFunctions.Raycast(playerRef.transform.position, raycastDir[i], dist, CurrentLayer == 0 ? groundLayerTwo : groundLayerOne).collider != null) inblock = true;
            }
            if (inblock == true)
            {
         
                //play error sound
            coyoteChangeTimer -= Time.deltaTime;
            }
            else
            {
                coyoteChangeTimer = 0;
                CurrentLayer = flip(CurrentLayer);
                ChangeLayers(CurrentLayer);
            }

        }
        else
        {
            coyoteChangeTimer = 0;
        }


        if (Input.GetKeyDown(GlobalManager.globalManagerRef.layerSwapKey))
        {
            //Check if player is in a block
            bool inblock = false;
            for (int i = 0; i < raycastDir.Length; i++)
            {
                float dist = i % 2 != 0 ? inGroundSafeDist * Mathf.Sqrt(2) : inGroundSafeDist;
                if (GlobalFunctions.Raycast(playerRef.transform.position, raycastDir[i], dist, CurrentLayer == 0 ? groundLayerTwo : groundLayerOne).collider != null) inblock = true;
            }
            if (inblock == true)
            { 
                if(coyoteChangeTimer == 0)
                {
                    coyoteChangeTimer = coyoteChangeTime;
                }
                //play error sound
            }
            else
            {
                CurrentLayer = flip(CurrentLayer);
                ChangeLayers(CurrentLayer);
            }
        }
    }
    public void ChangeLayers(int _index)
    {
        bool value = _index == 0 ? true : false;
        playerManagerRef.FlipSprite(playerManagerRef.spriteLayerOne, value);
        playerManagerRef.FlipSprite(playerManagerRef.spriteLayerTwo, !value);
        LayerMask nextLayer = value == true ? groundLayerOne : groundLayerTwo;
        playerManagerRef.ChangeGround(nextLayer);
    }
    private int flip(int _value)
    {
        if(_value == 1)
        {
            return 0;
        }
        else if(_value == 0)
        {
            return 1;
        }
        else
        {
            Debug.LogWarning("Invalid Value");
            return 0;
        }
    }
}
