using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private string[] Tags;
    public int DamageAmount;
    [SerializeField] private bool ConstantDamage;
    [SerializeField] private bool GiveKnockback = true;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (ConstantDamage)
        {
            return;
        }

        foreach (string item in Tags)
        {
            if (collision.gameObject.tag == item)
            {
                ApplyDamage(collision.gameObject);
            }
            break;
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!ConstantDamage)
        {
            return;
        }

        foreach (string item in Tags)
        {
            if (collision.gameObject.tag == item)
            {
                ApplyDamage(collision.gameObject);
            }
            break;
        }
    }
    private void ApplyDamage(GameObject Target)
    {
        if (Target.TryGetComponent(out DamageReceiver TargetDR))
        {
            TargetDR.ApplyDamageToHealth(DamageAmount, transform, GiveKnockback);
        }
    }

}
