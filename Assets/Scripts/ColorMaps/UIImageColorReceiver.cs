using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]

public class UIImageColorReceiver : MonoBehaviour
{

    private ObjectPaletteManager objPalette;
    private Image ImRef;
    [SerializeField] private int colorIndex;
    private void Awake()
    {
        ImRef = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent<ObjectPaletteManager>(out objPalette) || !transform.parent.TryGetComponent<ObjectPaletteManager>(out objPalette))
        {

            objPalette = GetComponentInParent<ObjectPaletteManager>();
        }
        if (objPalette != null)
        {
            ImRef.color = objPalette.GetColor(colorIndex);
        }
        else
        {
            Debug.LogWarning("Failed to get Object Palette Manager on Obejct: " + gameObject.name + " ID: " + gameObject.GetInstanceID());
        }
    }
}
