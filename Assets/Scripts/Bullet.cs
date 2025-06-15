using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public BulletType m_bulletType;
    //general settings
   [SerializeField] private float m_baseBulletSpeed;
   [SerializeField] private float m_bulletKillTime;
    //curved bullet settings


    [HideInInspector] public float m_curveAmount;
    //firework bullet settings
    [HideInInspector] public int m_splitAmount { private set; get; }
    //tracking bullet settings
    [HideInInspector] public float m_trackingSpeed { private set; get; }





   


    [System.Serializable]
    public enum BulletType
    {

        Straight,
        Curved,
        Firework,
        Tracking,

    };


}
