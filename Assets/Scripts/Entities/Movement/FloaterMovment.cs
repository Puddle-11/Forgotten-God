using UnityEngine;

public class FloaterMovment : EntityMovement
{
    float standardY;
    [SerializeField] private float deviation;
    [SerializeField] private float sinSpeed;
    [SerializeField] private float rotAmount;
    [SerializeField] private float rotOffset;
    [SerializeField] private float moveDirection;
    private float lastY;
    public override void Start()
    {
        standardY = transform.position.y;
        base.Start();
    }
    public override void Move()
    {
        float raw = Mathf.Sin(Time.time * sinSpeed);
        
        raw *= deviation;
        float currY = standardY + raw;
        transform.position = new Vector3(transform.position.x + moveDirection * moveSpeed * Time.deltaTime, currY, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Cos(Time.time * sinSpeed) * -rotAmount);
        lastY = currY;

    }
    public override float CalculateSpeed()
    {
        return moveSpeed;
    }
}
