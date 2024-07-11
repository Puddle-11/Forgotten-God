using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
   public GameObject target;
    public SpriteRenderer Sprite;
    [Space]
    [Header("Rotation")]
    [Space]
    public float rotationSpeed;
    [Space]
    [Header("Movement")]
    [Space]
    public float MinDist;
    public float moveSpeed;
    private float currentSpeed;
    [SerializeField] private float AccelerationSpeed;

    private Vector2 Direction;
    public void Start()
    {
        if (target == null && GlobalManager.Player != null)
        {


            target = GlobalManager.Player;


        }

    }
    private void FixedUpdate()
    {
        
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
            /*
            Direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            */


        }

    }
}
