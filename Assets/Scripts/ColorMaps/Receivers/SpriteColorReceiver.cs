using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorReceiver : BaseColorReceiver
{
    private SpriteRenderer SP;
    private void Awake()
    {
        SP = GetComponent<SpriteRenderer>();

    }
   
    public override void SetColor()
    {
        SP.color = objPalette.GetColor(colorIndex);
    }

}
