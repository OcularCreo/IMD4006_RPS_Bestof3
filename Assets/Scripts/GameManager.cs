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
//using static UnityEditor.PlayerSettings;

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

    //NEW TIME
    public TimeBar timebar;

    public TextMeshProUGUI stateLabelUI;
    public InputAction action;
    [SerializeField] private PlayerInputManager playerManager;

    
    [SerializeField] private GameObject inputPlayerManager;
    [SerializeField] private GameObject menuColliders;
    //[SerializeField] private GameObject cameraWalls;
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

    //Winner
	[SerializeField] private GameObject victoryGraphic1;
    [SerializeField] private GameObject victoryGraphic2;
    private GameObject playerWinner;
	[SerializeField] private ParticleSystem winnerParticles;

    public Level selectedLevel;
    [SerializeField] private GameObject bts;

    [SerializeField] private AudioSource source;
    [SerializeField] private Sound sound;

    

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

        selectedLevel = Level.None;

        source = GetComponent<AudioSource>();

	}

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.menu)
        {

            //check that there are two players
            if(playerManager.playerCount > 1 && selectedLevel != Level.None) {

                //reveal start game graphic to players


                //when the player presses the start game button
                if (action.triggered && selectedLevel != Level.Tutorial)
                {

                    //Load selected scene
                    if(selectedLevel == Level.Dino) {
                        SceneManager.LoadScene("DinoScene", LoadSceneMode.Additive);
                    } 
                    else if(selectedLevel == Level.Math)
                    {
                        SceneManager.LoadScene("HomeworkLvl", LoadSceneMode.Additive);
                    }

                    state = GameState.RPS;                              //change the game state to RPS stage
                    shownGraphic = false;                               //set to false to trigger a stage transition graphic to be revealed
                    menuColliders.SetActive(false);                     //remove menu walls/colliders to start the game
                    //cameraWalls.SetActive(false);                       //remove camera walls/colliders to start the game
                    virtualCam.Follow = cameraTargetGroup.transform;    //enable camera follow players
                    action.Disable();                                   //disable start action controls

                    //turn off menu background and show canvas UI
                    menuBackground.SetActive(false);
                    canvas.SetActive(true);

                } 
                //spawn players back at a the start screen
                if(action.triggered && selectedLevel == Level.Tutorial)
                {
                    TeleportPlayers(bts.GetComponent<Transform>().position);
                }

            }

            //PLAY SOUND
            source.PlayOneShot(sound.sound_background_menu);
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
            //stateLabelUI.text = "RPS Time!";

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

			}
            source.PlayOneShot(sound.sound_background_rps);
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
            //if (player1.GetComponent<Combat>().lives <= 0 || player2.GetComponent<Combat>().lives <= 0)
            //if (player1.GetComponent<Combat>().lives <= 0 || player2.GetComponent<Combat>().lives <= 0)
            //{
                //state = GameState.gameOver; // switch to game over state
                //Debug.Log("game over");
            //}
            //Debug.Log("player 1 lives: " + player1.GetComponent<Combat>().lives);
            //Debug.Log("player 2 lives: " + player2.GetComponent<Combat>().lives);
            source.PlayOneShot(sound.sound_background_fight);
        }
        //going to assume the only other possible state is game over
        else if (state == GameState.gameOver)
        {
			// display game over screen
			// change label to 
			//stateLabelUI.text = "GAME OVER";
			Instantiate(winnerParticles, playerWinner.gameObject.transform.position, playerWinner.gameObject.transform.rotation);
		}


    }

    private IEnumerator fightGraphicReveal()
    {
        if(state != GameState.gameOver) 
        {
			fightGraphic.gameObject.SetActive(true);

			yield return new WaitForSeconds(0.5f);

			fightGraphic.gameObject.SetActive(false);
			shownGraphic = true;
		}
    }
    private IEnumerator RPSGraphicReveal()
    {
        if (state != GameState.gameOver) 
        {
			rpsGraphic.gameObject.SetActive(true);

			yield return new WaitForSeconds(0.5f);

			rpsGraphic.gameObject.SetActive(false);
			shownGraphic = true;
		}

    }


    public void EndGame (Player playerNum)
    {
        state = GameState.gameOver;

		if (playerNum == Player.P1)
        {
            playerWinner = player2;
        }
		if (playerNum == Player.P2)
		{
			playerWinner = player1;
		}
		StartCoroutine(EndgameDelay(playerNum));
    }

    private  IEnumerator EndgameDelay(Player playerNum)
    {
        //inputPlayerManager.GetComponent<PlayerInputManager>().joiningEnabled = false;
        if (playerNum == Player.P1)
        {
            victoryGraphic2.SetActive(true);
        }
        else if (playerNum == Player.P2)
        {
            victoryGraphic1.SetActive(true);
        }
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MenuScene");
    }

	public void TeleportPlayers(Vector2 pos)
	{
		player1.transform.position = new Vector2(pos.x, pos.y);
		player2.transform.position = new Vector2(pos.x, pos.y);
	}
}

