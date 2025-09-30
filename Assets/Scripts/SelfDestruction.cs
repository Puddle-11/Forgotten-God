using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    public float delay;
    private bool Destroying;
    [SerializeField] private bool DestroyTentacle;
    public bool DestroyOnEnable;
    public GameObject Particles;
    private Tentacle tentacleScr;
    private void Start()
    {
    }

    public void StartDestruction()
    {
        if (DestroyTentacle)
        {
            if (gameObject.TryGetComponent<Tentacle>(out tentacleScr))
            {
                tentacleScr.targetDist = 0;
            }
    
        }
        if(delay >= 0)
        {

            StartCoroutine(Destroydelay());
        }
    }
    IEnumerator Destroydelay()
    {
        Destroying = true;
        yield return new WaitForSeconds(delay);
        if (Particles != null)
        {
           GameObject i = Instantiate(Particles, transform.position, Quaternion.identity);
            i.GetComponent<ParticleSystem>().Play();

        }
        Destroy(gameObject);
    }
    public void ForceDestruction()
    {
        Destroying = true;
        if (Particles != null)
        {
            GameObject i = Instantiate(Particles, transform.position, Quaternion.identity);
            i.GetComponent<ParticleSystem>().Play();

        }



        Destroy(gameObject);


    }
    public void OnEnable()
    {
        if (Destroying)
        {
            StartCoroutine(Destroydelay());


        }
        if (DestroyOnEnable)
        {
            StartDestruction();

        }
    }

}
