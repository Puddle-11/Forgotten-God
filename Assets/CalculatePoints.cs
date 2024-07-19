using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class CalculatePoints : MonoBehaviour
{
    [SerializeField] private GameObject[] points;
    private float[] angles;
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    const float C = Mathf.PI * 2;
    // Start is called before the first frame update
    void Start()
    {
        angles = new float[points.Length];

    }

    // Update is called once per frame
    void Update()
    {
        angles = calcPoints(angles);
        
        for (int i = 0; i < points.Length; i++)
        {
            points[i].transform.position = new Vector3(Mathf.Sin(angles[i]), Mathf.Cos(angles[i]), 0) * distance;
        }
    }
    float[] calcPoints(float[] currentAngles)
    {


        currentAngles[0] = currentAngles[0] > C ? 0 : currentAngles[0] += Time.deltaTime * speed;

        for (int i = 1; i < currentAngles.Length; i++)
        {
            currentAngles[i] = currentAngles[0] + (C / currentAngles.Length) * i;
            if (currentAngles[i] > C)
            {
                currentAngles[i] -= C;
            }
        }

        return currentAngles;
    }
}
