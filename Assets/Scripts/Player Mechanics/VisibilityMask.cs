using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityMask : MonoBehaviour
{
    [SerializeField] private int layerIndex;
    [SerializeField] private GameObject Mask;
    void Update()
    {
        if(Mask.activeInHierarchy == true && layerIndex != LayerManager.CurrentLayer)
        {
            Mask.SetActive(false);

        }
        else if(Mask.activeInHierarchy == false && layerIndex == LayerManager.CurrentLayer)
        {
            Mask.SetActive(true);

        }
    }
}
