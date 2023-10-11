using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//different states the game can be in
public enum GameState
{
    RPS,
    battle,
    gameOver
}

public class Manager : MonoBehaviour
{

    float battleTime;  //timer variable for battle times
    public float RPS_time;    //timer variable for swtiching characters

    public GameState state; //variable to keep track of the game's current state

    public GameObject player1;
    public GameObject player2;

    public GameObject time;

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
        //start the game with the rock paper scissors state
        state = GameState.RPS;

        //begining values for timers
        battleTime = 15f;
        RPS_time = 3f;

        //UI timer variable
        time.GetComponent<Transform>().localScale = new Vector2(15.8f, 1f);
        time.GetComponent<SpriteRenderer>().color =  new Vector4(0.8301f, 0.2388f, 0.2388f, 1f);
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //when in the rock paper scissors state
        if(state == GameState.RPS)
        {
            //start to count down the time
            RPS_time -= Time.deltaTime;
            
            time.GetComponent<Transform>().localScale = new Vector2(RPS_time, 1f);

            //when the time runs out
            if(RPS_time < 0)
            {
                //change the state
                state = GameState.battle;

                //reset the timer
                RPS_time = 3f;

                //give weapons back
                player1.GetComponent<Combat>().WeaponEnable();
                player2.GetComponent<Combat>().WeaponEnable();

                UnityEngine.Debug.Log("RPS over. Battle time!");

                
                
            }
        } 
        //when in the battle state
        else if (state == GameState.battle)
        {

            battleTime -= Time.deltaTime;
            time.GetComponent<Transform>().localScale = new Vector2(battleTime, 1f);
            
            if(battleTime < 0)
            {
                //change to the 
                state = GameState.RPS;

                //reset timer
                battleTime = 15f;

                //remove weapons
                player1.GetComponent<Combat>().WeaponDisable();
                player2.GetComponent<Combat>().WeaponDisable();

                UnityEngine.Debug.Log("battle over. Now for some RPS!");

                
                
            }

        } 
        //going to assume the only other possible state is game over
        else
        {
            //fill out game over stuff here
        }

    }

}
