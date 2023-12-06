using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller_Movement : MonoBehaviour
{

    //***** GROUND CHECK VARIABLES *****
    [Header("Ground Check Variables")]
    [SerializeField] public Transform groundCheck;  //getting transform of empty object used for player ground checking
    [SerializeField] public LayerMask groundLayer;  //referencing unity layer mask name ground
    private Rigidbody2D rb;                         //rigid body of object script is attached to

    //***** GENERAL MOVEMENT VARIABLES *****
    [Header("Movement Variables")]
    [SerializeField] private float horizontal;           //variable used for horizontal (left and right) movement
    [SerializeField] private float speed = 10f;          //variable used to determine speed of player
    public float acceleration = 7f;    //sets acceleration of the player
    public float decceleration = 7f;   //sets the deceleration of the player
    [SerializeField] private float velPower = 0.9f;      //sets the velocity power of the plyaer
    [SerializeField] private float frictionAmount = 2f;  //sets the amount of friciton the player has to the floor when deccelerating
    
    private float slamPower = 50f;      //variable used to determine how strong slams are
    public bool slamming;              //tells us if the player his holding the slamming button
    public bool isFacingRight = true;  //variable used for determining player orentations
	
    //Movement Particles
	[Header("Particles")]
	[SerializeField] private ParticleSystem walkDust;      //what dust particle to use
	[SerializeField] private GameObject walkDustPos;       //position of particles spawn
	[SerializeField] private ParticleSystem jumpParticle;  //jump particle

	//***** JUMP VARIABLES *****
	[Header("Jump Variables")]

    //Number of extra jump variables (double, triple, etc)
    [SerializeField] public int extraJumpValues;    //variable used to set the number of character's extra jumps. Connected to extraJumps
    private int extraJumps;                         //variable keeps track of how many extra jumps a player can do (double, triple, etc.)

    //Jump height variables (contributes to how high a player jumps when holding the jump button)
    [SerializeField] public float jumpingPower;            //variable used to determine how high player jumps
    [SerializeField] public float jumpTime;                 //variable used to dictate how long a player can hold down the jump button to go higher
    private float jumpTimeCounter;                          //used to keep track of how long a player is "jumping" or holding down the jump button
    private bool jumping;                                   //variable used to determine if the player is "jumping" or holding down the jump button

    //coyote time jump variables
    private float coyoteTime = 0.2f;    //determines how long coyotetime is
    private float coyoteTimeCounter;    //keep track of current coyoteTime (subtracting time.deltatime)

	//Knockback
	public bool isBeingKnockedBack = false;

	// Start is called before the first frame update
	void Start()
    {
        //getting values from player object
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        extraJumps = extraJumpValues;

        //setting up default variable values
        slamming = false;
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

        //*** TO DO: PROBABLY MOVE THIS TO THE FIXED UPDATE FUNCTON ***
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

        //Player 1 Movement
        //if (GetComponent<RPS_Switching>().player == Player.P1)
        //{
        //Player1Keyboard();
		//}

		//Player 2 Movement
		//if (GetComponent<RPS_Switching>().player == Player.P2)
		//{
		//Player2Keyboard();
		//}

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

        //when the player is holding the slam button
        if (slamming)
        {
            //Debug.Log("slamming now");
            rb.AddForce(Vector2.down * slamPower); //keep applying the downward force times the slamming power

            //when the player finally reaches the ground
            if (isGrounded())
            {
                slamming = false;
            }
        }

        
    }

    //function returns true if the gameObject attached to this script is grounded
    //using emptybody transform for groundcheck position
    //using Unity Layer Mask named ground for platforms
    public bool isGrounded()
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
        else if(!isBeingKnockedBack)
        {
            horizontal = context.ReadValue<Vector2>().x;
			//Debug.Log(context.ReadValue<Vector2>().x);
			if (isFacingRight && context.ReadValue<Vector2>().x < -0.5f)
			{
				Flip();
				isFacingRight = false;

                //create dust when switching directions
				if (isGrounded())
				{
					CreateDust();
					//Debug.Log(context.ReadValue<Vector2>().x);
				}
			}
            else if(!isFacingRight && context.ReadValue<Vector2>().x > 0.5f)
			{
				Flip();
				isFacingRight = true;

				//create dust when switching directions
				if (isGrounded())
                {
					CreateDust();
					//Debug.Log(context.ReadValue<Vector2>().x);
				}

			}

		}

    }

	public void onMoveKeyboard(int val)
	{
        horizontal = 0f;
		//when the player is moving on the controller left stick, read it's vector value
		if(val == 1)
		{
			horizontal = val;
		}
		else if (val == -1)
		{
			horizontal = val;
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

				CreateJumpParticles();

			}
            //When the player doesn't have any extra jumps, still let them jump once (remember that it is called extra jumps and not number of jumps)
            else if (context.action.triggered && extraJumps == 0 && coyoteTimeCounter > 0f /*isGrounded()*/)
            {
                jumpTimeCounter = jumpTime;
                jumping = true;
			} 
        }
    }

	public void onJumpKeyboard(int val)
	{

		//context.cancelled works for button up
		//If this happens then we cancell the jump input
		if (val == 0)
		{
			jumping = false;
			coyoteTimeCounter = 0f;

		}

		//make sure that the player is not jumping still
		if (!jumping)
		{
			//context.performed returns true when the input has been pressed
			//this case it is the south gamepad button (xbox = A button)
			if (val == 1 && extraJumps > 0)
			{
				//rb.velocity = Vector2.up * jumpingPower; //old code
				jumpTimeCounter = jumpTime;
				jumping = true;
				extraJumps--;

			}
			//When the player doesn't have any extra jumps, still let them jump once (remember that it is called extra jumps and not number of jumps)
			else if (val == 1 && extraJumps == 0 && coyoteTimeCounter > 0f /*isGrounded()*/)
			{
				jumpTimeCounter = jumpTime;
				jumping = true;
			}
		}
	}

	//funciton used to read in when player hits the slam button
	public void onSlam(InputAction.CallbackContext context)
    {
        /*//when the player lifts the trigger
        if (context.canceled)
        {
            slamming = false;
        }*/
        
        //when the player presses the right trigger have them move down at a speed determined by slam power
        if (context.action.triggered)
        {
            //rb.AddForce(Vector2.down * slamPower); //old code
            slamming = true;
        }
        
    }

    //funciton used to read in when player hits the slam button
    public void onSlamKeyboard(int val)
	{
		//when the player lifts the trigger
		if (val == 0)
		{
			slamming = false;
		}

		//when the player presses the right trigger have them move down at a speed determined by slam power
		if (val == 1)
		{
			//rb.AddForce(Vector2.down * slamPower); //old code
			slamming = true;
		}

	}

	private void Flip()
	{
		//Debug.Log(transform.localPosition);
		//transform.localPosition = new Vector3(-transform.localPosition.x, 0, 0);
		transform.Rotate(0, 180, 0);
	}

    private void CreateDust() {
        Instantiate(walkDust, walkDustPos.transform.position, walkDustPos.transform.rotation);
    }
	private void CreateJumpParticles()
	{
		Instantiate(jumpParticle, walkDustPos.transform.position, walkDustPos.transform.rotation);
	}

	//Movement Function for Keyboard Player 1
	public void Player1Keyboard()
    {
        //Movement
		int moveVal = 0;
		if (Input.GetKey("a"))
		{
			moveVal = -1;
		}
		if (Input.GetKey("d"))
		{
			moveVal = 1;
		}
		if (Input.GetKey("d") && Input.GetKey("a"))
		{
			moveVal = 0;
		}
		onMoveKeyboard(moveVal);

        //Jump
		int jumpVal = 0;
		if (Input.GetKey("w"))
		{
			jumpVal = 1;
		}
		onJumpKeyboard(jumpVal);

        //Slam
		int abilityVal = 0;
		if (Input.GetKey("e"))
		{
			abilityVal = 1;
		}
		onSlamKeyboard(abilityVal);
	}

	//Movement Function for Keyboard Player 2
	public void Player2Keyboard()
	{
		
	}
}
