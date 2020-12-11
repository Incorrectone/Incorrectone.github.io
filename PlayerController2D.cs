using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;   //Define The layer it works on [Platform's Layer]
    private Rigidbody2D rbb;                                //RigidBody of the 2D player
    private BoxCollider2D bxcol;                            //Collider for Head (Can Be disabled when Crouching thus will decrease height)
    private CircleCollider2D crcol;                         //Collider for the rest of the body
    private Animator animator;                              //Defines Animator
    private bool direcchck = true;                          //
    private bool Isjump;                                    //
    private float direc;                                    //Some Variables...
    private float yspeed;                                   //...Which will be Required
    private float xspeed;                                   //
    private float IsCrouch;                                 //
    
    //Runs the Code at Start
    void Start() 
    {
        bxcol = transform.GetComponent<BoxCollider2D>();    //
        crcol = transform.GetComponent<CircleCollider2D>(); //Automaticaly Gets...
        rbb = transform.GetComponent<Rigidbody2D>();        //...These things
        animator = transform.GetComponent<Animator>();      //
    }
    
    //Runs Once every frame
    void Update()
    {
        direc = Input.GetAxis("Horizontal");                //Takes Input from ["A" && "D"] keys
        Isjump = Input.GetButtonDown("Jump");               //Takes input from ["Space"  && "W"] key
        IsCrouch = Input.GetAxisRaw("Crouch");              //Takes Input from ["S"] key {Will Have to define new input}

        //Jump CODE
        if (Isjump && IsGrounded())
        {
            xspeed = rbb.velocity.x;
            rbb.velocity = new Vector3(xspeed, 12f, 0);
            animator.SetBool("IsJumping", true);            //Animator Parameter "IsJumping", Denotes what it does [bool][1/3]
        }else
        {
            animator.SetBool("IsJumping", false);           //Animator Parameter "IsJumping", Denotes what it does [bool][1/3]
        }
        //Jump CODE ends
        
        if (IsCrouch != 0f && !(IsGrounded()) && rbb.velocity.y < 0.01f)
        {
            rbb.gravityScale = 1f;
            Debug.Log("1");
        }else{
            rbb.gravityScale = 3f;
            Debug.Log("3");        
        }

        //Crouch CODE
        if (IsCrouch != 0f && ShouldUncrouch())
        {
            bxcol.enabled = false;                          //Disables the top head collider[Height-Reduced]
            animator.SetFloat("IsCrouching", 1);            //Animator Parameter "IsCrouching", Denotes what it does [Float][2/3]
        }else if (!(ShouldUncrouch()))
        {
            bxcol.enabled = true;                           //Enables the top head collider[Height-Normal]
            animator.SetFloat("IsCrouching", -1);           //Animator Parameter "IsCrouching", Denotes what it does [Float][3/3]
        }
        //Crouch CODE ends
    }
    
    //Runs every Fixed time
    void FixedUpdate() {
        animator.SetFloat("Speed", Mathf.Abs(direc));       //Animator Parameter "Speed", Denotes what it does [Float][3/3]
        
        //Move CODE
        if (direc != 0 && !(ShouldUncrouch()))              //Checks if Player is pressing ["A" && "D"] keys and is crouching or not[he is not] 
        {
            yspeed = rbb.velocity.y;
            rbb.velocity = new Vector3(6 * direc, yspeed, 0);
        }else if (direc != 0 && ShouldUncrouch())          //Checks if Player is pressing ["A" && "D"] keys and is crouching or not[he is]
        {
            yspeed = rbb.velocity.y;
            rbb.velocity = new Vector3(2 * direc, yspeed, 0); //Speed is reduced to 2 from 10
        }
        //Move CODE ends

        //Flips the player if moves in other direction
        if (direc > 0 && !(direcchck))
        {
            Flip();                                         //Calls Private Do Function
            direcchck = true;
        }else if (direc < 0 && direcchck)
        {
            Flip();                                         //Calls Private Do Function
            direcchck = false;
        }    
        //Ends    
    }
    
    //Private Check Functions
    private bool IsGrounded() {                             //Does what its name is [Uses Box-Cast]
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(crcol.bounds.center, crcol.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        return raycastHit.collider != null;
    }

    private bool ShouldUncrouch() {                        //Does what its name is [Uses Another Box Collider on Child Object]
        if (transform.Find("CrouchCheck").GetComponent<CrouchCheck>().isCrouchCheck || IsCrouch != 0){
            return true;
        }
        return false;
    }
    
    //Private Do Functions
    private void Flip()                                     //Flips the player
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}