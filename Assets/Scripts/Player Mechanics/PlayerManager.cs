using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{
    public GameObject spriteLayerOne;
    public GameObject spriteLayerTwo;
    public TarodevController.PlayerController playerControllerRef;
    public bool inBlockLayerOne, inBlockLayerTwo;
    public Material mapOne;
    public Material mapTwo;

    void Update()
    {

    }


    public void UpdateSprite(GameObject _spriteObj, bool _value)
    {
        Material m = _value == true ? mapOne : mapTwo;
        Debug.Log(m);

        _spriteObj.GetComponent<SpriteRenderer>().material = m;
    }
    public void ChangeGround(LayerMask _newGround)
    {
        playerControllerRef.UpdateGround(_newGround);
    }
}
