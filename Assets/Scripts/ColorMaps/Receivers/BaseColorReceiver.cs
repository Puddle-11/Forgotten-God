using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseColorReceiver : MonoBehaviour
{
    protected ObjectPaletteManager objPalette;
    [SerializeField] protected int colorIndex;
    

    void Start()
    {
        GetPalette();
        SetColor();
    }
    public virtual void SetColor()
    {

    }
    public void GetPalette()
    {
        if (objPalette != null) return;
        if (!TryGetComponent(out objPalette))
        {
            objPalette = GetComponentInParent<ObjectPaletteManager>();
        }
        if (objPalette != null)
        {
            
        }
        else
        {
            Debug.LogWarning("Failed to get Object Palette Manager on Obejct: " + gameObject.name + " ID: " + gameObject.GetInstanceID());
        }

    }

  
}
