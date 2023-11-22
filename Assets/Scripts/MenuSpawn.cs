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

    public int numPlayers = 0;                                          //keep track of the number of players. Used to start game
    
    //when a player joins this funciton is called
    void OnPlayerJoined(PlayerInput playerInput)
    {
        
        //spawn the player at their given location
        playerInput.gameObject.transform.position = spawnPoint.transform.position;

        //attach the player as a target
        cameraTargetGroup.m_Targets[numPlayers].target = playerInput.gameObject.transform;

        numPlayers++;
        
    }

    //on the event that a player leaves
    private void OnPlayerLeft(PlayerInput playerInput)
    {
        numPlayers--;
    }
}
