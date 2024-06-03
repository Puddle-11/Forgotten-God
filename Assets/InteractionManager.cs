using System;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;
public class InteractionManager : MonoBehaviour
{
    public List<Interaction> staticInteractions = new List<Interaction>();
    public Interaction currentInteraction;
    private Vector3 playerPastPos;
    [SerializeField] private float playerMovementMargin;
    public bool moving = false;
    private void Update()
    {
        for (int i = 0; i < staticInteractions.Count; i++)
        {
                runTimerLogic(staticInteractions[i]);   
        }
            runTimerLogic(currentInteraction);
    }
    private void FixedUpdate()
    {
        if (GlobalManager.Player.transform.position.x > playerPastPos.x + playerMovementMargin || GlobalManager.Player.transform.position.y > playerPastPos.y + playerMovementMargin || GlobalManager.Player.transform.position.y < playerPastPos.y - playerMovementMargin || GlobalManager.Player.transform.position.x < playerPastPos.x - playerMovementMargin)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        playerPastPos = GlobalManager.Player.transform.position;
    }
    private void runTimerLogic(Interaction _interaction)
    {

        if (_interaction == null) return;

        if (_interaction.A_prompt != null)
        {
            _interaction.A_prompt.GetComponent<Image>().fillAmount = _interaction.A_timer / _interaction.A_delay;
        }
        if (moving)
        {
            _interaction.A_timer = 0;
            return;
        }



        if (_interaction.A_delay == 0 && Input.GetKeyDown(_interaction.A_KeyCode))
        {
            TriggerAction(_interaction);
        }
        else if (_interaction.A_timer < _interaction.A_delay)
        {
            //if timer is still within current delay timer max
            if (Input.GetKey(_interaction.A_KeyCode) && (_interaction.A_uses > 0 || _interaction.A_uses == -1))
            {
                //increment when player is holding down interaction key
                    _interaction.A_timer += Time.deltaTime;
            }
            else
            {
                //slowly reduce to 0 when player isnt holding down interaction key and lock at 0
                _interaction.A_timer = _interaction.A_timer < 0 ? _interaction.A_timer -= Time.deltaTime : 0;
            }
        }
        else
        {
            //if timer is outside of timer max run trigger action
            _interaction.A_timer = 0;
            TriggerAction(_interaction);
        }
    }
    private void TriggerAction(Interaction _interaction)
    {
        if (_interaction.A_uses > 0)
        {
            _interaction.A_uses--;
            _interaction.A_action();
            if (_interaction.A_uses == 0) ClearAction();
        }
        else if(_interaction.A_uses == -1)
        {
            _interaction.A_action();
        }
    }
    public void SetAction(Interaction _newInteraction)
    {
        currentInteraction = _newInteraction;
        if(currentInteraction.A_prompt != null) currentInteraction.A_prompt.SetActive(true);
        currentInteraction.A_timer = 0;
    }

    public void ClearAction() 
    {
        //clears current action
        if (currentInteraction.A_prompt != null) currentInteraction.A_prompt.SetActive(false);
        currentInteraction.A_timer = 0;
        currentInteraction = null;
    }
    public bool ClearAction(Interaction _oldAction)
    {
        //only clears a specific current action
        if (currentInteraction == _oldAction)
        {
            ClearAction(); 
            return true;
        }
        return false;
    }
}
[System.Serializable]
public class Interaction {

    public KeyCode A_KeyCode;
    public Action A_action;
    public float A_delay;
    public int A_uses;
    public GameObject A_prompt;
    public float A_timer;
    public Interaction(Action _action, float _delay, int _uses, KeyCode _keyc) : this(_action, null, _delay, _uses, _keyc)
    {
    }
    public Interaction(Action _action, GameObject _prompt, float _delay, int _uses, KeyCode _keyc)
    {
        A_action = _action;
        A_delay = _delay;
        A_uses = _uses;
        A_prompt = _prompt;
        A_KeyCode = _keyc;
    }
 
}
