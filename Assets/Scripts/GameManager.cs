using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
//using System;
using UnityEngine.InputSystem;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

//different states the game can be in
public enum GameState
{
    RPS,
    battle,
    gameOver, 
    menu
}

public class Manager : MonoBehaviour
{

    float battleTime;         //timer variable for battle times
    public float RPS_time;    //timer variable for swtiching characters

    public GameState state; //variable to keep track of the game's current state

    public GameObject player1;
    public GameObject player2;

    [SerializeField] public GameObject time;

    public bool stopSwitchingAnimation; // boolean to stop the switching animation when rps state ends

    //NEW TIME
    public TimeBar timebar;

    public TextMeshProUGUI stateLabelUI;
    public InputAction action;
    
    [SerializeField] private GameObject inputPlayerManager;
    [SerializeField] private GameObject menuColliders;
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;

	[SerializeField] private GameObject menuBackground;
	[SerializeField] private GameObject canvas;

	[SerializeField] private GameObject switchIcons;
	[SerializeField] private GameObject hp1;
	[SerializeField] private GameObject hp2;

    [SerializeField] private GameObject fightGraphic;
    [SerializeField] private GameObject rpsGraphic;
    private bool shownGraphic;

	[SerializeField] private GameObject timerFightGraphic;
	[SerializeField] private GameObject timerRpsGraphic;

	[SerializeField] private GameObject victoryGraphic1;
    [SerializeField] private GameObject victoryGraphic2;


    //funciton used to handle different game states
    /*public void updateGameState(GameState newState)
    {
        state = newState;   //get the new state of the game

        //handle the new game state
        switch (newState)
        {
            case GameState.RPS:
                break;
            case GameState.battle:
                break;
            case GameState.gameOver:
                break;
        }

    }*/

    // Start is called before the first frame update
    void Start()
    {

        action.Enable();

        //start the game with the rock paper scissors state
        state = GameState.menu;

        //begining values for timers
        battleTime = 25f;
        RPS_time = 10f;

        //UI timer variable
        time.GetComponent<Transform>().localScale = new Vector2(15.8f, 1f);
        time.GetComponent<SpriteRenderer>().color =  new Vector4(0.1863f,0.7452f,0.3768f,1f);
        time.GetComponent<SpriteRenderer>().enabled = true;

        //NEWBAR
        //WHEN START SET TO MAX RPS TIME
        timebar.setMaxTime(RPS_time);

        //stateLabelUI = GameObject.Find("State Label").GetComponent<TextMeshProUGUI>();

        //assigned colour to the player
        /*Vector4 p1Color = new Vector4(1f, 0.6181373f, 0.3820755f, 1f);
		Vector4 p2Color = new Vector4(0.3726415f, 0.7f, 1f, 1f);

		player1.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = p1Color;
        player1.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = p1Color;
        player1.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = p1Color;
		player1.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().color = p1Color;
		player1.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().color = p1Color;
		player1.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().color = p1Color;

		player2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = p2Color;
        player2.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = p2Color;
        player2.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = p2Color;
		player2.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().color = p2Color;
		player2.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().color = p2Color;
		player2.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().color = p2Color;*/

        //player2.GetComponent<SpriteRenderer>().color = new Vector4(1f,0f,0f,1f);

        stopSwitchingAnimation = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.menu)
        {
            //if the controller is triggered and two players are in the game, enter RPS state, trun of the menu
            if ((action.triggered && inputPlayerManager.GetComponent<MenuSpawn>().numPlayers > 1))
            {
                state = GameState.RPS;
                shownGraphic = false;
                menuColliders.SetActive(false);
                virtualCam.Follow = cameraTargetGroup.transform;

                menuBackground.SetActive(false);
                canvas.SetActive(true);
            }
        }
        
        if (state == GameState.RPS)
        {
            //show the rps graphic
            if (!shownGraphic)
            {
                StartCoroutine(RPSGraphicReveal());
            }

            hp1.SetActive(false);
            hp2.SetActive(false);
            switchIcons.SetActive(true);

            //start to count down the time
            RPS_time -= Time.deltaTime;

            time.GetComponent<Transform>().localScale = new Vector2(RPS_time, 1f);
            time.GetComponent<SpriteRenderer>().color = new Vector4(0.1863f, 0.7452f, 0.3768f, 1f);

            //NEW TIME BAR
            timebar.setTime(RPS_time);

            // change label to 
            stateLabelUI.text = "RPS Time!";

            //timerRpsGraphic.SetActive(true);

            // hide menu
            //GameObject.Find("Menu_P1_EmptyHealth").GetComponent<Renderer>().enabled = false;

            //when the time runs out
            if (RPS_time < 0 || Input.GetKeyDown("p")) // press p to change state (for dev)
            {
				hp1.SetActive(true);
				hp2.SetActive(true);
				switchIcons.SetActive(false);

				//change the state
				state = GameState.battle;
                shownGraphic = false;

                //reset the timer
                RPS_time = 10f;

                //give weapons back
                //player1.GetComponent<Combat>().WeaponEnable();
                //player2.GetComponent<Combat>().WeaponEnable();

                //UnityEngine.Debug.Log("RPS over. Battle time!");

                // change label to 
                stateLabelUI.text = "Battle Time!";

                //Timer changes
				timebar.setMaxTime(battleTime);
                timerRpsGraphic.SetActive(false);
				timerFightGraphic.SetActive(true);

                // stop switching animation
                stopSwitchingAnimation = true;

			}
        }
        //when in the battle state
        else if (state == GameState.battle)
        {

            //show the rps graphic
            if (!shownGraphic)
            {
                StartCoroutine(fightGraphicReveal());
            }

            battleTime -= Time.deltaTime;
            time.GetComponent<Transform>().localScale = new Vector2(battleTime, 1f);
            time.GetComponent<SpriteRenderer>().color = new Vector4(0.8301f, 0.2388f, 0.2388f, 1f);

            //NEW TIME BAR
            timebar.setTime(battleTime);

            if (battleTime < 0 || Input.GetKeyDown("p")) // press p to change state (for dev)
            {
                //change to the 
                state = GameState.RPS;
                shownGraphic = false;

                //reset timer
                battleTime = 25f;

                //remove weapons
                //player1.GetComponent<Combat>().WeaponDisable();
                //player2.GetComponent<Combat>().WeaponDisable();

                //UnityEngine.Debug.Log("battle over. Now for some RPS!");

                timebar.setTime(battleTime);
                
                //Timer Changes
				timebar.setMaxTime(RPS_time);
				timerRpsGraphic.SetActive(true);
				timerFightGraphic.SetActive(false);

			}


            // if a player has lost all their lives, then game over
            if (player1.GetComponent<Combat>().lives <= 0 || player2.GetComponent<Combat>().lives <= 0)
            {
                state = GameState.gameOver; // switch to game over state
                //Debug.Log("game over");
            }
            //Debug.Log("player 1 lives: " + player1.GetComponent<Combat>().lives);
            //Debug.Log("player 2 lives: " + player2.GetComponent<Combat>().lives);

        }
        //going to assume the only other possible state is game over
        else if (state == GameState.gameOver)
        {
            // display game over screen
            // change label to 
            stateLabelUI.text = "GAME OVER";
        }

    }

    private IEnumerator fightGraphicReveal()
    {
        fightGraphic.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        fightGraphic.gameObject.SetActive(false);
        shownGraphic = true;

    }
    private IEnumerator RPSGraphicReveal()
    {
        rpsGraphic.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        rpsGraphic.gameObject.SetActive(false);
        shownGraphic = true;

    }


    public void EndGame (Player playerNum)
    {
        StartCoroutine(EndgameDelay(playerNum));
    }

    private  IEnumerator EndgameDelay(Player playerNum)
    {
        //inputPlayerManager.GetComponent<PlayerInputManager>().joiningEnabled = false;
        if (playerNum == Player.P1)
        {
            victoryGraphic1.SetActive(true);
        }
        else
        {
            victoryGraphic2.SetActive(true);
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("BetaScene");
    }
}

