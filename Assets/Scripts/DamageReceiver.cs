using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private EntityManager EnMan;
    [SerializeField] private float defense = 1;
    [SerializeField] private GameObject hitParticles;
    public bool ShieldOn = false;
    public void ApplyDamageToHealth(int ammount, Transform HitTransform, bool TakeKnockback)
    {
        if (EnMan != null)
        {
            if (ShieldOn == false) {
                EnMan.UpdateHealth(-ammount);
                GameObject i = Instantiate(hitParticles);
                Vector2 dir = HitTransform.right;
                float angle = Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
                i.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90);
                i.transform.position = transform.position;
                Destroy(i, 5);
            }
        }
        else
        {
            Debug.LogWarning("Entity Manager not assigned to: " + gameObject.name);
        }
    
    }
 
}
