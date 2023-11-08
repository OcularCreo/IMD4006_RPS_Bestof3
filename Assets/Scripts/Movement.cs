using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor.Presets;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public Manager gameManager;   //get the script from the game manager
    public Rigidbody2D rb;                         // the player rigid body
    [SerializeField] private float moveSpeed = 3f; // speed moving left and right
    [SerializeField] private float jumpSpeed = 5f; // speed or strength of jump

    private bool jumping = false;   // check whether the player has jumped
    private int jumpCount = 0;      // counter for double jump

	public bool facingRight = true; // check what direction player is facing

    public bool isBeingKnockedBack = false;

    //Variables used for switching characters

    private float jumpXPos;           //tracks the xPosition of a character when they jump
    public int changeSlamNum;      //tracks number of times player has slammed in the same spot

	// Start is called before the first frame update
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // freeze rotation of the player, because otherwise the circle rotates when it moves
    }

    // Update is called once per frame
    void Update()
    {
        // PLAYER 1
        if(GetComponent<RPS_Switching>().player == Player.P1 && !isBeingKnockedBack)
        {
            // If player presses a, go left
            if (Input.GetKey("a"))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

				// flip player
				if (facingRight)
				{
					Flip();
					facingRight = false;
				}
			}
            // if player presses d, go right
            if (Input.GetKey("d"))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

				// flip player
				if (!facingRight)
				{
					Flip();
					facingRight = true;
				}
			}

            // if player presses w and player has not jumped twice yet, let player jump
            if (Input.GetKeyDown("w") && !(jumping))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpCount += 1;

                // if the character is paper, allow triple jump
                if(GetComponent<RPS_Switching>().character == Character.paper)
                {
                    jumping = checkJump(3);
                }
                else // otherwise only double jump
                {
                    jumping = checkJump(2);
                }
            }

            // if player presses s
            if (Input.GetKeyDown("s"))
            {
                // if player is rock, drop down
                if(GetComponent<RPS_Switching>().character == Character.rock)
                {
                    rb.velocity = new Vector2(0, (-3f) * jumpSpeed);
                }
                // if player is scissors, diagonal motion
                if (GetComponent<RPS_Switching>().character == Character.scissors)
                {
                    if (!facingRight) // diagonal towards the left if player is facing left
                    {
                        rb.velocity = new Vector2((-2f) * jumpSpeed, (-2f) * jumpSpeed);
                    }
                    else // otherwise diagonal towards the right
                    {
                        rb.velocity = new Vector2((2f) * jumpSpeed, (-2f) * jumpSpeed);
                    }
                }
            }
        }

        // PLAYER 2
        else if (GetComponent<RPS_Switching>().player == Player.P2 && !isBeingKnockedBack)
        {
            // If player presses left arrow, go left
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

				// flip player
				if (facingRight)
				{
					Flip();
					facingRight = false;
				}
			}
            // if player presses right arrow, go right
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

                // flip player
				if (!facingRight)
				{
					Flip();
					facingRight = true;
				}
			}
            // if player presses up arrow and player has not jumped twice yet, let player jump
            if (Input.GetKeyDown(KeyCode.UpArrow) && !(jumping))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpCount += 1;

                // if the character is paper, allow triple jump
                if (GetComponent<RPS_Switching>().character == Character.paper)
                {
                    jumping = checkJump(3);
                }
                else // otherwise only double jump
                {
                    jumping = checkJump(2);
                }
            }

            // if player presses down arrow
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // if player is rock, drop down
                if (GetComponent<RPS_Switching>().character == Character.rock)
                {
                    rb.velocity = new Vector2(0, (-3f) * jumpSpeed);
                }
                // if player is scissors, diagonal motion
                if (GetComponent<RPS_Switching>().character == Character.scissors)
                {
                    if (!facingRight) // diagonal towards the left if player is facing left
                    {
                        rb.velocity = new Vector2((-2f) * jumpSpeed, (-2f) * jumpSpeed);
                    }
                    else // otherwise diagonal towards the right
                    {
                        rb.velocity = new Vector2((2f) * jumpSpeed, (-2f) * jumpSpeed);
                    }
                }

            }
        }

    }

    // check if the player has jumped twice (or 3 times)
    private bool checkJump(int maxJump)
    {
        if(jumpCount >= maxJump)
        {
            return true;
        }
        return false;
    }

    // check if the player has collided with the platform
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // if collided, allow the player to jump again
        if (collision.gameObject.tag == "Platform")
        {
            // reset jump variables
            jumping = false;
            jumpCount = 0;

            //During the swithcing stage
            if(gameManager.state == GameState.RPS)
            {
                //when the player remains remains in the same x position as their initial jump
                //add to the change slam, otherwise reset it to 0.
                if (jumpXPos == gameObject.transform.position.x)
                {
                    changeSlamNum++;
                }
                else
                {
                    changeSlamNum = 0;
                }

                //when change slam is at 3
                if(changeSlamNum > 2)
                {
                    changeSlamNum = 0; //reset the game slam number
                    GetComponent<RPS_Switching>().changeCharacter();

                }
            }
            
        }
    }

    // check if the player has collided with the platform
    public void OnCollisionExit2D(Collision2D collision)
    {
        // if collided, allow the player to jump again
        if (collision.gameObject.tag == "Platform")
        {
            jumpXPos = gameObject.transform.position.x;

        }
    }

    private void Flip()
	{
		//Debug.Log(transform.localPosition);
		//transform.localPosition = new Vector3(-transform.localPosition.x, 0, 0);
		transform.Rotate(0, 180, 0);
	}

}
