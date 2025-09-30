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
    [SerializeField] protected ParticleController particleControllerRef;
    [SerializeField] private Slider healthBar;
    private int currentHealth;
    [SerializeField] private int maxHealth;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float lowHealthThreshhold = 0.1f;

    protected bool dead = false;
    [SerializeField] protected int deathParticleIndex;
    [SerializeField] protected int lowHealthParticleIndex;
    [SerializeField] protected float killTime;
    public bool testParticles;

    public virtual void Start()
    {

        if (LevelGeneration.instance != null) LevelGeneration.instance.GenerateDitherMask(transform, Vector2.zero);
        if (particleControllerRef == null)
        {
            if (!TryGetComponent(out particleControllerRef))
            {
                Debug.LogWarning("No particle controller found. Please manually asign in inspector");
            }
        }
        SetHealth(maxHealth, false);
    }
    public virtual void Update()
    {

        if (testParticles)
        {
            UpdateHealth(-10);
            testParticles = false;
        }
    }
    //---------------------------------------------------------

    #region Set Health Functions
    //=========================================================
    //Set Health Functions

    private void SetHealth(int _mHealth, bool flash = true)
    //sets the max health and current health, takes one health variable for both max and current health
    {
        SetHealth(_mHealth, _mHealth, flash);

    }
    private void SetHealth(int _mHealth, int _cHealth, bool flash = true)
    //Overload 1, takes two ints, max health and current health
    {

        //set the max health via function
        SetMaxHealth(_mHealth);

        //set current health via function
        SetCurrentHealth(_cHealth, flash);
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
        if (_val < 1) _val = 1;

        maxHealth = _val;
    }
    public virtual void SetCurrentHealth(int _val, bool flash = true)
    {
        _val = Math.Clamp(_val, 0, maxHealth);
        currentHealth = _val;

        float v = (float)currentHealth / (float)maxHealth;
        if (v <= lowHealthThreshhold || currentHealth == 1)
        {
            particleControllerRef.StartParticle(lowHealthParticleIndex);
        }
        else
        {
            particleControllerRef.StopParticle(lowHealthParticleIndex);
        }

        if (healthBar != null)
        {
            healthBar.value = v;
        }

        if (currentHealth == 0) Kill();

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

    public virtual void Kill()
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
    public virtual void RemoveFromScope()
    {
        Destroy(gameObject);
        if (EnemyCoordinator.instance != null) EnemyCoordinator.instance.RemoveFromActive(gameObject);
    }

}
