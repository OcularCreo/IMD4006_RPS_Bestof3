using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Level
{
    Dino,
    Math,
    Tutorial,
    None
}

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private Manager gameManager;    
    private Transform thisTransform;

    [SerializeField] Level thisLvl;
    private bool isSelected;
    private int numInside;
    
    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        thisTransform = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //only players have this on them. If the object has this that means they've entered the zone
        if (collision.GetComponent<RPS_Switching>() != null) {
            numInside++;
        }
 
        //when there are two players in the zone allow players to choose this option and communicate to game manager       
        if(numInside >= 2)
        {
            gameManager.selectedLevel = thisLvl;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //only players have this on them. If the object has this that means they've entered the zone
        if (collision.GetComponent<RPS_Switching>() != null)
        {
            numInside--;
        }

        //when a there aren't 2 players in the zone disallow them from continuing until they choose one
        if (numInside < 2)
        {
            gameManager.selectedLevel = Level.None;

        }
    }
}
