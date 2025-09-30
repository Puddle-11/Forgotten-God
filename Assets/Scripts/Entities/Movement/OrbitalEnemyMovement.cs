using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalEnemyMovement : EntityMovement
{
    [Space]
    [Header("Orbital Values")]
    [Space]
    [SerializeField] protected float lerpDist;
    [SerializeField] protected float timer;
    [SerializeField] protected float weight;
    protected const float C = Mathf.PI * 2;

    // Start is called before the first frame update
    private void Awake()
    {
        TryGetComponent<EntityManager>(out Enman);
    }

    // Update is called once per frame
    public override void Update()
    {



        UpdateTimer();
        base.Update();
    }
  
    public override float CalculateSpeed()
    {
        if (Enman != null && Enman.isAlive())
        {
            return moveSpeed;
        }
        else
        {
            return 0;
        }
    }
    public override void Move()
    {
        if (target == null) return;

        if (moveSpeed > 0)
        {

            Vector3 orbitPos = target.transform.position + new Vector3(-Mathf.Sin(timer), -Mathf.Cos(timer)) * MinDist;
            Vector3 targetPos = target.transform.position;
            //generates a value between 0 and 1 based on how close to the orbital radius we are, a distance = to MinDist + lerpDist (aka the outer lerp radius) == 0
            //while a distance = to min distance (aka the inner orbital circle) = 1;
            //this weight is used to calculate the average position between the target and the orbital radius
            weight = Mathf.Clamp((MinDist + lerpDist) - Vector2.Distance(transform.position, targetPos), 0, lerpDist) / lerpDist;
            Vector3 lerpPos = (orbitPos * weight + (targetPos) * (1 - weight));
            Vector3 tempPos = (Vector3)Vector2.MoveTowards(transform.position, lerpPos, currentSpeed * Time.deltaTime);
            transform.position = tempPos + new Vector3(0, 0, transform.position.z);
            //sets position to temppos

        }

    }
    public virtual void UpdateTimer()
    {
        if (weight <= 0)
        {
            //do sin cos calculations to get the proper angle
            Vector3 p1 = transform.position;
            Vector3 p2 = target.transform.position;
            float x = p2.x - p1.x;
            float y = p2.y - p1.y;
            float angle = Mathf.Atan2(x, y);
            timer = angle;
        }
        else
        {
            timer = timer > C ? 0 : timer + Time.deltaTime * (moveSpeed / MinDist) * weight;
        }
    }
   
}
