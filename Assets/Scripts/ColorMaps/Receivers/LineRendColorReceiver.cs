using UnityEngine;

public class LineRendColorReceiver : BaseColorReceiver
{
     private LineRenderer lineRend;
    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();

    }

    public override void SetColor()
    {
        lineRend.startColor = objPalette.GetColor(colorIndex);
        lineRend.endColor = objPalette.GetColor(colorIndex);
    }
}
