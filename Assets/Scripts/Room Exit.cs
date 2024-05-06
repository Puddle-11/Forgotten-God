using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour
{
    [SerializeField] private RoomVariables nextRoom;
    [SerializeField] private SpriteRenderer nextRoomIcon;

    private void Start()
    {
        
    }
    public void SetIcon()
    {
        nextRoomIcon.sprite = nextRoom.R_entranceSprite;
    }


}
