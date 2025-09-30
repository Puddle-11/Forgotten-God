using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightController : MonoBehaviour
{
    public Volume DuskVolume;
    public Volume NightVolume;
    public ColorLookup duskLUT;


    public float CycleDurration;
    private float CycleTimer;
    public float sunPosition;
    public float DistFromMidDay;


    // Update is called once per frame
    private void Start()
    {
        DuskVolume.profile.TryGet(out duskLUT);

    }
    void Update()
    {
        CycleTimer = (CycleTimer + Time.deltaTime) % CycleDurration;
        sunPosition = Mathf.Sin(2*Mathf.PI * (CycleTimer/CycleDurration));
        
            duskLUT.contribution.value = Mathf.Abs(sunPosition);
      


        
    } 
}
