using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller_Movement : MonoBehaviour
{

    //***** GROUND CHECK VARIABLES *****
    private Rigidbody2D rb;                         //rigid body of object script is attached to
    [SerializeField] public Transform groundCheck;  //getting transform of empty object used for player ground checking
    [SerializeField] public LayerMask groundLayer;  //referencing unity layer mask name ground

    //***** GENERAL MOVEMENT VARIABLES *****
    private float horizontal;           //variable used for horizontal (left and right) movement
    private float speed = 10f;           //variable used to determine speed of player
    private float jumpingPower = 12f;    //variable used to determine how high player jumps
    private float slamPower = 16f;      //variable used to determine how strong slams are
    private bool isFacingRight = true;  //variable used for determining player orentations

    //movement testing
    [Header("Movement Variables")]
    [SerializeField] private float acceleration = 7f; 
    [SerializeField] private float decceleration = 7f; 
    [SerializeField] private float velPower = 0.9f; 
    [SerializeField] private float frictionAmount = 2f; 

    //***** JUMP VARIABLES *****

    //Number of extra jump variables (double, triple, etc)
    private int extraJumps;             //variable keeps track of how many extra jumps a player can do (double, triple, etc.)
    public int extraJumpValues;         //variable used to set the number of character's extra jumps. Connected to extraJumps
    
    //Jump height variables (contributes to how high a player jumps when holding the jump button)
    private float jumpTimeCounter;      //used to keep track of how long a player is "jumping" or holding down the jump button
    public float jumpTime;              //variable used to dictate how long a player can hold down the jump button to go higher
    private bool jumping;               //variable used to determine if the player is "jumping" or holding down the jump button

    private float coyoteTime = 0.2f;    //determines how long coyotetime is
    private float coyoteTimeCounter;    //keep track of current coyoteTime (subtracting time.deltatime)

    // Start is called before the first frame update
    void Start()
    {
        //getting values from player object
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        extraJumps = extraJumpValues;
    }

    // Update is called once per frame

    void Update()
    {
        
        //moves player by setting it's rigidbody a velcoity based off of player inputs
        //rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        //when the player touches the ground again reset the extra jump height
        if (isGrounded())
        {
            extraJumps = extraJumpValues;
            coyoteTimeCounter = coyoteTime;
        } else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //this block of code allows player to jump higher if they hold down the south gamepad button (A on Xbox)
        //if the player is jumping
        if (jumping)
        {
            //when they are within the limits of the jump time let them go higher
            if(jumpTimeCounter > 0)
            {

				//rb.velocity = new Vector2(rb.velocity.x, jumpingPower); //old code
				rb.velocity = new Vector2(rb.velocity.x, 0); //reset up value before applying jump
				rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
                
                jumpTimeCounter -= Time.deltaTime;
            } 
            //if the timer runs out then cancel the jump to stop them from going higher and have them begin to fall
            else
            {
                jumping = false;
            }
            
        }

    }

    //Used for phsyics calcuations since the function has the same frequencey as the physics system
    void FixedUpdate()
    {
        //**** using forces for player movements ****

        float targetSpeed = horizontal * speed;                                                         //Get the desired desired velocity
        float speedDif = targetSpeed - rb.velocity.x;                                                   //Get the difference between the current velocity and our desired velocity
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;              //If the absolute value of the tharget speed is greater than 0.1 set accel rate to accel
                                                                                                        //otherwise set it to decel

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);   //calculate the movement

        rb.AddForce(movement * Vector2.right);                                                          //apply the movement

        //when the player is grounded and they've stoped moving the left stick
        if(isGrounded() && Mathf.Abs(horizontal) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));  //set the friction amount to the smaller value (between the velocity and set friction amount)
            amount *= Mathf.Sign(rb.velocity.x);                                            //applies movement direction

            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);                      //applying friction force in opposite direction as movement
        }
    }

    //function returns true if the gameObject attached to this script is grounded
    //using emptybody transform for groundcheck position
    //using Unity Layer Mask named ground for platforms
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }

    //function called when player moves left gamepad / controller stick
    public void onMove(InputAction.CallbackContext context)
    {

        //when the player is no longer moving on the controller left stick have the horizontal be 0
        if (context.canceled)
        {
            horizontal = 0f;
        } 
        //when the player is moving on the controller left stick, read it's vector value
        else
        {
            horizontal = context.ReadValue<Vector2>().x;
        }

    }

    //function called when the south controller button is pressed
    public void onJump(InputAction.CallbackContext context)
    {

        //context.cancelled works for button up
        //If this happens then we cancell the jump input
        if (context.canceled)
        {
            jumping = false;
            coyoteTimeCounter = 0f;

        }

        //make sure that the player is not jumping still
        if (!jumping)
        {
            //context.performed returns true when the input has been pressed
            //this case it is the south gamepad button (xbox = A button)
            if (context.action.triggered && extraJumps > 0)
            {
                //rb.velocity = Vector2.up * jumpingPower; //old code
                jumpTimeCounter = jumpTime;
                jumping = true;
                extraJumps--;

            }
            //When the player doesn't have any extra jumps, still let them jump once (remember that it is called extra jumps and not number of jumps)
            else if (context.action.triggered && extraJumps == 0 && coyoteTimeCounter > 0f /*isGrounded()*/)
            {
                jumpTimeCounter = jumpTime;
                jumping = true;
            } 
        }
    }

    //funciton used to read in when player hits the slam button
    public void onSlam(InputAction.CallbackContext context)
    {
        //when the player presses the right trigger have them move down at a speed determined by slam power
        if (context.action.triggered)
        {
            rb.velocity = Vector2.down * slamPower;
        }
        
    }

    //when the player object collides with something
    public void OnCollisionEnter2D(Collision2D collision)
    {

        //when a player lands on another player at a velocity greater than 11f
        //remove health from the player that was landed on
        if(collision.gameObject.tag == "Player" && collision.relativeVelocity.y > 11f)
        {
            //collision.gameObject.GetComponent<Controller_Movement>().health -= 10;
        }
        
    }
}
