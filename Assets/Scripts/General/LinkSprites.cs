using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LinkSprites : MonoBehaviour
{
    [SerializeField] private SpriteRenderer target;
    [SerializeField] private SpriteRenderer source;
    [SerializeField] private LayerAnimatonMap LM;
    void Update()
    {

        if (LM.sourceMap.Contains(source.sprite))
        {

            if (target.sprite != LM.targetMap[LM.sourceMap.IndexOf(source.sprite)])
            {

                target.sprite = LM.targetMap[LM.sourceMap.IndexOf(source.sprite)];
            }
        }
        else
        {
            Debug.Log("No sprite found");
        }
        if (target.flipX != source.flipX) target.flipX = source.flipX;

    }
}
