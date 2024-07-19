using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleColorReceiver : MonoBehaviour
{
    private ObjectPaletteManager objPalette;
    private ParticleSystem PS;
    [SerializeField] private int colorIndex1;
    [SerializeField] private int colorIndex2;

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

            if (useGradient)
            {

                var lifetimeMod = PS.colorOverLifetime;
                lifetimeMod.enabled = true;
                mainMod.startColor = Color.white;
                Gradient g = new Gradient();
                GradientColorKey[] cKeys = new GradientColorKey[2];
                cKeys[0] = new GradientColorKey(objPalette.GetColor(colorIndex1), 0.0f);
                cKeys[1] = new GradientColorKey(objPalette.GetColor(colorIndex2), 1.0f);
                GradientAlphaKey[] aKeys = new GradientAlphaKey[1];
                aKeys[0] = new GradientAlphaKey(1.0f,0.0f);
                g.SetKeys(cKeys, aKeys);
                lifetimeMod.color = g;

            }
            else
            {
                mainMod.startColor = objPalette.GetColor(colorIndex1);
            }
        }
        else
        {
            Debug.LogWarning("Failed to get Object Palette Manager on Obejct: " + gameObject.name + " ID: " + gameObject.GetInstanceID());
        }

    }

}
