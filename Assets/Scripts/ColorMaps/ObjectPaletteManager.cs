using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPaletteManager : MonoBehaviour
{
    [SerializeField] private Vector2Int[] colorPos;

    public Color GetColor(int _index)
    {
        if (_index >= colorPos.Length || _index < 0 || !ColorManager.isValid()) return Color.cyan;
        return ColorManager.CMref.GetTextureColor(colorPos[_index]);

    }
}
