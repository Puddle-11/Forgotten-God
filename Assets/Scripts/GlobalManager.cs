using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public KeyCode layerSwapKey;
    public static GlobalManager globalManagerRef;
    public static LayerManager layerManagerRef;
    public static GameObject Player;
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
