using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Custom Objects/Per Room Vars")]
public class PerRoomVars : ScriptableObject
{
    public GameObject enemyPool;
    public GameObject exitPrefabs;
    public Vector2Int exitCountRange;
    public Vector2Int enemyCountRange;
    public Vector2 difficultyRange;
}
