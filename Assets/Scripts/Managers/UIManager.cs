using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    private bool currentlyRunning = false;
    private bool fadeOpen = true;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private GameObject panelOne;
    [SerializeField] private GameObject panelTwo;
    private Vector2 panelOneEndAnchor = new Vector2(-2880, 0);
    private Vector2 panelTwoEndAnchor = new Vector2(2880, 0);
    private Vector2 panelOneStartAnchor;
    private Vector2 panelTwoStartAnchor;
    private void Start()
    {
        panelOneStartAnchor = panelOne.transform.localPosition;
        panelTwoStartAnchor = panelTwo.transform.localPosition;
        
    }
    void Update()
    {

    


        if (fadeOpen)
        {
            panelOne.transform.localPosition = Vector3.MoveTowards(panelOne.transform.localPosition, panelOneEndAnchor, fadeSpeed * Time.deltaTime * 100);
            panelTwo.transform.localPosition = Vector3.MoveTowards(panelTwo.transform.localPosition, panelTwoEndAnchor, fadeSpeed * Time.deltaTime * 100);

        }
        else
        {
           panelOne.transform.localPosition = Vector3.MoveTowards(panelOne.transform.localPosition, panelOneStartAnchor, fadeSpeed * Time.deltaTime * 100);
           panelTwo.transform.localPosition = Vector3.MoveTowards(panelTwo.transform.localPosition, panelTwoStartAnchor, fadeSpeed * Time.deltaTime * 100);
        }

    }
    public void Close()
    {
        fadeOpen = false;
    }
    public void Open()
    {
        fadeOpen = true;
    }
    public void BeginFade(float _delay, Action[] _funcs)
    {
        if (!currentlyRunning)
        {
            StartCoroutine(FadeMain(_delay, _funcs));
        }
    }

    private IEnumerator FadeMain(float _delay, Action[] _funcs)
    {
        currentlyRunning = true;
        WaitForSeconds wait = new WaitForSeconds(_delay/2);
        //close
        fadeOpen = false;
        yield return wait;

        foreach (Action func in _funcs)
        {
            if (func != null)
            {
                func();
            }
        }
        yield return wait;
        //open
        fadeOpen = true;
        currentlyRunning = false;
    }
    public void BeginFade(float _delay, Action _func)
    {
        BeginFade(_delay, new Action[] { _func });
    }
    public void BeginFade(float _delay)
    {
        BeginFade(_delay, new Action[] { null });
    }
}
