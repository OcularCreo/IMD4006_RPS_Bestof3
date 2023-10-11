using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//different states the game can be in
public enum GameState
{
    RPS,
    battle,
    gameOver
}

public class Manager : MonoBehaviour
{

    //float battleTime;  //timer variable for battle times
    //public float RPS_time;    //timer variable for swtiching characters

    public GameState state; //variable to keep track of the game's current state


    //timer variable
    
    private float newTime;
    public float RPS_x = 0.001f;
    public float battle_x = 0.001f;
    public float RPS_speed = 0.00002f; // change this, to change the time speed; the lower the slower the time count down
    public float battle_speed = 0.00002f; // change this, to change the time speed; the lower the slower the time count down

    private bool RPS_timeOut = false;
    private bool RPS_iscountDown = true;
    private float RPS_waitTime = 1f;

    private bool battle_timeOut = false;
    private bool battle_iscountDown = true;
    private float battle_waitTime = 1f;

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
        // battleTime = 15f;
        // RPS_time = 3f;

        //UI timer variable
        gameObject.GetComponent<Transform>().localScale = new Vector2(15.8f, 1f);
        GameObject.FindGameObjectWithTag("timerTag").GetComponent<SpriteRenderer>().color =  new Vector4(0.8301f, 0.2388f, 0.2388f, 1f);
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //when in the rock paper scissors state
        if(state == GameState.RPS)
        {
            //start to count down the time
            // RPS_time -= Time.deltaTime;
            
            // //when the time runs out
            // if(RPS_time < 0)
            // {
            //     //change the state
            //     state = GameState.battle;

            //     //reset the timer
            //     RPS_time = 3f;

            //     UnityEngine.Debug.Log("RPS over. Battle time!");
            // }


            //timer visual
            if(!RPS_timeOut){
            countDown(ref RPS_x, ref RPS_speed, ref RPS_timeOut);
            }

            if(RPS_timeOut){

                //change state when timeout
                state = GameState.battle;
                //end of change state

                //reset time variable 
                RPS_speed = 0.00002f;
                RPS_x = 0.001f;
                RPS_iscountDown = false;
                Transform time = GameObject.FindGameObjectWithTag("timerTag").GetComponent<Transform>();
                GameObject.FindGameObjectWithTag("timerTag").GetComponent<SpriteRenderer>().color =  new Vector4(0.1863f,0.7452f,0.3768f,1f);
                time.localScale = new Vector2(15.8f, 1f);
                RPS_timeOut = false;

                //debug
                UnityEngine.Debug.Log("RPS over. Battle time!");
            }
        
            if(RPS_x <0){
                RPS_timeOut = true;
            }

            if(RPS_waitTime <= 0){
                RPS_timeOut = false;
            }


        } 
        //when in the battle state
        else if (state == GameState.battle)
        {

            // battleTime -= Time.deltaTime;
            // if(battleTime < 0)
            // {
            //     //change to the 
            //     state = GameState.RPS;

            //     //reset timer
            //     battleTime = 15f;

            //     UnityEngine.Debug.Log("battle over. Now for some RPS!");
            // }

            //timer visual
            if(!battle_timeOut){
            countDown(ref battle_x,ref battle_speed,ref battle_timeOut);
            }

            if(battle_timeOut){

                //change to the RPS game state
                state = GameState.RPS;
                //end of change state

                //reset time variable 
                battle_speed = 0.00002f;
                battle_x = 0.001f;
                battle_iscountDown = false;
                Transform time = GameObject.FindGameObjectWithTag("timerTag").GetComponent<Transform>();
                GameObject.FindGameObjectWithTag("timerTag").GetComponent<SpriteRenderer>().color =  new Vector4(0.8301f, 0.2388f, 0.2388f, 1f);
                time.localScale = new Vector2(15.8f, 1f);
                //time.color = new Vector4(0.8301f, 0.2388f, 0.2388f, 1f);
                battle_timeOut = false;

                //debug
                UnityEngine.Debug.Log("Battle is over. RPS time!");
            }
        
            if(battle_x <0){
                battle_timeOut = true;
            }

            if(battle_waitTime <= 0){
                battle_timeOut = false;
            }





        } 
        //going to assume the only other possible state is game over
        else
        {
            //fill out game over stuff here
        }

    }


    //countdown timer function
     private void countDown(ref float time_x, ref float speed, ref bool isTimeOut)
    {
        //Transform time = gameObject.GetComponent<Transform>();
        Transform time = GameObject.FindGameObjectWithTag("timerTag").GetComponent<Transform>();

        //if there is time left
        if (time.localScale.x >= 0.01f)
        {
            time_x += 0.0009f;
            newTime = time.localScale.x - (speed * (time_x));
            time.localScale = new Vector2(newTime, 1f);
           
        }
        else
        {
            isTimeOut = true;
        }
    }   
}
