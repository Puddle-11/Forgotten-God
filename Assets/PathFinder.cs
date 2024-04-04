using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Tilemap MainMap;
    [SerializeField] private Tilemap DisplayMap;
    [SerializeField] private TileBase DisplayTile;
    [SerializeField] private TileBase DisplayTile2;
    private Vector2Int finalPosition;
    [SerializeField] private float speed;
    [SerializeField] private int jumpHeight;
    [SerializeField] private int[] jumpDistanceMatrix;
    [SerializeField] private Vector2Int currentPos;
    [SerializeField] private TileBase AirTile;
    [SerializeField] private TileBase GroundTile;
    [SerializeField] private Transform StartingPosTrans;
    [SerializeField] private int maxFallDist;
    [SerializeField] private List<Vector2Int> PreviousPath = new List<Vector2Int>();
    [SerializeField] private List<Vector2Int> activeBreakpoints;
    [SerializeField] private List<Vector2Int> deactiveBreakpoints = new List<Vector2Int>();
    public bool BreakPoint;
    public bool StartFind;
    private void Start()
    {
        Vector2Int temp = new Vector2Int();
      
    }
    private void Update()
    {
        if (StartFind)
        {
            StartFindAlgorithm();
            StartFind = false;
        }


    }
    private void StartFindAlgorithm()
    {
        PreviousPath.Clear();
        currentPos = (Vector2Int)MainMap.WorldToCell(StartingPosTrans.position);
            finalPosition = new Vector2Int(-10000, 0);
            StartCoroutine(FindNextPath(false));

    }
    public IEnumerator FindNextPath(bool _breakPoint)
    {
        yield return new WaitForSeconds(speed);
        Vector2Int nextPos = currentPos;
        bool grounded = MainMap.GetTile(new Vector3Int(currentPos.x, currentPos.y - 1, 0)) == GroundTile ? true : false;
        bool Finish = false;
         BreakPoint = false;



        #region results
        Vector3Int fallRes = CalculateFall(currentPos);
        Vector3Int walkRes = Move(currentPos);
        Vector3Int jumpRes = CalculateJump(currentPos);
        Vector3Int overrideWalkRes = overrideMove(currentPos);
        #endregion


        if (fallRes.z == 0)
        {
            nextPos = (Vector2Int)fallRes;
        }
        else if (walkRes.z == 0)
        {

            nextPos = (Vector2Int)walkRes;
        }
        else if (jumpRes.z == 0 && _breakPoint == false)
        {
            nextPos = (Vector2Int)jumpRes;
            bool contains = false;
            for (int i = 0; i < deactiveBreakpoints.Count; i++)
            {
                if (deactiveBreakpoints[i] == currentPos)
                {
                    contains = true;
                }
            }
            if (contains == false)
            {
                activeBreakpoints.Add(currentPos);
            }

        }
        else if (overrideWalkRes.z == 0)
        {

            nextPos = (Vector2Int)overrideWalkRes;
        }
        else if(jumpRes.z == 0)
        {
            nextPos = (Vector2Int)jumpRes;
         
        }
        else if (activeBreakpoints.Count > 0)
        {
            nextPos = activeBreakpoints[activeBreakpoints.Count - 1];
            deactiveBreakpoints.Add(activeBreakpoints[activeBreakpoints.Count - 1]);
            activeBreakpoints.RemoveAt(activeBreakpoints.Count - 1);
            BreakPoint = true;
        }
        else
        {
            Finish = true;
        }
        currentPos = nextPos;
        if (currentPos.x > finalPosition.x)
        {
            finalPosition = currentPos;
        }
        DisplayMap.ClearAllTiles();
        DisplayMap.SetTile((Vector3Int)currentPos, DisplayTile);
       
            PreviousPath.Add(currentPos);
        
        if (!Finish)
        {
            StartCoroutine(FindNextPath(BreakPoint));
        }
        else
        {
            currentPos = finalPosition;
            DisplayMap.ClearAllTiles();
            DisplayMap.SetTile((Vector3Int)currentPos, DisplayTile);
        }

    }



    public Vector3Int Move(Vector2Int _pos)
    {



        if (MainMap.GetTile(new Vector3Int(_pos.x + 1, _pos.y, 0)) == AirTile && MainMap.GetTile(new Vector3Int(_pos.x + 1, _pos.y - 1, 0)) == GroundTile)
        {
            return new Vector3Int(_pos.x + 1, _pos.y, 0);
        }
        return new Vector3Int(0,0,-1);
    }
    public Vector3Int overrideMove(Vector2Int _pos)
    {
        if (CalculateFall(new Vector2Int(_pos.x + 1, _pos.y)).z == 0)
        {
            if (MainMap.GetTile(new Vector3Int(_pos.x + 1, _pos.y, 0)) == AirTile)
            {
                return new Vector3Int(_pos.x + 1, _pos.y, 0);
            }
        }
        return new Vector3Int(0, 0, -1);
    }
    public Vector3Int CalculateJump(Vector2Int _pos)
    {

        List<bool> RowsFilled = new List<bool>();
        for (int y = 0; y > -jumpDistanceMatrix.Length; y--)
        {
            RowsFilled.Add(true);
            if (-y > 0 && RowsFilled[-y - 1] == true) continue;
            for (int x = 0; x < jumpDistanceMatrix[-y]; x++)
            {
                if(MainMap.GetTile(new Vector3Int(_pos.x + x + 1, _pos.y + y + 1, 0)) == AirTile)
                {
                    RowsFilled[-y] = false;
                }
                if (MainMap.GetTile(new Vector3Int(_pos.x + x + 1, _pos.y + y + 1, 0)) == GroundTile)
                {
                    if (MainMap.GetTile(new Vector3Int(_pos.x + x + 1, _pos.y + y + 2, 0)) == AirTile)
                    {
                        DisplayMap.SetTile(new Vector3Int(_pos.x + x + 1, _pos.y + y + 1, 0), DisplayTile2);
                        return new Vector3Int(_pos.x + x + 1, _pos.y + y + 2, 0);
                    }
                    break;
                }
            }
        }


        return new Vector3Int(0,0,-1);

    }
    public void CheckDown(int TopY, int bottomY, int startX)
    {

    }
    public Vector3Int CalculateFall(Vector2Int _pos)
    {


        for (int y = _pos.y; y > _pos.y - maxFallDist; y--)
        {
            if (MainMap.GetTile(new Vector3Int(_pos.x, y - 1, 0)) == GroundTile)
            {
                if (y == _pos.y)
                {
                    return new Vector3Int(0, 0, -1);

                }
                return new Vector3Int(_pos.x,y, 0);
            }
        }
        return new Vector3Int(0,0,-1);
    }


    private void OnDrawGizmos()
    {
        for (int i =0; i < PreviousPath.Count - 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(MainMap.CellToWorld((Vector3Int)PreviousPath[i]) + (Vector3.one ), MainMap.CellToWorld((Vector3Int)PreviousPath[i + 1]) + (Vector3.one ));
        }
    }
    public Vector2Int findNearestFreeSpot(Vector2Int _pos, int _searchDist)
    {

        if(MainMap.GetTile((Vector3Int)_pos) == null) return _pos;
        
        Vector2Int res = new Vector2Int(10000, 10000);
        bool foundSpot = false;



        for (int x = -_searchDist; x < _searchDist; x++)
        {
            for (int y = -_searchDist; y < _searchDist; y++)
            {
                if (MainMap.GetTile(new Vector3Int(x,y, 0)) == AirTile)
                {
                    if(Vector2.Distance(_pos, new Vector2(x,y)) > Vector2.Distance(_pos, res))
                    {
                        foundSpot = true;
                        res = new Vector2Int(x, y);
                    }
                }


            }
        }
        

        if(foundSpot == true)
        {
        return res;

        }
        else
        {
            return _pos;
        }
    }
}
