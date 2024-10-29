using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class RoomExit : MonoBehaviour
{
    [SerializeField] private SpriteRenderer nextRoomIcon;
    private bool inPortal;
    private float timer;
    [SerializeField] private GameObject prompt;
    [SerializeField] private PerRoomVars _roomVars;
    private Interaction objectInteraction;

    public PerRoomVars GetRoomCars() { return _roomVars; }
    private void Start()
    {
        objectInteraction = new Interaction(EnterPortal, prompt, 1, 1, GlobalManager.globalManagerRef.interactionKey);
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GlobalManager.Player)
        {
            GlobalManager.globalManagerRef.GetInteractionManager().SetAction(objectInteraction);

           // GlobalManager.globalManagerRef.GetInteractionManager().SetAction(EnterPortal, 1, prompt, 1);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == GlobalManager.Player)
        {
            GlobalManager.globalManagerRef.GetInteractionManager().ClearAction(objectInteraction);
        }
    }
    private void EnterPortal()
    {
        LevelGeneration.instance.SetPerRoomVars(_roomVars);
       GlobalManager.globalManagerRef.GetUIManager().BeginFade(1, LevelGeneration.instance.Generate);
    }
}
