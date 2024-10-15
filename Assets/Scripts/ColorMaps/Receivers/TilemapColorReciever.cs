using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[RequireComponent(typeof(Tilemap))]
public class TilemapColorReciever : BaseColorReceiver
{
    private Tilemap TM;
    private void Awake()
    {
        TM = GetComponent<Tilemap>();

    }
    public override void SetColor()
    {
        TM.color = objPalette.GetColor(colorIndex);

    }

}
