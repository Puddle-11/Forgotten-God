using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private ParticleController particleControllerRef;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private bool dead = false;
    [SerializeField] private int deathParticleIndex;
    [SerializeField] private int lowHealthParticleIndex;
    public bool testParticles;
    private void Start()
    {
        SetHealth(10);
    }
    public void Update()
    {
        if (testParticles)
        {
            UpdateHealth(-9);
            testParticles = false;
        }
    }
    private void SetHealth(int _health)
    {
        if (_health < 1) _health = 1;
        maxHealth = _health;
        currentHealth = _health;

    }

    public void UpdateHealth()
    {
        SetHealth(maxHealth);
    }
    public void UpdateHealth(int _val)
    {
        int newHealth = Math.Clamp(_val + currentHealth, 0, maxHealth);
        currentHealth = newHealth;
        if (currentHealth <= maxHealth / 10 || currentHealth== 1)
        {

            particleControllerRef.StartParticle( lowHealthParticleIndex);
        }
        else
        {

            particleControllerRef.StopParticle(lowHealthParticleIndex);
        }
        if(currentHealth == 0) Kill();
        
    }
    public void Kill()
    {
        particleControllerRef.StartParticle(1, deathParticleIndex, RemoveFromScope);
        dead = true;

    }
    public void RemoveFromScope()
    {
        Destroy(this);
    }
    
}
