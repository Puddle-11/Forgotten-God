using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{

    [SerializeField] private EntityManager EM;

    // Start is called before the first frame update
    private void Awake()
    {
        EM = GetComponent<EntityManager>();

    }

}
