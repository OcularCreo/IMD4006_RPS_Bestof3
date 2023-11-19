using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

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

    string[] RPS_cntrl = new string[3];         //creating empty character array
    
    public Character character;                 //variable to keep track of the current character the player is
    Character selectionCharacter;               //veriable used when selected a new character
    bool applyedChange;
    public GameObject rock, paper, scissors;    //variable to take in the different character objects/types

    // Start is called before the first frame update
    void Start()
    {
        //defaulting players to rock character
        character = Character.rock;
        toggleCharacter(true, Character.rock);

        //setting up variables
        applyedChange = false;

        //getting the combat script from the game object itself
        combat = gameObject.GetComponent<Combat>();

        //set the controls dependong on if they are player 1 or 2
        if(player == Player.P1)
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
        if (gameManager.state == GameState.RPS)
        {

            //change the character type
            if (Input.GetKeyDown(RPS_cntrl[0]))
            {
                selectionCharacter = Character.rock;

                // start animation
                StartCoroutine(changeCharacterAnimation(0));
            }
            else if (Input.GetKeyDown(RPS_cntrl[1]))
            {
                selectionCharacter = Character.paper;

                // start animation
                StartCoroutine(changeCharacterAnimation(1));
            }
            else if (Input.GetKeyDown(RPS_cntrl[2]))
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

            //subtract 5 health
            gameObject.GetComponent<Combat>().switchDamage(); // do we still want to do this?
        }

    }

    //function used to either turn a character on or off
    private void toggleCharacter(bool active, Character activeChar)
    {
        //find the character type and activate or deactiveate it depending on function parameter inputs
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

    // plays an animation of the character doing 3 jumps + slams, then calls changeCharacter() function
    private IEnumerator changeCharacterAnimation(int rpsCntrlKey)
    {
        for (int i = 0; i < 3; i++)
        {
            // play animation only if the game is still RPS state and the player is still holding down the key
            if (gameManager.state == GameState.RPS && Input.GetKey(RPS_cntrl[rpsCntrlKey])) 
            {
                // jump
                GetComponent<Movement>().rb.velocity = new Vector2(0, 6f);
                yield return new WaitForSeconds(0.3f);

                //slam
                GetComponent<Movement>().rb.velocity = new Vector2(0, -6f);
                yield return new WaitForSeconds(0.3f);


            }
        }
        // change character if game is still in RPS state and if the player is still holding down the key
        if (gameManager.state == GameState.RPS && Input.GetKey(RPS_cntrl[rpsCntrlKey]))
        {
            changeCharacter();
        }
    }

    // maybe use forces/acceleration instead of velocity for the animation? 
    // do we want to restrict left/right movement when the animation is playing?
}