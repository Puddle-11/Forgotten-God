using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalEnemyMovement : EntityMovement
{
    [SerializeField] private float OrbitalSpeed = 1;
    [SerializeField] private float lerpDist;
    public float timer;
    public float weight;
  
    // Start is called before the first frame update
    private void Awake()
    {
        TryGetComponent<EntityManager>(out Enman);
    }
   
    // Update is called once per frame
    void Update()
    {
        
        if (timer > Mathf.PI * 2)
        {
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime * (moveSpeed / MinDist) * weight;
        }

        Move();
        currentSpeed = CalculateSpeed();
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
        if (target != null)
        {
            Vector2 TempPos = transform.position;
            if (moveSpeed > 0)
            {

                Vector3 orbitPos = target.transform.position + new Vector3(-Mathf.Sin(timer), -Mathf.Cos(timer)) * MinDist;
                Vector3 targetPos = target.transform.position;
                //generates a value between 0 and 1 based on how close to the orbital radius we are, a distance = to MinDist + lerpDist (aka the outer lerp radius) == 0
                //while a distance = to min distance (aka the inner orbital circle) = 1;
                //this weight is used to calculate the average position between the target and the orbital radius
                weight = Mathf.Clamp((MinDist + lerpDist) - Vector2.Distance(transform.position, targetPos), 0, lerpDist) / lerpDist;
                Vector3 lerpPos = (orbitPos * weight + targetPos * (1-weight));
                float distTotarget = Vector2.Distance(transform.position, targetPos);
                

                //TempPos = Vector2.MoveTowards(transform.position, target.transform.position + new Vector3(-Mathf.Sin(timer), -Mathf.Cos(timer), 0) * MinDist, currentSpeed * 10 * Time.deltaTime);
                
              
                TempPos = Vector2.MoveTowards(transform.position, lerpPos, currentSpeed * Time.deltaTime);

                
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

              
               
                transform.position = new Vector3(TempPos.x, TempPos.y, transform.position.z);

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.transform.position, MinDist);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.transform.position, MinDist + lerpDist);
    }
}
