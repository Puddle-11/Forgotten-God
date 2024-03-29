using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Custom Objects/Animaton Map")]
public class LayerAnimatonMap : ScriptableObject
{
    public List<Sprite> targetMap;
    public List<Sprite> sourceMap;

}
