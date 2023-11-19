using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mvmnt_2 : MonoBehaviour
{
    //***** GROUND CHECK VARIABLES *****
    private Rigidbody2D rb;                         //rigid body of object script is attached to
    [SerializeField] public Transform groundCheck;  //getting transform of empty object used for player ground checking
    [SerializeField] public LayerMask groundLayer;  //referencing unity layer mask name ground

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
