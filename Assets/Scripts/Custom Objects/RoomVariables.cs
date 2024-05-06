using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Custom Objects/Room Variables")]
public class RoomVariables : ScriptableObject
{
    public Sprite R_entranceSprite;
    GlobalManager.roomType R_type;
    RoomPreset R_Preset;
}
