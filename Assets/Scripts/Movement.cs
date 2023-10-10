using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb; // the player rigid body
    private float moveSpeed = 3f; // speed moving left and right
    private float jumpSpeed = 5f; // speed of jump

    private bool jumping = false; // check whether the player has jumped
    private int jumpCount = 0; // counter for double jump

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // freeze rotation of the player, because otherwise the circle rotates when it moves
    }

    // Update is called once per frame
    void Update()
    {
        // Player 1
        if(GetComponent<RPS_Switching>().player == Player.P1)
        {
            // If player presses a, go left
            if (Input.GetKeyDown("a"))
            {
                rb.velocity = new Vector2(-moveSpeed, 0);
            }
            // if player presses d, go right
            if (Input.GetKeyDown("d"))
            {
                rb.velocity = new Vector2(moveSpeed, 0);
            }
            // if player presses w and player has not jumped twice yet, let player jump
            if (Input.GetKeyDown("w") && !(jumping))
            {
                rb.velocity = Vector2.up * jumpSpeed;
                jumpCount += 1;
                jumping = checkJump();
            }
        }
        // Player 2
        else if (GetComponent<RPS_Switching>().player == Player.P2)
        {
            // If player presses left arrow, go left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                rb.velocity = new Vector2(-moveSpeed, 0);
            }
            // if player presses right arrow, go right
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rb.velocity = new Vector2(moveSpeed, 0);
            }
            // if player presses up arrow and player has not jumped twice yet, let player jump
            if (Input.GetKeyDown(KeyCode.UpArrow) && !(jumping))
            {
                rb.velocity = Vector2.up * jumpSpeed;
                jumpCount += 1;
                jumping = checkJump();
            }
        }


    }

    // check if the player has jumped twice
    private bool checkJump()
    {
        if(jumpCount >= 2)
        {
            return true;
        }
        return false;
    }

    // check if the player has collided with the platform
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // if collided, allow the player to jump again
        if (collision.gameObject.tag == "Platform")
        {
            // reset jump variables
            jumping = false;
            jumpCount = 0;
        }
        //Debug.Log("collision with " + collision.gameObject.name);
    }

}
