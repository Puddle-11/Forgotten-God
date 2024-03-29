using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Custom Objects/Decoration Presets")]
public class DecorationPreset : ScriptableObject
{
    public List<Decoration> Decorations;
    public enum DecorationType{
        Leaf,
        Grass,
        Vines,
    }
}
