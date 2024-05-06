using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int Length;
    public LineRenderer Linerend;
    public Vector3[] SegmentPoses;
    private Vector3[] Segmentv;
    public Transform TargetDir;
    public float targetDist;
    public float smoothSpeed;
    public float trailSpeed;
    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;
    public Transform[] BodyParts;
    void Start()
    {

        wiggleSpeed = wiggleSpeed / wiggleMagnitude;
        wiggleSpeed = Mathf.Abs(wiggleSpeed);


        Linerend.positionCount = Length;
        SegmentPoses = new Vector3[Length];
        Segmentv = new Vector3[Length];

        SegmentPoses[0] = TargetDir.position;
        for (int i = 1; i < SegmentPoses.Length; i++)
        {
            SegmentPoses[i] = SegmentPoses[0];
        }
        Linerend.SetPositions(SegmentPoses);
        BodyParts = CopyArr(SegmentPoses.Length, BodyParts);





    }


    private void FixedUpdate()
    {
        //wiggle function
        if (wiggleMagnitude != 0 && wiggleSpeed != 0)
        {
            wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
            //-------------------------------------
        }

        //physics
        SegmentPoses[0] = TargetDir.position;
        for (int i = 1; i < SegmentPoses.Length; i++)
        {
            SegmentPoses[i] = Vector3.SmoothDamp(SegmentPoses[i], SegmentPoses[i - 1] + TargetDir.right * targetDist, ref Segmentv[i], smoothSpeed + i / trailSpeed);


            if (BodyParts[i - 1] != null)
            {

                BodyParts[i - 1].transform.position = new Vector3(SegmentPoses[i].x, SegmentPoses[i].y, BodyParts[i - 1].transform.position.z);

            }



        }
        Linerend.SetPositions(SegmentPoses);
        //------------------------------------

    }



    public Transform[] CopyArr(int length, Transform[] Origin)
    {
        if (length < Origin.Length)
        {
            length = Origin.Length;

        }
        Transform[] Res = new Transform[length];
        for (int i = 0; i < Origin.Length; i++)
        {
            Res[i] = Origin[i];


        }
        return Res;


    }
}
