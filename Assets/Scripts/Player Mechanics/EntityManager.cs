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
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    private bool dead = false;
    [SerializeField] private int deathParticleIndex;
    [SerializeField] private int lowHealthParticleIndex;
    [SerializeField] private float killTime;
    public bool testParticles;
    
    private void Start()
    {
        if (particleControllerRef == null)
        {
            if (!TryGetComponent<ParticleController>(out particleControllerRef))
            {
                Debug.LogWarning("No particle controller found. Please manually asign in inspector");
            }
        }
            SetHealth(maxHealth);
    }
    public void Update()
    {
        if (testParticles)
        {
            SetCurrentHealth(50);
            testParticles = false;
        }
    }
    //---------------------------------------------------------

    #region Set Health Functions
    //=========================================================
    //Set Health Functions

    private void SetHealth(int _mHealth) 
    //sets the max health and current health, takes one health variable for both max and current health
    {
        SetHealth(_mHealth, _mHealth);

    }
    private void SetHealth(int _mHealth, int _cHealth) 
    //Overload 1, takes two ints, max health and current health
    {
        //clamp max health to min 1
        if (_mHealth < 1) _mHealth = 1; 
        //set the max health via function
        SetMaxHealth(_mHealth);
        //clamp the current health between max and 0
        _cHealth = Math.Clamp(_cHealth, 0, maxHealth);
        //set current health via function
        SetCurrentHealth(_cHealth);
    }
    //=========================================================
    #endregion 

    //---------------------------------------------------------

    #region Set Health Sub-Functions
    //=========================================================
    //Set Health Sub-Functions
    //Set Max health and Set current Health
    public void SetMaxHealth(int _val)
    {
        maxHealth = _val;
    }
    public void SetCurrentHealth(int _val)
    {
        _val = Math.Clamp(_val, 0, maxHealth);
        currentHealth = _val;
        if (currentHealth <= maxHealth / 10 || currentHealth== 1)
        {
            particleControllerRef.StartParticle(lowHealthParticleIndex);
        }
        else
        {
            particleControllerRef.StopParticle(lowHealthParticleIndex);
        }
        float v = (float)currentHealth / (float)maxHealth;

        healthBar.value = v;
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
