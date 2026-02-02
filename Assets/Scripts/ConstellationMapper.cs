using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ConstellationMapper : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private Vector2Int lineCountRange;
    [SerializeField] private Vector2Int starCount;
    [SerializeField] private GameObject starPrefab;
    private List<GameObject> stars = new List<GameObject>();
    [SerializeField] private LineRenderer ln;
    public List<int> lineList = new List<int>();
    private void Start()
    {

        GenerateStarSystem();

    }

    private void ClearTrash()
    {
        if (stars == null) return;
        if (stars.Count <= 0) return;
        for (int i = 0; i < stars.Count; i++)
        {
            Destroy(stars[i]);
        }
        stars.Clear();
        lineList.Clear();
    }
    public void GenerateStarSystem()
    {
        ClearTrash();
        int collapsedStarCount = Random.Range(starCount.x, starCount.y);
        int collapsedLineCount = Random.Range(lineCountRange.x, lineCountRange.y);

        
        for (int i = 0; i < collapsedStarCount; i++)
        {
            stars.Add(Instantiate(starPrefab, Random.insideUnitCircle * range + (Vector2)transform.position, Quaternion.identity, transform));
        }
        GameObject currentStar = stars[0];
        lineList.Add(0);
        for (int i = 0; i < collapsedLineCount; i++)
        {
            float currDist = Mathf.Infinity;
            bool loopDone = false;
            for (int j = 0; j < collapsedStarCount; j++)
            {
                if (loopDone)
                {
                    continue;
                }
                if (stars[j] == currentStar)
                {
                    continue;
                }
                float newDist = Vector2.Distance(currentStar.transform.position, stars[j].transform.position);
                if (newDist < currDist)
                {
                    if (lineList.Contains(j))
                    {
                        //list has visited this star before, check to see if its too close

                        continue;

                    }
                    currDist = newDist;
                    currentStar = stars[j];
                    lineList.Add(j);
                    loopDone = true;

                }

            }
        }
        List<Vector3> pos = new List<Vector3>();
        for (int i = 0; i < lineList.Count; i++)
        {
            pos.Add(stars[lineList[i]].transform.localPosition);

        }
        ln.positionCount = pos.Count;

        ln.SetPositions(pos.ToArray());
    }

}
