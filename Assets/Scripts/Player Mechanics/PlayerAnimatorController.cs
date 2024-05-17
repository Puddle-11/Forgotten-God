using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private float SpeedThreshhold;
    [SerializeField] private Rigidbody2D RB;
    [SerializeField] private float Velocity;
    [SerializeField] private float UpdateSpeed;
    [SerializeField] private Vector3 OldPos;

    [SerializeField] private Animator Anim;
    [SerializeField] private Animator Anim2;
    [SerializeField] private Animator SAnim;
    [SerializeField] private bool Flip;
    [SerializeField] private SpriteRenderer SP;
    [SerializeField] private SpriteRenderer SP2;
    [SerializeField] private SpriteRenderer SSP;
     public bool Grounded;
    [SerializeField] private float FallPos;
    [SerializeField] private float GroundedSpeedThreshhold;
    [HideInInspector] public bool FlipState;
    [SerializeField] private bool LookingUp;
    [SerializeField] private bool LookingDown;

    public void Update()
    {
    
    
        
       
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                LookingUp = false;

            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                LookingDown = false;

            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {

                }
                else
                {
                    LookingUp = true;
                    LookingDown = false;
                }

            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {

                }
                else
                {
                    LookingUp = false;
                    LookingDown = true;

                }
            }
            else
            {
                LookingDown = false;
                LookingUp = false;
            }
        ChangeBool("LookingUp", LookingUp, Anim);
        ChangeBool("LookingUp", LookingUp, Anim2);
        ChangeBool("LookingUp", LookingUp, SAnim);

        ChangeBool("LookingDown", LookingDown, Anim);
        ChangeBool("LookingDown", LookingDown, Anim2);
        ChangeBool("LookingDown", LookingDown, SAnim);

            if (FallPos - transform.position.y == 0)
            {
            ChangeBool("Ground", true, Anim);
            ChangeBool("Ground", true, Anim2);
            ChangeBool("Ground", true, SAnim);


            ChangeBool("Falling", false, Anim);
            ChangeBool("Falling", false, Anim2);
            ChangeBool("Falling", false, SAnim);


  
                if (Grounded == false)
                {
                    Grounded = true;
                }

            }
            else if (FallPos - transform.position.y != 0)
            {

            ChangeBool("Ground", false, Anim);
            ChangeBool("Ground", false, Anim2);
            ChangeBool("Ground", false, SAnim);
            if (Grounded == true)
                {
                    Grounded = false;
                }

            }

            if (transform.position.y - FallPos < 0 && Anim.GetBool("Ground") == false)
            {
            ChangeBool("Falling", true, Anim);
            ChangeBool("Falling", true, Anim2);
            ChangeBool("Falling", true, SAnim);
        }
            else if (transform.position.y - FallPos > 0)
            {
            ChangeBool("Falling", false, Anim);
            ChangeBool("Falling", false, Anim2);
            ChangeBool("Falling", false, SAnim);
        }

            FallPos = transform.position.y;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                FlipState = Flip;
                SP.flipX = Flip;
                SP2.flipX = Flip;
                SSP.flipX = Flip;

            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                FlipState = !Flip;

                SP2.flipX = !Flip;
                SSP.flipX = !Flip;
                SP.flipX = !Flip;

            }

            OldPos = transform.position;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
            ChangeBool("Walk", true, Anim);
            ChangeBool("Walk", true, Anim2);
            ChangeBool("Walk", true, SAnim);
        }
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
            ChangeBool("Walk", false, Anim);
            ChangeBool("Walk", false, Anim2);
            ChangeBool("Walk", false, SAnim);

        }
            if (Input.GetKeyDown(KeyCode.Space))
            {

            
                Anim.SetTrigger("Jump");
                Anim2.SetTrigger("Jump");
                SAnim.SetTrigger("Jump");


        }


    }
 
    public void ChangeTrigger(string _triggerName, Animator _anim)
    {
        if (_anim != null)
        {
            _anim.SetTrigger(_triggerName);
        }
    }
    public void ChangeBool(string _boolname, bool _state, Animator _anim)
    {
        if (_anim != null)
        {
            _anim.SetBool(_boolname, _state);
        }
    }

}
