using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class Attacks : MonoBehaviour
{
    
    public Type TypeOf;
    public int Cost;



  



    //summoner Settings;

    [HideInInspector]
    public bool SummonAtEnemy;
    [HideInInspector]


    public float EnemySearchRange;
    [HideInInspector]

    public GameObject SummonPrefab;
    [HideInInspector]

    public int Quantity;


    [HideInInspector]

    public bool ParentToPlayer;
    [HideInInspector]

    public float Size;
    [HideInInspector]

    public GameObject AOEPrefab;

    public float Cooldown;

    public bool CoolingDown = false;
    [HideInInspector]

    private bool RunningDelay;

    [HideInInspector]
    public bool AutoLockM;
    [HideInInspector]

    public GameObject AttackM;
    [HideInInspector]

    public Vector2 OffsetM;
    
    public enum Type : int
    {
        Summoner = 1,
        Melee = 2,
        Ranged = 3,
        Magic = 4,
        Area = 5,
        Support = 6,



    }


    // Update is called once per frame
    void Update()
    {
        if(CoolingDown && RunningDelay == false)
        {
            StartCoroutine(Delay());

        }
    }
    private void OnEnable()
    {
        if (RunningDelay)
        {
            StartCoroutine(Delay());

        }
    }
    public IEnumerator Delay()
    {
        CoolingDown = true;

        RunningDelay = true;
        yield return new WaitForSeconds(Cooldown);
        RunningDelay = false;
        CoolingDown = false;
    }
    
#if UNITY_EDITOR
[CustomEditor(typeof(Attacks))]
    public class AttackCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
        base.OnInspectorGUI();

        Attacks AttacksCast = (Attacks)target;




        if(AttacksCast.TypeOf == Type.Summoner){
        DrawSummoner(AttacksCast);

        }
        else if(AttacksCast.TypeOf == Type.Melee){
                DrawMelee(AttacksCast);

        }
          else if(AttacksCast.TypeOf == Type.Ranged){
                  DrawRange(AttacksCast);

        }
          else if(AttacksCast.TypeOf == Type.Magic){
                            DrawMagic(AttacksCast);

        }
          else if(AttacksCast.TypeOf == Type.Area){
                            DrawAOE(AttacksCast);

        }
          else if(AttacksCast.TypeOf == Type.Support){
                            DrawSupport(AttacksCast);

        }
        

        }



    }
    static void DrawSummoner(Attacks AttacksCast)
    {
        EditorUtility.SetDirty(AttacksCast);

        EditorGUILayout.Space(20);

    EditorGUILayout.LabelField("SummonerSettings", EditorStyles.boldLabel);
    EditorGUILayout.Space();



    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Summon At Enemy", GUILayout.MaxWidth(145));
    AttacksCast.SummonAtEnemy = EditorGUILayout.Toggle(AttacksCast.SummonAtEnemy);
        EditorGUILayout.EndHorizontal();
        

      
        

        if (AttacksCast.SummonAtEnemy){
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Enemy Search Radius", GUILayout.MaxWidth(145));
        AttacksCast.EnemySearchRange = EditorGUILayout.FloatField(AttacksCast.EnemySearchRange);
                EditorGUILayout.EndHorizontal();


    }
     EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Summon Prefab", GUILayout.MaxWidth(145));
    AttacksCast.SummonPrefab = EditorGUILayout.ObjectField(AttacksCast.SummonPrefab, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();
           EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Minion Quantity", GUILayout.MaxWidth(145));
    AttacksCast.Quantity = EditorGUILayout.IntField(AttacksCast.Quantity);
        EditorGUILayout.EndHorizontal();


    }
    static void DrawMelee(Attacks AttacksCast){
        EditorUtility.SetDirty(AttacksCast);

        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("Melee Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Lock To Enemy", GUILayout.MaxWidth(145));
        AttacksCast.AutoLockM = EditorGUILayout.Toggle(AttacksCast.AutoLockM);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Attack Prefab", GUILayout.MaxWidth(145));
      
            GameObject x = EditorGUILayout.ObjectField(AttacksCast.AttackM, typeof(GameObject), true) as GameObject;
        AttacksCast.AttackM = x;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

       // EditorGUILayout.LabelField("Offset", GUILayout.MaxWidth(145));
        AttacksCast.OffsetM = EditorGUILayout.Vector2Field("Offset", AttacksCast.OffsetM);
        EditorGUILayout.EndHorizontal();

    }

    static void DrawAOE(Attacks AttacksCast){
        EditorUtility.SetDirty(AttacksCast);

        EditorGUILayout.Space(20);

    EditorGUILayout.LabelField("Area Of Effect Settings", EditorStyles.boldLabel);
    EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Parent To Player", GUILayout.MaxWidth(145));
        AttacksCast.ParentToPlayer = EditorGUILayout.Toggle(AttacksCast.ParentToPlayer);
                EditorGUILayout.EndHorizontal();

                  EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Area Size", GUILayout.MaxWidth(145));
        AttacksCast.Size = EditorGUILayout.FloatField(AttacksCast.Size);
                EditorGUILayout.EndHorizontal();

                 EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("Area Of Effect Prefab", GUILayout.MaxWidth(145));
    AttacksCast.AOEPrefab = EditorGUILayout.ObjectField(AttacksCast.AOEPrefab, typeof(GameObject), true) as GameObject;
        EditorGUILayout.EndHorizontal();

    }
    static void DrawMagic(Attacks AttacksCast){



    }
    static void DrawSupport(Attacks AttacksCast){


    }
    static void DrawRange(Attacks AttacksCast){


    }
#endif
}
