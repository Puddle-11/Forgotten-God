using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[ExecuteAlways]
public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    public Sprite mainPalette;
    public Vector2Int size;
    [SerializeField] private Vector2Int playerBodyPos;
    [SerializeField] private Vector2Int playerEyePos;
    public Material playerLayerOneMat;
    public Material playerLayerTwoMat;

    private void Awake()
    {
        size.x = mainPalette.texture.width;
        size.y= mainPalette.texture.height;
    }
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    public static bool isValid()
    {
        return instance != null;
    }
   
   
    public Color GetTextureColor(Vector2Int _pos)
    {
        if (_pos.x > size.x || _pos.y > size.y) return Color.cyan;
        else return mainPalette.texture.GetPixel(_pos.x, _pos.y);
    }

}
