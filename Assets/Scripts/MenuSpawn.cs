using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEditor.Tilemaps;

public class MenuSpawn : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private GameObject spawnPoint;                     //transform that sets the spawn location
    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;  //camera target group object

	[SerializeField] private Manager gameManagerObject;
	[SerializeField] private GameObject respawnPoints;
	[SerializeField] private GameObject hpP1;
	[SerializeField] private GameObject hpP2;

	[SerializeField] private GameObject p1Join;
	[SerializeField] private GameObject p2Join;
	[SerializeField] private GameObject yStart;

	[SerializeField] private GameObject menuCameraTarget;


	public int numPlayers = 0;                                          //keep track of the number of players. Used to start game


    //testing out manually joining players
    public InputAction joinAction;
    
    //when a player joins this funciton is called
    void OnPlayerJoined(PlayerInput playerInput)
    {
        
        //spawn the player at their given location
        playerInput.gameObject.transform.position = spawnPoint.transform.position;

        playerInput.GetComponent<RPS_Switching>().gameManager = gameManagerObject;
        playerInput.GetComponent<Combat>().gameManager = gameManagerObject;

        //attach the player as a target
        cameraTargetGroup.m_Targets[numPlayers].target = playerInput.gameObject.transform;

        if (numPlayers > 0)
        {
			playerInput.GetComponent<RPS_Switching>().player = Player.P2;
			//playerInput.GetComponent<RPS_Switching>().gameManager = gameManagerObject;
			playerInput.GetComponent<Combat>().healthBar_thisCharacter = hpP2;
            //playerInput.GetComponent<Combat>().respawnPointsObject = respawnPoints;
            gameManagerObject.player2 = playerInput.gameObject;

            playerInput.GetComponent<Combat>().enemy = gameManagerObject.player1;
			gameManagerObject.player1.GetComponent<Combat>().enemy = gameManagerObject.player2;

            if (menuCameraTarget != null)
            {
				menuCameraTarget.GetComponent<MenuCameraTarget>().player2 = playerInput.gameObject;
			}

			p2Join.SetActive(false);
			yStart.SetActive(true);

		}
		else 
        {
			playerInput.GetComponent<RPS_Switching>().player = Player.P1;
			//playerInput.GetComponent<RPS_Switching>().gameManager = gameManagerObject;
			playerInput.GetComponent<Combat>().healthBar_thisCharacter = hpP1;
			//playerInput.GetComponent<Combat>().respawnPointsObject = respawnPoints;
			gameManagerObject.player1 = playerInput.gameObject;

			if (menuCameraTarget != null)
			{
				menuCameraTarget.GetComponent<MenuCameraTarget>().player1 = playerInput.gameObject;
			}

			p1Join.SetActive(false);
		}
        
        numPlayers++;

        //prenting error
        if(numPlayers > 1)
        {
            GetComponent<PlayerInputManager>().DisableJoining();
        }
        
    }

    //on the event that a player leaves
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        numPlayers--;
    }

    private void Start()
    {
        joinAction.Enable();
    }

}
