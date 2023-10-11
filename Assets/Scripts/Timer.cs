using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //variable
    private float newTime;
    private float x = 0.001f;
    private float speed = 0.0000002f; // change this, to change the time speed; the lower the slower the time count down
    private bool timeOut = false;
    private bool iscountDown = true;
    private float waitTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Transform>().localScale = new Vector2(15.8f, 1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    
    // Update is called once per frame
    void Update()
    {
        
        
        
        if(!timeOut){
            countDown();
        }

        if(timeOut){
            speed = 0.0000002f;
            x = 0.001f;
            iscountDown = false;
            Transform time = GameObject.FindGameObjectWithTag("timerTag").GetComponent<Transform>();
            time.localScale = new Vector2(15.8f, 1f);
            timeOut = false;
        }

       
        if(x <0){
            timeOut = true;
        }

        if(waitTime <= 0){
            timeOut = false;
        }

    }

    
    
    private void countDown()
    {
        //Transform time = gameObject.GetComponent<Transform>();
        Transform time = GameObject.FindGameObjectWithTag("timerTag").GetComponent<Transform>();

        //if there is time left
        if (time.localScale.x >= 0.01f)
        {
            x += 0.005f;
            newTime = time.localScale.x - (speed * (Mathf.Pow(x, 3)));
            time.localScale = new Vector2(newTime, 1f);
           
        }
        else
        {
            timeOut = true;
        }
    }    
}

