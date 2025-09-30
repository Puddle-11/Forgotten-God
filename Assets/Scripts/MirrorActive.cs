using UnityEngine;

public class MirrorActive : MonoBehaviour
{
    public GameObject target;
    [SerializeField] private SpriteMask receiver;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 

    // Update is called once per frame
    void Update()
    {
        if (target != null) 
        receiver.enabled = target.activeInHierarchy;
    }
}
