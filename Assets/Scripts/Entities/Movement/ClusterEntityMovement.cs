using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClusterEntityMovement : OrbitalEnemyMovement
{
    [Space]
    [Header("Cluster Variables")]
    [Space]
    public GameObject[] clusterElements;
    // Start is called before the first frame update
    public override void Start()
    {
        if (clusterElements.Length > 0)
        {

            if (clusterElements[0] == gameObject)
            {
                if (defaultToPlayer && target == null)
                {
                    target = GlobalManager.Player;

                }
                for (int i = 1; i < clusterElements.Length; i++)
                {
                    ClusterEntityMovement tempRef = clusterElements[i].GetComponent<ClusterEntityMovement>();
                    tempRef.target = target;
                    tempRef.moveSpeed = moveSpeed * 2;

                }
            }
        }
        else if (defaultToPlayer && target == null)
        {
            target = GlobalManager.Player;

        }
    }

    // Update is called once per frame
    public override void Update()
    {


        base.Update();
        
    }

}
