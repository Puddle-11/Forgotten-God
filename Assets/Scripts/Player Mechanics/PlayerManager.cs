using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
public class PlayerManager : MonoBehaviour
{
    public GameObject spriteLayerOne;
    public GameObject spriteLayerTwo;
    public TarodevController.PlayerController playerControllerRef;
    public bool inBlockLayerOne, inBlockLayerTwo;

    void Update()
    {
    }

    public void FlipSprite(GameObject _spriteObj, bool _value)
    {
        _spriteObj.GetComponent<SpriteRenderer>().enabled = _value;
    }
    public void ChangeGround(LayerMask _newGround)
    {
        playerControllerRef.UpdateGround(_newGround);
    }
}
