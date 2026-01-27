using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : EntityManager
{

    [SerializeField] private int m_flashColorIndex = -1;
    [SerializeField] private Color m_flashColor = Color.white;
    [SerializeField] private SpriteRenderer[] m_renderList;
    [SerializeField] private Tentacle[] m_tentacles;
    [SerializeField] private float flashDelay = 0.1f;
    [SerializeField] private EntityMovement m_movement;

    private bool flashing;


    public override void SetCurrentHealth(int _val, bool flash = true)
    {
        base.SetCurrentHealth(_val);
        if(flash) StartCoroutine(Flash());
    }
    public override void Start()
    {
        if(TryGetComponent(out ObjectPaletteManager pRef))
        {

         m_flashColor =   pRef.GetColor(m_flashColorIndex);
        }

        m_renderList = GetComponentsInChildren<SpriteRenderer>();
        m_tentacles = GetComponentsInChildren<Tentacle>();
        base.Start();
    }
    public override void TakeKnockback(Vector2 _dir, float _amount, float _duration)
    {
        m_movement.StartKnockback(_dir, _amount, _duration);
    }
    private IEnumerator Flash()
    {
        if (flashing) yield break;
        flashing = true;

        Color[] originalColors_S = new Color[m_renderList.Length];
        Color[,] originalColors_T = new Color[m_tentacles.Length, 2];

        for (int i = 0; i < m_renderList.Length; i++)
        {
            originalColors_S[i] = m_renderList[i].color;
            m_renderList[i].color = m_flashColor;
        }
        for (int i = 0; i < m_tentacles.Length; i++)
        {
            originalColors_T[i, 0] = m_tentacles[i].GetColors()[0];
            originalColors_T[i, 1] = m_tentacles[i].GetColors()[1];
            m_tentacles[i].SetColors(m_flashColor, m_flashColor);
        }

        yield return new WaitForSeconds(flashDelay);
        for (int i = 0; i < m_renderList.Length; i++)
        {
            m_renderList[i].color = originalColors_S[i];
        }
        for (int i = 0; i < m_tentacles.Length; i++)
        {
            m_tentacles[i].SetColors(originalColors_T[i, 0], originalColors_T[i, 1]);
        }
        flashing = false;
    }
    public override void Kill()
    {
        m_movement.StopVel();
        base.Kill();
    }
}
