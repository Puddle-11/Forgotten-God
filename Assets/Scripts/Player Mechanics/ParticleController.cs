using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;

    public void StartParticle(int _index)
    {
        PlayParticle(_index);
    }
    public void StartParticle(float _delay, int _index, Action _postParticleAction)
    {
        StartCoroutine(ParticleAction(_delay, _index, _postParticleAction));
    }
    private IEnumerator ParticleAction(float _delay, int _index, Action _postParticleAction)
    {
        PlayParticle(_index);
        yield return new WaitForSeconds(_delay);
        if (_postParticleAction != null)
        {
            _postParticleAction();
        }
    }
    private void PlayParticle(int _index)
    {
        if (particles.Length > 0)
        {
            _index = Math.Clamp(_index, 0, particles.Length);
            particles[_index].Play();
        }
    }
    public void StopParticle(int _index)
    {
        if (particles.Length > 0)
        {
            _index = Math.Clamp(_index, 0, particles.Length);
            particles[_index].Stop();
        }
    }
   
}
