using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PlayerManager : EntityManager
{
   
    public TarodevController.PlayerController playerControllerRef;
    private bool inBlockLayerOne, inBlockLayerTwo;
    public Vector2 entranceOffset;
    [SerializeField] private float teleportDelay;
    [SerializeField] private SpriteRenderer[] ConnectedSprites;
    [SerializeField] private Image[] ConnectedImages;
    [SerializeField] private GameObject progressBar;
    [SerializeField] private float lowerThreshold;


    private float teleportTimer;
    private void Start()
    {
        GlobalManager.globalManagerRef.GetInteractionManager().staticInteractions.Add(new Interaction(MoveToEntrance, progressBar, teleportDelay, -1, GlobalManager.globalManagerRef.moveToEntranceKey));
    }
    public override void Update()
    {
        base.Update();
        RunSafety();

    }
    private void RunSafety()
    {
        if(transform.position.y < lowerThreshold)
        {
            MoveToEntrance();
        }
    }
    public void UpdateSprite(bool _val)
    {


        Material mat = _val ? ColorManager.CMref.playerLayerOneMat : ColorManager.CMref.playerLayerTwoMat;
        foreach (SpriteRenderer Sp in ConnectedSprites)
        {
            Sp.material = mat;
        }
        foreach (Image Im in ConnectedImages)
        {
            Im.material = mat;
        }
    }




    public void ChangeGround(LayerMask _newGround)
    {
        playerControllerRef.UpdateGround(_newGround);
    }
    public void ChangeSortingOrder(string id)
    {
        foreach (SpriteRenderer Sp in ConnectedSprites)
        {
        Sp.sortingLayerID = SortingLayer.NameToID(id);
        }
    }
    public void MoveToEntrance()
    {
        if (LevelGeneration.LevelGenRef != null && LevelGeneration.LevelGenRef.entrance != null)
        {
            transform.position = LevelGeneration.LevelGenRef.entrance.transform.position + (Vector3)entranceOffset;
            GlobalManager.globalManagerRef.GetLayerManager().ChangeLayers(0);
        }
    }
}
