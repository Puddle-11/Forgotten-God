using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public KeyCode layerSwapKey;
    public KeyCode moveToEntranceKey;
    public KeyCode interactionKey;
    public static GlobalManager globalManagerRef;
    public LayerManager layerManagerRef;
    public UIManager UIManagerRef;
    public PlayerManager playerManagerRef;
    public InteractionManager interactionManagerRef;
    public static GameObject Player;

    public enum roomType
    {
        Item = 0,
        Battle = 1,
        Heal = 2,
        Boss = 3,
        Secret = 4,
        Gauntlet = 5,
        Rest = 6,
    }
    public InteractionManager GetInteractionManager()
    {
        if (interactionManagerRef == null)
        {
            interactionManagerRef = GetComponentInChildren<InteractionManager>();
        }
        return interactionManagerRef;

    }
    public UIManager GetUIManager()
    {
        if (UIManagerRef == null)
        {
            UIManagerRef = GetComponentInChildren<UIManager>();
        }
        return UIManagerRef;
    }
    public LayerManager GetLayerManager()
    {
        if(layerManagerRef == null)
        {
            layerManagerRef = GetComponentInChildren<LayerManager>();
        }
        return layerManagerRef;
    }
    public PlayerManager GetPlayerManager()
    {
        if(playerManagerRef == null)
        {
            playerManagerRef = Player.GetComponent<PlayerManager>();
        }
        return playerManagerRef;
    }
    // Start is called before the first frame update
    void Awake()
    {

        if(globalManagerRef == null)
        {
            globalManagerRef = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GetManagers();
    }
    private void GetManagers()
    {

        layerManagerRef = GetComponentInChildren<LayerManager>();
        if(layerManagerRef == null) Debug.LogWarning("Layer Manager Missing");

        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) Debug.LogWarning("Player Missing");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
