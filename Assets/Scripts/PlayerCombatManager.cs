using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombatManager : MonoBehaviour
{
    public int CurrentNumberOfAttacks = 1;
    public int MaxNumOfAttacks = 3;
    public List<Attacks> AllAttacks = new List<Attacks>();
    public Attacks SlotOne;
    public Attacks SlotTwo;
    public Attacks SlotThree;
    public Animator Anim;
    public Sprite[] SigilSprites;
    public GameObject SigilObj;
    public float MeleeAirHangtime;
    private void Start()
    {
    }
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.X))
        {

            if (SlotOne == null)
            {
                return;
            }
            if (SlotOne.CoolingDown == true)
            {
                return;
            }
            SlotOne.StartCoroutine(SlotOne.Delay());
            if (SlotOne.TypeOf == Attacks.Type.Melee)
            {

                Melee(SlotOne);
            }
            else if (SlotOne.TypeOf == Attacks.Type.Area)
            {
                Sigil();
                AOE(SlotOne);
            }
            else if (SlotOne.TypeOf == Attacks.Type.Summoner)
            {
                Sigil();
                Summon(SlotOne);

            }
        }

    }
    public void Summon(Attacks _Att)
    {


        Debug.Log("Test");
        bool TrackEnemies = _Att.SummonAtEnemy;
        float Radius = _Att.EnemySearchRange;
        GameObject Minion = _Att.SummonPrefab;
        int Quantity = _Att.Quantity;
        if (TrackEnemies)
        {
            bool foundEnemy = false;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, Radius);
            foreach (Collider2D item in enemies)
            {

                if (item.gameObject.tag == "Enemy")
                {

                    GameObject x = Instantiate(Minion, new Vector3(item.transform.position.x, item.transform.position.y, transform.position.z), Quaternion.identity);
                    x.GetComponent<FollowObj>().Target = item.gameObject.transform;
                    foundEnemy = true;
                }
            }
            if (foundEnemy == false)
            {


                for (int i = 0; i < Quantity; i++)
                {
                    Instantiate(Minion, new Vector3(Random.Range(transform.position.x - Radius, transform.position.x + Radius), Random.Range(transform.position.y - Radius, transform.position.y + Radius), transform.position.z), Quaternion.identity);


                }
            }
        }
        else
        {
            //spawn at player
        }
    }
    public void Melee(Attacks Att)

    {
        GameObject x;
        Vector2 Dir = Vector2.zero;
        

        Dir = GlobalManager.Player.GetComponent<PlayerAnimatorController>().SP.flipX ? new Vector2(-1, 0) : new Vector2(1, 0);

        if (Input.GetKey(KeyCode.DownArrow) && GlobalManager.Player.GetComponent<PlayerAnimatorController>().Grounded == false)
        {
            Dir = new Vector2(0, -1);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {

            Dir = new Vector2(0, 1);
        }


        Vector2 Offset = new Vector2(Dir.x * Att.OffsetM.x, Dir.y * Att.OffsetM.y);
        x = Instantiate(Att.AttackM, Vector3.zero, Quaternion.identity);

        float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        x.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Dir.x < 0) x.transform.localScale = new Vector3(1, -1, 1);
        
     
        x.transform.parent = GlobalManager.Player.transform;
        x.transform.localPosition = Offset;
        x.GetComponent<SelfDestruction>().StartDestruction();

    }
    public void AOE(Attacks _Att)
    {
        bool ParentToPlayer = _Att.ParentToPlayer;
        float Size = _Att.Size;
        GameObject Prefab = _Att.AOEPrefab;
        if (ParentToPlayer)
        {
            GameObject i = Instantiate(Prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            i.transform.localScale = i.transform.localScale * Size;
            i.transform.parent = transform;
        }
        else
        {
            GameObject i = Instantiate(Prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            i.transform.localScale = i.transform.localScale * Size;
        }
    }
    public void Magic()
    {


    }
    public void Range()
    {



    }
    public void Support()
    {

    }
    public void Sigil()
    {
        SigilObj.GetComponent<SpriteRenderer>().sprite = SigilSprites[Random.Range(0, SigilSprites.Length)];
        Anim.SetTrigger("Cast");


    }


}
