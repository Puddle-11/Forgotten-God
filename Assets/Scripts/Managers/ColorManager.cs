using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColorManager : MonoBehaviour
{
    public static ColorManager CMref;
    public Sprite mainPalette;
    public Vector2Int size;
    [SerializeField] private Vector2Int playerBodyPos;
    [SerializeField] private Vector2Int playerEyePos;
    public Material playerLayerOneMat;
    public Material playerLayerTwoMat;

    private void Awake()
    {
        if(CMref == null)
        {
            CMref = this;
        }
        else
        {
            Destroy(this);
        }
        size.x = mainPalette.texture.width;
        size.y= mainPalette.texture.height;
    }

   
   
    public Color GetTextureColor(Vector2Int _pos)
    {
        if (_pos.x > size.x || _pos.y > size.y) return Color.cyan;
        else return mainPalette.texture.GetPixel(_pos.x, _pos.y);
    }

}
