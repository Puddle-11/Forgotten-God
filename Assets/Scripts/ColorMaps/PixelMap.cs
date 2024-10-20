using Common;
using UnityEngine;
using UnityEngine.Rendering;

namespace Editor
{
    public class PixelMap : ScriptableObject
    {
        public Common.SerializedDictionary<Color32, Vector2Int> lookup = new();
        public Color32[] data;
    }
}