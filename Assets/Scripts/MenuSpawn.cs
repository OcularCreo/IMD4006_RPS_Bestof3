using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class MenuSpawn : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private GameObject spawnPoint;                     //transform that sets the spawn location
    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;  //camera target group object

	[SerializeField] private Manager gameManagerObject;
	[SerializeField] private GameObject respawnPoints;
	[SerializeField] private HealthBar hpP1;
	[SerializeField] private HealthBar hpP2;


	public int numPlayers = 0;                                          //keep track of the number of players. Used to start game
    
    //when a player joins this funciton is called
    void OnPlayerJoined(PlayerInput playerInput)
    {
        
        //spawn the player at their given location
        playerInput.gameObject.transform.position = spawnPoint.transform.position;

        //attach the player as a target
        cameraTargetGroup.m_Targets[numPlayers].target = playerInput.gameObject.transform;

        if (numPlayers > 0)
        {
			playerInput.GetComponent<RPS_Switching>().player = Player.P2;
			playerInput.GetComponent<RPS_Switching>().gameManager = gameManagerObject;
			playerInput.GetComponent<Combat>().healthBar_thisCharacter = hpP1;
			playerInput.GetComponent<Combat>().respawnPointsObject = respawnPoints;

		}
		else 
        {
			playerInput.GetComponent<RPS_Switching>().player = Player.P1;
			playerInput.GetComponent<RPS_Switching>().gameManager = gameManagerObject;
			playerInput.GetComponent<Combat>().healthBar_thisCharacter = hpP1;
			playerInput.GetComponent<Combat>().respawnPointsObject = respawnPoints;
		}
        
        numPlayers++;
        
    }

    //on the event that a player leaves
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        numPlayers--;
    }
}
