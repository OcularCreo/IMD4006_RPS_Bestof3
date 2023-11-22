using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
    private string switchButton;

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
        //controllerMovement = gameObject.GetComponent<Controller_Movement>();

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
    }

    // Update is called once per frame
    void Update()
    {

        //checking the game manager's state
        //when in RPS mode and player has not started to do a change slam, allow the player to change their character
        //if(gameManager.state == GameState.RPS && GetComponent<Movement>().changeSlamNum < 1)
        if (gameManager.state == GameState.RPS && playerOnPlatform)
        {
            //controlLayout.SetActive(true);

            //change the character type
            if (switchButton == "buttonWest")
            {
                selectionCharacter = Character.rock;

                // start animation
                StartCoroutine(changeCharacterAnimation(0));
            }
            else if (switchButton == "buttonNorth")
            {
                selectionCharacter = Character.paper;

                // start animation
                StartCoroutine(changeCharacterAnimation(1));
            }
            else if (switchButton == "buttonEast")
            {
                selectionCharacter = Character.scissors;

                // start animation
                StartCoroutine(changeCharacterAnimation(2));
            }

        } 
        //need to reset toggle bool once out of the switching characters state
        else
        {
            applyedChange = false;
            //controlLayout.SetActive(false);
        }

    }

    public void onSwitching(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            switchButton = "none";
        }

        if (context.action.triggered)
        {
            switchButton = context.control.name;
            
            UnityEngine.Debug.Log(switchButton);
            /*switch (context.control.name)
            {
                case "buttonWest":
                    break;
                case "buttonNorth":
                    break;
                case "buttonEast":
                    break;
            }*/
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
            gameObject.GetComponent<Combat>().switchDamage(); // do we still want to do this? ye probably
        }

    }

    //function used to either turn a character on or off
    private void toggleCharacter(bool active, Character activeChar)
    {
        //find the character type and activate or deactiveate it depending on function parameter inputs
        if (player == Player.P1)
        {
            switch (activeChar)
            {
                case Character.rock:
                    GetComponent<PlayerGFX>().rockIdle.SetActive(active);
					controllerMovement.acceleration = 5;
                    controllerMovement.decceleration = 13;
					controllerMovement.jumpingPower = 15;
					gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
					gameObject.GetComponent<Rigidbody2D>().mass = 2;
					break;
                case Character.paper:
					GetComponent<PlayerGFX>().paperIdle.SetActive(active);
					controllerMovement.acceleration = 3;
					controllerMovement.decceleration = 3;
					controllerMovement.jumpingPower = 8;
					gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
					gameObject.GetComponent<Rigidbody2D>().mass = 1;
					break;
                case Character.scissors:
					GetComponent<PlayerGFX>().scissorsIdle.SetActive(active);
					controllerMovement.acceleration = 13;
					controllerMovement.decceleration = 7;
					controllerMovement.jumpingPower = 12;
					gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
					gameObject.GetComponent<Rigidbody2D>().mass = 1.5f;
					break;
            }
        }
        else {
			switch (activeChar)
			{
				case Character.rock:
					GetComponent<PlayerGFX>().rockIdle2.SetActive(active);
					controllerMovement.acceleration = 5;
					controllerMovement.decceleration = 13;
					controllerMovement.jumpingPower = 15;
					gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
					gameObject.GetComponent<Rigidbody2D>().mass = 2;
					break;
				case Character.paper:
					GetComponent<PlayerGFX>().paperIdle2.SetActive(active);
					controllerMovement.acceleration = 3;
					controllerMovement.decceleration = 3;
					controllerMovement.jumpingPower = 8;
					gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
					gameObject.GetComponent<Rigidbody2D>().mass = 1;
					break;
				case Character.scissors:
					GetComponent<PlayerGFX>().scissorsIdle2.SetActive(active);
					controllerMovement.acceleration = 13;
					controllerMovement.decceleration = 7;
					controllerMovement.jumpingPower = 12;
					gameObject.GetComponent<Rigidbody2D>().gravityScale = 2;
					gameObject.GetComponent<Rigidbody2D>().mass = 1.5f;
					break;
			}
		}
        
    }

    // plays an animation of the character doing 3 jumps + slams, then calls changeCharacter() function
    private IEnumerator changeCharacterAnimation(int rpsCntrlKey)
    {
        for (int i = 0; i < 3; i++)
        {
            // play animation only if the game is still RPS state and the player is still holding down the key
            if (gameManager.state == GameState.RPS && switchButton != "none") 
            {
				// jump
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 6f);
                yield return new WaitForSeconds(0.3f);

                //slam
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -6f);
                yield return new WaitForSeconds(0.3f);


            }
        }
        // change character if game is still in RPS state and if the player is still holding down the key
        if (gameManager.state == GameState.RPS && switchButton != "none")
        {
            changeCharacter();
        }
    }

    // check if the player has collided with the platform
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // if collided, set variable to true
        if (collision.gameObject.tag == "Platform")
        {
            playerOnPlatform = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        // if end of collision, set variable to false
        if (collision.gameObject.tag == "Platform")
        {
            playerOnPlatform = false;
        }

    }

    // notes:
    // maybe use forces instead of velocity for the animation? 
    // restrict left/right movement when the animation is playing
    // when changing characters, will need to change movement stuff specific to each character

}