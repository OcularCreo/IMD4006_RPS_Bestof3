using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] GameObject alert_Health;
    [SerializeField] GameObject alert_Lives;
    [SerializeField] GameObject health_bar;

    int health;
    int lives;
    float alertBlinkWaitTime; 
    float alertBlinkstart;
    float alertEvery; 

    float healthBarNum;
    float healthBarLocalPosition;

    bool isHit;

    // Start is called before the first frame update
    void Start()
    {
       
       alertBlinkWaitTime = 0.2f; //seconds
       alertBlinkstart = 0f; //seconds
       alertEvery = 0.4f; // seconds

       healthBarNum = health_bar.GetComponent<Transform>().localScale.x;
       healthBarLocalPosition = health_bar.GetComponent<Transform>().position.x;

       
    }

    // Update is called once per frame
    void Update()
    {
      
        health = gameObject.GetComponent<Combat>().health;
        lives = gameObject.GetComponent<Combat>().lives;
        Debug.Log(gameObject.name + " health is: " + health);

        //health_alert();

        //lives_aleart();

        isHit = gameObject.GetComponent<Combat>().alreadyHit;


        // if(isHit){
            
        //     healthBarNum -= (1 - (health/100f));
        //     healthBarLocalPosition =  (health/100f);
        //     health_bar.GetComponent<Transform>().localScale = new Vector2(health_bar.GetComponent<Transform>().localScale.x-0.3,health_bar.GetComponent<Transform>().localScale.y);
        //     health_bar.GetComponent<Transform>().position = new Vector2(GetComponent<Transform>().position.x,health_bar.GetComponent<Transform>().position.y);
        //     Debug.Log(gameObject.name + "is hit" + healthBarNum);
        // }
       

    }


    void health_alert(){
       if(health < 20){
            //InvokeRepeating("blinkAlert", alertBlinkstart, alertEvery);
            alert_Health.SetActive(true);
        }
        else {
            //InvokeRepeating("blinkAlert", alertBlinkstart, alertEvery);
            alert_Health.SetActive(false);
        } 
    }

    void lives_aleart(){

        if(lives <2){
            alert_Lives.SetActive(true);
        }else{
            alert_Lives.SetActive(false);
        }

    }
}
