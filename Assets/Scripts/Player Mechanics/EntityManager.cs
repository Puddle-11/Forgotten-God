using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class EntityManager : MonoBehaviour
{
    [SerializeField] private ParticleController particleControllerRef;
    [SerializeField] private Slider healthBar;
    private int currentHealth;

    [SerializeField] private int maxHealth;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float lowHealthThreshhold = 0.1f;

    private bool dead = false;
    [SerializeField] private int deathParticleIndex;
    [SerializeField] private int lowHealthParticleIndex;
    [SerializeField] private float killTime;
    public bool testParticles;
    private void Start()
    {
        SetHealth(maxHealth);
    }
    public void Update()
    {
        if (testParticles)
        {
            UpdateHealth(-10);

            testParticles = false;
        }
    }
    public void SetHealth(int _health)
    {
        if (_health < 1) _health = 1;
        maxHealth = _health;
        currentHealth = _health;

    }
    public void SetHealth()
    {
        SetHealth(maxHealth);
    }
    public void UpdateHealth(int _val)
    {
        _val = Math.Clamp(_val, 0, maxHealth);
        currentHealth = _val;

        
            if (currentHealth / maxHealth <= lowHealthThreshhold || currentHealth == 1)
            {

                particleControllerRef.StartParticle(lowHealthParticleIndex);
            }
            else
            {
                particleControllerRef.StopParticle(lowHealthParticleIndex);
            }
        

        

        float v = (float)currentHealth / (float)maxHealth;
        if (healthBar != null)
        {
            healthBar.value = v;
        }
        
        if(currentHealth == 0) Kill();

    }
    //=========================================================
    #endregion

    //---------------------------------------------------------

    #region Update Health
    //=========================================================
    //Update health Function
    public void UpdateHealth(int _val)
    {
        SetCurrentHealth(currentHealth + _val);
    }
    //=========================================================
    #endregion

    //---------------------------------------------------------
    public bool isAlive()
    {
        return !dead;
    }
    public void Kill()
    {
        if (dead) return;
        Tentacle[] tentacleArr = GetComponentsInChildren<Tentacle>();
        for (int i = 0; i < tentacleArr.Length; i++)
        {
            tentacleArr[i].targetDist = 0;
        }
        particleControllerRef.StopParticle(lowHealthParticleIndex);
        particleControllerRef.ChangeParent(deathParticleIndex, null);
        particleControllerRef.StartParticle(killTime, deathParticleIndex, RemoveFromScope);
        
        particleControllerRef.RemoveParticle(deathParticleIndex, -1);

        dead = true;

    }
    public void RemoveFromScope()
    {
        Destroy(gameObject);
    }
    
}
