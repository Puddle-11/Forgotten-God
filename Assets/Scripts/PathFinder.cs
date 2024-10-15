using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{

    [SerializeField] private Tilemap tm;
    [SerializeField] private Vector2Int[] jumpHeightColumns;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2Int startingPosition;
    private List<Vector2Int> prevPos = new List<Vector2Int>();

    public struct MapperResults
    {
        public int totalTraversableTiles;
        public Vector2Int furthestLeft;
        public Vector2Int furthestRight;
        public Vector2Int[] allPositions;
    }
    public void SetStartingPosition(Vector3 _worldPos)
    {
        startingPosition = (Vector2Int)tm.WorldToCell(_worldPos);
    }
    public MapperResults StartFind()
    {
        MapperResults res = new MapperResults();
        ClearAll();
        Itterate(startingPosition, 200);
        res.furthestLeft = prevPos[0];
        res.furthestRight = prevPos[0];
        res.totalTraversableTiles = prevPos.Count;
        res.allPositions = prevPos.ToArray();
        if (prevPos.Count > 1)
        {
            foreach (Vector2Int vec in prevPos)
            {

                if (vec.x > res.furthestRight.x)
                {
                    res.furthestRight = vec;
                }

                if (vec.x < res.furthestLeft.x)
                {
                    res.furthestLeft = vec;
                }

            }
        }
        return res;
    }
    public void ClearAll()
    {
        prevPos.Clear();
    }
    public void Itterate(Vector2Int _currPos, int decay)
    {
        if (tm == null) return;
        decay--;

        if (decay > 0)
        {
            if (tm.GetTile((Vector3Int)_currPos + (Vector3Int)Vector2Int.down) == null)
            {
                Vector2Int temp = new Vector2Int(_currPos.x, _currPos.y - 1);
                if (!AlreadySearched(temp))
                {
                    prevPos.Add(temp);
                    Itterate(temp, decay);
                }
            }
            if (tm.GetTile((Vector3Int)_currPos + (Vector3Int)Vector2Int.down) != null)
            {
                //=============================
                //Side to Side check
                if (tm.GetTile((Vector3Int)_currPos + (Vector3Int)Vector2Int.right) == null)
                {
                    Vector2Int temp = new Vector2Int(_currPos.x + 1, _currPos.y);
                    if (!AlreadySearched(temp))
                    {
                        prevPos.Add(temp);
                        Itterate(temp, decay);
                    }
                }
                //-----------------------------
                if (tm.GetTile((Vector3Int)_currPos + (Vector3Int)Vector2Int.left) == null)
                {
                    Vector2Int temp = new Vector2Int(_currPos.x - 1, _currPos.y);
                    if (!AlreadySearched(temp))
                    {

                        prevPos.Add(temp);
                        Itterate(temp, decay);
                    }
                }


                //=============================



                //=============================
                //Jump Check
                if (CanJump(out Vector2Int _leftPos, _currPos, -1))
                {
                    if (!AlreadySearched(_leftPos))
                    {
                        prevPos.Add(_leftPos);
                        Itterate(_leftPos, decay);
                    }
                }
                if (CanJump(out Vector2Int _newPos, _currPos, 1))
                {
                    if (!AlreadySearched(_newPos))
                    {
                        prevPos.Add(_newPos);
                        Itterate(_newPos, decay);
                    }
                }
                //=============================

            }
        }

    }

    public bool AlreadySearched(Vector2Int _val)
    {
        return prevPos.Contains(_val);
    }
    public bool CanJump(out Vector2Int _newPos, Vector2Int _currPos, int _dir)
    {
        List<Vector2Int> potentialPoints = new List<Vector2Int>();
        for (int j = 0; j < jumpHeightColumns.Length; j++)
        {
            for (int i = jumpHeightColumns[j].x; i <= jumpHeightColumns[j].y; i++)
            {
                if (i > 1)
                {
                    if (tm.GetTile((Vector3Int)(_currPos + new Vector2Int(0, i))) != null)
                    {
                        break;
                    }
                }
                if (tm.GetTile((Vector3Int)(_currPos + new Vector2Int(_dir * (j + 1), i))) == null && tm.GetTile((Vector3Int)(_currPos + new Vector2Int(_dir * (j + 1), i - 1))) != null)
                {
                    //Do raycast check from curr to y then from new curr to x
                    //curr to y
                    Vector3 yRayOrigin = tm.CellToWorld((Vector3Int)_currPos) + Vector3.one;
                    Vector3 xRayOrigin = tm.CellToWorld((Vector3Int)(_currPos + new Vector2Int(0, i))) + Vector3.one;


                    if (i < 0)
                    {
                        xRayOrigin = tm.CellToWorld((Vector3Int)(_currPos)) + Vector3.one;
                        yRayOrigin = tm.CellToWorld((Vector3Int)(_currPos + new Vector2Int(_dir * (j + 1), 0))) + Vector3.one;
                    }

                    float distToY = Vector2.Distance((Vector2)tm.CellToWorld((Vector3Int)_currPos), (Vector2)tm.CellToWorld((Vector3Int)(_currPos + new Vector2Int(0, i))));
                    float distToX = Vector2.Distance((Vector2)tm.CellToWorld((Vector3Int)_currPos), (Vector2)tm.CellToWorld((Vector3Int)(_currPos + new Vector2Int(_dir * (j + 1), 0))));


                    //Debug.DrawRay(xRayOrigin, Vector2.right * distToX * dir, Color.red, 5);
                    //Debug.Log(distToX);
                    //Debug.DrawRay(yRayOrigin, Vector2.up * distToY, Color.red, 5);



                    RaycastHit2D hitToY = Physics2D.Raycast(yRayOrigin, Vector3.up, distToY, groundLayer);
                    RaycastHit2D hitToX = Physics2D.Raycast(xRayOrigin, Vector3.right * _dir, distToX, groundLayer);
                    if (hitToY.collider != null || hitToX.collider != null)
                    {
                        continue;
                    }
                    potentialPoints.Add(_currPos + new Vector2Int(_dir * (j + 1), i));

                    


                }
            }
        }
        if (potentialPoints.Count <= 0)
        {
            _newPos = Vector2Int.zero;
            return false;
        }
        else
        {
            Vector2Int Final = potentialPoints[0];
            for (int i = 0; i < potentialPoints.Count; i++)
            {
                if (potentialPoints[i].y > Final.y)
                {
                    Final = potentialPoints[i];
                }
            }
            _newPos = Final;

            return true;
        }
    }

    private void OnDrawGizmos()
    {
        if (tm != null)
        {

            if (prevPos.Count > 0)
            {

                for (int j = 0; j < prevPos.Count; j++)
                {

                    Gizmos.DrawCube(tm.CellToWorld((Vector3Int)prevPos[j]) + Vector3.one + Vector3.forward * j, Vector3.one);
                }
                Gizmos.color = Color.green;

            }
            Gizmos.color = Color.white;

            Gizmos.DrawCube(tm.CellToWorld((Vector3Int)startingPosition) + Vector3.one, Vector3.one);
        }
    }
}
