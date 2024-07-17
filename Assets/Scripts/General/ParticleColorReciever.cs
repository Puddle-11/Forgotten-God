using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorReciever : MonoBehaviour
{
    private ObjectPaletteManager objPalette;
    private ParticleSystem PS;
    [SerializeField] private int colorIndex;
    [SerializeField] private bool useGradient;
    private void Awake()
    {
        PS = GetComponent<ParticleSystem>();

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
            var mainMod = PS.main;
            mainMod.startColor = objPalette.GetColor(colorIndex);
            
        }

    }

}
