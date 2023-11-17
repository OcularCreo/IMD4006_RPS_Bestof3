using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller_Movement : MonoBehaviour
{
    //ground check variables
    private Rigidbody2D rb;                         //rigid body of object script is attached to
    [SerializeField] public Transform groundCheck;  //getting transform of empty object used for player ground checking
    [SerializeField] public LayerMask groundLayer;  //referencing unity layer mask name ground

    //movement variables
    private float horizontal;           //variable used for horizontal (left and right) movement
    private float speed = 8f;           //variable used to determine speed of player
    private float jumpingPower = 8f;    //variable used to determine how high player jumps
    private bool isFacingRight = true;  //variable used for determining player orentation

    //extra jump variables
    private int extraJumps;
    public int extraJumpValues;

    // Start is called before the first frame update
    void Start()
    {
        //getting values from player object
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        extraJumps = extraJumpValues;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        //when the player touches the ground again reset the extra jump height
        if (isGrounded())
        {
            extraJumps = extraJumpValues;
            Debug.Log("PLAYER JUMPS RESET");

        }

    }

    //function returns true if the gameObject attached to this script is grounded
    //using emptybody transform for groundcheck position
    //using Unity Layer Mask named ground for platforms
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.5f, groundLayer);
    }

    //function called when player moves left gamepad / controller stick
    public void onMove(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    //function called when the south controller button is pressed
    public void onJump(InputAction.CallbackContext context)
    {


        //Debug.Log("Context Triggered: " + context.action.triggered);
        //Debug.Log("Extra Jumps: " + extraJumps);

        //context.performed returns true when the input has been pressed
        //this case it is the south gamepad button (xbox = A button)
        if (context.action.triggered && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpingPower;
            extraJumps--;

            //Debug.Log("Accessed Now");

        }

    }
}
