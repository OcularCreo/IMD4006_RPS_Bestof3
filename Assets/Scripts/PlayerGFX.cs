using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGFX : MonoBehaviour
{
    //***** PLAYER 1 SPRITES *******//
    [Header("Player 1 Sprites")]

    //ROCK
    [Header("Rock")]
    [SerializeField] public GameObject rockIdle;
	[SerializeField] public GameObject rockAttack;
	[SerializeField] public GameObject rockChange;
	[SerializeField] public GameObject rockAbility;

    //PAPER
    [Header("Paper")]
    [SerializeField] public GameObject paperIdle;
	[SerializeField] public GameObject paperAttack;
	[SerializeField] public GameObject paperChange;
    [SerializeField] public GameObject paperAbility;

    //SCISSORS
    [Header("Scissors")]
    [SerializeField] public GameObject scissorsIdle;
    [SerializeField] public GameObject scissorsAttack;
    [SerializeField] public GameObject scissorsChange;
    [SerializeField] public GameObject scissorsAbility;


    //***** PLAYER 2 SPRITES *******//
    [Header("Player 2 Sprites")]

    //ROCK
    [Header("Rock")]
    [SerializeField] public GameObject rockIdle2;
	[SerializeField] public GameObject rockAttack2;
	[SerializeField] public GameObject rockChange2;
    [SerializeField] public GameObject rockAbility2;

    //PAPER
    [Header("Paper")]
    [SerializeField] public GameObject paperIdle2;
	[SerializeField] public GameObject paperAttack2;
	[SerializeField] public GameObject paperChange2;
    [SerializeField] public GameObject paperAbility2;

    //SCISSORS
    [Header("Scissors")]
    [SerializeField] public GameObject scissorsIdle2;
    [SerializeField] public GameObject scissorsAttack2;
    [SerializeField] public GameObject scissorsChange2;
    [SerializeField] public GameObject scissorsAbility2;
}
