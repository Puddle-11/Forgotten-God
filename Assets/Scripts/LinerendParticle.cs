using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinerendParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem P1;
    [SerializeField] private ParticleSystem P2;
    [SerializeField] private float density;
    public void UpdateParticles(float _width, Vector2 _pos)
    {
        var Pshape1 = P1.shape;
        var PEm1 = P1.emission;
        Pshape1.radius = _width;
        PEm1.rateOverTime = density * _width;

        var Pshape2 = P2.shape;
        var PEm2 = P2.emission;
        Pshape2.radius = _width;
        PEm2.rateOverTime = density * _width;
        transform.position = _pos;
    }

}
