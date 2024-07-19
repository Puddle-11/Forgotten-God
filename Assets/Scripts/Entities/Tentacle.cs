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
    [SerializeField] private int col1;
    [SerializeField] private int col2;
    private ObjectPaletteManager objPalette;
    void Start()
    {
        if(!TryGetComponent<ObjectPaletteManager>(out objPalette) || !transform.parent.TryGetComponent<ObjectPaletteManager>(out objPalette))
        {
         
           objPalette = GetComponentInParent<ObjectPaletteManager>();
        }
     

            if(objPalette != null)
            {
                Linerend.startColor = objPalette.GetColor(col1);
                Linerend.endColor = objPalette.GetColor(col2);
            }
        wiggleSpeed = Mathf.Abs(wiggleSpeed / wiggleMagnitude);


        Linerend.positionCount = Length;
        SegmentPoses = new Vector3[Length];
        Segmentv = new Vector3[Length];

        SegmentPoses[0] = TargetDir.position;
        for (int i = 1; i < SegmentPoses.Length; i++)
        {
            SegmentPoses[i] = SegmentPoses[0];
        }
        Linerend.SetPositions(SegmentPoses);





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
        }
        Linerend.SetPositions(SegmentPoses);
        //------------------------------------

    }



 
}
