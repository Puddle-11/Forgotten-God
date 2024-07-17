using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
   public GameObject target;
    [SerializeField] private bool defaultToPlayer = true;
    public float rotationSpeed;
    private void Start()
    {
        EntityMovement outE;
        if (TryGetComponent<EntityMovement>(out outE) && outE.target != null)
        {

            target = GetComponent<EntityMovement>().target;
        }
        else if (target == null && GlobalManager.Player != null && defaultToPlayer)
        {
            target = GlobalManager.Player;
        }
    }
    void Update()
    {
        if (target != null)
        {
            Vector2 Direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

    }
}
