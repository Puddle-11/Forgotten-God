using UnityEngine;

public class Dasher : EntityMovement
{
    private float timer = 0;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeed;

    public override void Move()
    {
        if (inRange)
        {

            if (timer > dashTime)
            {
                Vector2 dir = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
                RB.AddForce(dir * dashSpeed);
                timer = 0;
            }

            timer = timer + Time.deltaTime;

        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject == GlobalManager.Player)
        {
            RB.linearVelocity = -RB.linearVelocity;
        }
    }
}
