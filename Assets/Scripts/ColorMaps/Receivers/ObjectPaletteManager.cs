using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
[ExecuteAlways]
public class ObjectPaletteManager : MonoBehaviour
{
    public Vector2Int[] colorPos;
    [Range(5, 30)]
    public int displaySize = 10;
    public void Update()
    {
        CleanList();
    }
    public void CleanList()
    {
        if (!ColorManager.isValid()) return;
        for (int i = 0; i < colorPos.Length; i++)
        {
            colorPos[i].x = Mathf.Clamp(colorPos[i].x, 0, ColorManager.instance.size.x - 1);
            colorPos[i].y = Mathf.Clamp(colorPos[i].y, 0, ColorManager.instance.size.y - 1);
        }
    }
    public Color GetColor(int _index)
    {
        if (_index >= colorPos.Length || _index < 0 || !ColorManager.isValid())
        {
            return Color.cyan;
        }
        return ColorManager.instance.GetTextureColor(colorPos[_index]);
    }
}
