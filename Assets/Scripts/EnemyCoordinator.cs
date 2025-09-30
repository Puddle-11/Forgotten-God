using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemyCoordinator : MonoBehaviour
{
    public static EnemyCoordinator instance;
    [SerializeField] private float width, height;
    public bool summon;
    [SerializeField] private EnemyType[] enemyTypes;
    private List<GameObject> activeEnemies = new List<GameObject>();
    [System.Serializable]
    public struct EnemyType
    {
        public EnemyType(GameObject _obj, int _diff)
        {
            _object = _obj;
            _difficulty = _diff;
        }

        public GameObject _object;
        public float _difficulty;
    }
    [System.Serializable]
    public struct EnemyPool
    {
        public EnemyPool(EnemyType _et, int _c)
        {
            _type = _et;
            _count = _c;
        }
        public EnemyType _type;
        public int _count;
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (summon)
        {
            SummonEntities(GetPoolFromDifficulty(5));
            summon = false;
        }
    }

    public void RemoveFromActive(GameObject obj)
    {
        activeEnemies.Remove(obj);
        if(activeEnemies.Count <= 0)
        {
            LevelGeneration.instance.OpenExits();
        }
    }
    private void CleanEnemyList()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i] != null)
            {
                Destroy(activeEnemies[i]);
            }
        }
        activeEnemies.Clear();
    }

    private EnemyPool[] GetPoolFromDifficulty(float _difficulty)
    {
        List<EnemyPool> pool = new List<EnemyPool>();
        if (enemyTypes.Length <= 0) return pool.ToArray();
        float countedDifficulty = 0;
        int index;
        while (countedDifficulty < _difficulty)
        {
            index = UnityEngine.Random.Range(0, enemyTypes.Length);
            pool.Add(new EnemyPool(enemyTypes[index], 1));
            countedDifficulty += Mathf.Max(0.1f, enemyTypes[index]._difficulty);
        }
        return pool.ToArray();
    }
    public void SummonEntities(float _diff)
    {
        SummonEntities(GetPoolFromDifficulty(_diff));
    }
    public void SummonEntities()
    {

        SummonEntities(new EnemyPool[] { new EnemyPool(enemyTypes[0], 1) });
    }
    public void SummonEntities(EnemyPool[] _enemyArr)
    {
        CleanEnemyList();
        int count;
        Vector2 spawnPos;
        for (int q = 0; q < _enemyArr.Length; q++)
        {
            count = _enemyArr[q]._count;

            for (int i = 0; i < count; i++)
            {
                spawnPos = GetPosInBounds();
                activeEnemies.Add(Instantiate(_enemyArr[q]._type._object, spawnPos, Quaternion.identity, null));
            }
        }
    }
    private Vector2 GetPosInBounds()
    {
        return (Vector2)transform.position + new Vector2(UnityEngine.Random.Range(-width, width), UnityEngine.Random.Range(-height, height));
    }
    private void OnDrawGizmos()
    {
        Vector3[] points =
        {
           transform.position + new Vector3(width, height,0),
           transform.position + new Vector3(-width, height,0),
           transform.position + new Vector3(-width, -height,0),
           transform.position + new Vector3(width, -height,0)
        };
        ReadOnlySpan<Vector3> rsp = points.AsSpan();

        Gizmos.DrawLineStrip(rsp, true);

    }
}
