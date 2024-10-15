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
    private Interaction objectInteraction;
    private void Start()
    {
        objectInteraction = new Interaction(EnterPortal, prompt, 1, 1, GlobalManager.globalManagerRef.interactionKey);
    }
    private void Update()
    {
       
   
    }
    private void OnDisable()
    {
        
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
       GlobalManager.globalManagerRef.GetUIManager().BeginFade(1, LevelGeneration.instance.Generate);
    }
}
