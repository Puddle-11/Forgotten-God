using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public GameObject target;
    public SpriteRenderer Sprite;
    [SerializeField] private bool defaultToPlayer = true;
    [Space]
    [Header("Movement")]
    [Space]
    public float MinDist;
    public float moveSpeed;
    private float currentSpeed;
    [SerializeField] private float AccelerationSpeed;

    public void Start()
    {
     

    }
    private void FixedUpdate()
    {
        currentSpeed = Vector2.Distance(transform.position, target.transform.position) > MinDist ? Mathf.MoveTowards(currentSpeed, moveSpeed, AccelerationSpeed) : Mathf.MoveTowards(currentSpeed, 0, AccelerationSpeed);
    }
    void Update()
    {
        if (target != null)
        {
            Vector2 TempPos = transform.position;
            if (moveSpeed > 0)
            {

                    TempPos = Vector2.MoveTowards(transform.position, target.transform.position, currentSpeed * Time.deltaTime);
                    transform.position = new Vector3(TempPos.x, TempPos.y, transform.position.z);
  
            }
        }

    }
}
