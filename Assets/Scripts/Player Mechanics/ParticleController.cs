using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;


public class ParticleController : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particles;

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
        if (IsIndexValid(_index))
        {
            particles[_index].Play();
        }
    }
 
    public void StopParticle(int _index)
    {
        if (particles.Count > 0 && IsIndexValid(_index))
        {
            particles[_index].Stop();
        }
    }
    public bool ChangeParent(int _index,Transform _newParent)
    {
        if (IsIndexValid(_index))
        {
            particles[_index].transform.parent = _newParent;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool RemoveParticle(int _index, float _deletionTime)
    {

        if (IsIndexValid(_index))
        {
            if (_deletionTime == -1)
            {
                Destroy(particles[_index].gameObject, particles[_index].main.duration + particles[_index].main.startLifetime.Evaluate(0) + particles[_index].main.startLifetime.Evaluate(1));
            }
            else
            {
                Destroy(particles[_index].gameObject, _deletionTime);

            }
            particles.RemoveAt(_index);
            return true;
        }
        else return false;
        

    }
    public bool RemoveParticle(int _index)
    {
        return RemoveParticle(_index, 0);
    }
    public bool IsIndexValid(int _index)
    {
        if (particles.Count > 0 && _index > 0 && _index < particles.Count)
        {
            if (particles[_index] == null)
            {
                Debug.LogWarning("Particle Not assigned at index " + _index + " On object: " + gameObject.name + " ID: " + gameObject.GetInstanceID());
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.LogWarning("Tried accessing a particle system outside the range");
            return false;

        }
    }
}
