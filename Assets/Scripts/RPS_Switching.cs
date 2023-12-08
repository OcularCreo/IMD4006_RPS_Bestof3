using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//easily determine which character the player is playing as
public enum Character
{
    rock,
    paper,
    scissors
}

//used to easily read which player is which
public enum Player
{
    P1, 
    P2
}

public class RPS_Switching : MonoBehaviour
{

    [SerializeField] public Manager gameManager;//get the script from the game manager
    [SerializeField] public Player player;      //determine which player the character belongs to
    private Combat combat;                      //getting the combat script
    [SerializeField] private Controller_Movement controllerMovement;

    string[] RPS_cntrl = new string[3];         //creating empty character array
    
    public Character character;                 //variable to keep track of the current character the player is
    Character selectionCharacter;               //veriable used when selected a new character
    bool applyedChange;
    public GameObject rock, paper, scissors;    //variable to take in the different character objects/types

    private bool playerOnPlatform;              //boolean to check if the player is on a platform
    private bool playerStationary;
    private string switchButton;

    public Animator animator;
    private bool stoppedAnim;

    //Switch Particles
	[SerializeField] private ParticleSystem switchParticles;
    private bool switchParticlesReady = true;
	private int particleCounter = 3;

	//[SerializeField] private GameObject controlLayout;

	// Start is called before the first frame update
	void Start()
    {

        switchButton = "none";
        
        //defaulting players to rock character
        character = Character.rock;
        toggleCharacter(true, Character.rock);

        //setting up variables
        applyedChange = false;

        //getting the combat script from the game object itself
        combat = gameObject.GetComponent<Combat>();


        //set the controls dependong on if they are player 1 or 2
        if (player == Player.P1)
        {
            RPS_cntrl[0] = "z";
            RPS_cntrl[1] = "x";
            RPS_cntrl[2] = "c";

            

        } else
        {
            RPS_cntrl[0] = "b";
            RPS_cntrl[1] = "n";
            RPS_cntrl[2] = "m";

            
        }

        stoppedAnim = true;
    }

    // Update is called once per frame
    void Update()
    {
        // check if player is on platform
        playerOnPlatform = gameObject.GetComponent<Controller_Movement>().isGrounded();
        // check if player is moving controller stick
        if(Mathf.Abs(gameObject.GetComponent<Controller_Movement>().horizontal) > 0.1)
        {
            playerStationary = false;
        }
        else 
        {
            playerStationary = true;
        }
        

        //when in RPS mode and player is on platform and player not moving L/R, allow the player to change their character
        if (gameManager.state == GameState.RPS && playerOnPlatform)
        {
            //controlLayout.SetActive(true);

            //change the character type
            if (switchButton == "buttonWest")
            {
                selectionCharacter = Character.rock;

				// start animation
				changeCharacterAnimation();
            }
            else if (switchButton == "buttonNorth")
            {
                selectionCharacter = Character.paper;

				// start animation
				changeCharacterAnimation();
            }
            else if (switchButton == "buttonEast")
            {
                selectionCharacter = Character.scissors;

                // start animation
                changeCharacterAnimation();
            }

        } 
        //need to reset toggle bool once out of the switching characters state
        else if (gameManager.state != GameState.RPS && !stoppedAnim)
        {
            applyedChange = false;

            stopAnimation();
            stoppedAnim = true;

			//controlLayout.SetActive(false);
		}

    }

    //function called via player controller input (type of component attached to the player game object)
    public void onSwitching(InputAction.CallbackContext context)
    {
        //on button up (player no longer holding button)
        if (context.canceled)
        {
            switchButton = "none";
            // if the game state is RPS and the button is released, stop the switching animation
            if(gameManager.state == GameState.RPS)
            {
                stopAnimation();
            }

            //reset particle counter
            particleCounter = 3;
        }

        //on button down (player has pressed the button)
        if (context.action.triggered)
        {
            switchButton = context.control.name;

		}
    }

    //function called after players have chosen their character
    public void changeCharacter()
    {

        //when the character is the same as before do nothing
        if(character == selectionCharacter)
        {
            //UnityEngine.Debug.Log("nothing changed");
            return;
        } 
        //when the character has changed...
        else
        {
            
            //turn off the previously active character
            toggleCharacter(false, character);

            //turn on the new character
            toggleCharacter(true, selectionCharacter);

            //set the character of the player to the new character
            character = selectionCharacter;

            //Take double damage for 3 Seconds
            gameObject.GetComponent<Combat>().switchDamage();
        }

    }

    //function used to either turn a character on or off
    private void toggleCharacter(bool active, Character activeChar)
    {
        //change the character stats only looking at the character 
        switch (activeChar)
        {
            case Character.rock:
                controllerMovement.acceleration = 5;
                controllerMovement.decceleration = 13;
                controllerMovement.jumpingPower = 15;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
                gameObject.GetComponent<Rigidbody2D>().mass = 2;
                break;
            case Character.paper:
                controllerMovement.acceleration = 3;
                controllerMovement.decceleration = 3;
                controllerMovement.jumpingPower = 8;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
                gameObject.GetComponent<Rigidbody2D>().mass = 1;
                break;
            case Character.scissors:
                controllerMovement.acceleration = 13;
                controllerMovement.decceleration = 7;
                controllerMovement.jumpingPower = 12;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
                gameObject.GetComponent<Rigidbody2D>().mass = 1.5f;
                break;
        }

        //find the character type and activate or deactiveate it depending on function parameter inputs
        if (player == Player.P1)
        {
            switch (activeChar)
            {
                case Character.rock:
                    GetComponent<PlayerGFX>().rockIdle.SetActive(active);
                    break;
                case Character.paper:
					GetComponent<PlayerGFX>().paperIdle.SetActive(active);
					break;
                case Character.scissors:
                    GetComponent<PlayerGFX>().scissorsIdle.SetActive(active);
                    break;
            }
        }
        else {
			switch (activeChar)
			{
				case Character.rock:
					GetComponent<PlayerGFX>().rockIdle2.SetActive(active);
                    break;
				case Character.paper:
					GetComponent<PlayerGFX>().paperIdle2.SetActive(active);
					break;
				case Character.scissors:
					GetComponent<PlayerGFX>().scissorsIdle2.SetActive(active);
                    break;
			}
		}
        
    }

    public void swapSprites(bool idle, Character charType)
    {
        //change sprites depending on who the player is
        if (player == Player.P1)
        {
            switch (charType)
            {
                case Character.rock:
                    GetComponent<PlayerGFX>().rockIdle.SetActive(idle);
                    GetComponent<PlayerGFX>().rockChange.SetActive(!idle);
                    break;
                case Character.paper:
                    GetComponent<PlayerGFX>().paperIdle.SetActive(idle);
                    GetComponent<PlayerGFX>().paperChange.SetActive(!idle);
                    break;
                case Character.scissors:
                    GetComponent<PlayerGFX>().scissorsIdle.SetActive(idle);
                    GetComponent<PlayerGFX>().scissorsChange.SetActive(!idle);
                    break;
            }
        }
        else
        {
            switch (charType)
            {
                case Character.rock:
                    GetComponent<PlayerGFX>().rockIdle2.SetActive(idle);
                    GetComponent<PlayerGFX>().rockChange2.SetActive(!idle);
                    break;
                case Character.paper:
                    GetComponent<PlayerGFX>().paperIdle2.SetActive(idle);
                    GetComponent<PlayerGFX>().paperChange2.SetActive(!idle);
                    break;
                case Character.scissors:
                    GetComponent<PlayerGFX>().scissorsIdle2.SetActive(idle);
                    GetComponent<PlayerGFX>().scissorsChange2.SetActive(!idle);
                    break;
            }
        }
    }


    //new animation function using unity animation
    private void changeCharacterAnimation()
    {
        stoppedAnim = false;
        // change character if game is still in RPS state and if the player is still holding down the key
        if (gameManager.state == GameState.RPS && switchButton != "none" && playerStationary)
        {
            //Play Particles
            if (switchParticlesReady && particleCounter > 0) 
            {
                particleCounter--;
                StartCoroutine(SwitchParticleTime());
			}

			//turn off the idle sprite and then turn on the changing sprite
			swapSprites(false, character);
            // play switching animation
            animator.SetBool("Switching", true);
            //animator.StartPlayback();
        }
        else
        {
            //if switching is stopped go back to the idle sprite and stop switching animation
            stopAnimation();
		}
    }

	private IEnumerator SwitchParticleTime()
	{
		switchParticlesReady = false;
		yield return new WaitForSeconds(0.41f);
		Instantiate(switchParticles, gameObject.transform.position, gameObject.transform.rotation);
		switchParticlesReady = true;
	}

	public void stopAnimation()
    {
        animator.SetBool("Switching", false); //stop playing the animation
        swapSprites(true, character); // go back to the idle sprite
	}
    


}