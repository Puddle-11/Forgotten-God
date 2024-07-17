using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorReceiver : MonoBehaviour
{
    private ObjectPaletteManager objPalette;
    private SpriteRenderer SP;
    [SerializeField] private int colorIndex;
    private void Awake()
    {
        SP = GetComponent<SpriteRenderer>();

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
           SP.color = objPalette.GetColor(colorIndex);
        }
      
    }

  
}
