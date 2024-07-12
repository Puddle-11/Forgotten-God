using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityManager))]
public class EnemyHealthBar : MonoBehaviour
{

    private EntityManager EM;

    // Start is called before the first frame update
    private void Awake()
    {
        EM = GetComponent<EntityManager>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
