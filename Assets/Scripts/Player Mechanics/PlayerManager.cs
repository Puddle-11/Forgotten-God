using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PlayerManager : EntityManager
{
    [SerializeField] private bool respawnable;
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


        Material mat = _val ? ColorManager.instance.playerLayerOneMat : ColorManager.instance.playerLayerTwoMat;
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
        if (LevelGeneration.instance != null && LevelGeneration.instance.GetCurrentEntrance() != null)
        {
            transform.position = GetRespawnPosition();
            GlobalManager.globalManagerRef.GetLayerManager().ChangeLayers(0);
        }
    }
    public Vector3 GetRespawnPosition()
    {
        return LevelGeneration.instance.GetCurrentEntrance().transform.position + (Vector3)entranceOffset;
    }
    
    public override void Kill()
    {
        if (dead) return;
        
        particleControllerRef.StopParticle(lowHealthParticleIndex);
        
        ParticleSystem newPS = Instantiate(particleControllerRef.GetParticle(deathParticleIndex).gameObject, transform.position,
                                           Quaternion.identity, null).GetComponent<ParticleSystem>();
        
        int index = particleControllerRef.AddParticle(newPS);
        
        particleControllerRef.ChangeParent(index, null);
        particleControllerRef.StartParticle(killTime, index, Respawn);
        
        particleControllerRef.RemoveParticle(index, -1);
    }
    
    public void Respawn()
    {
        
    }
    
}
