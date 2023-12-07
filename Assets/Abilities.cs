using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Abilities : MonoBehaviour
{
	//Generic
	private Rigidbody2D rb;

	//Rock
	private float slamPower = 50f;      //variable used to determine how strong slams are
	public bool slamming;              //tells us if the player his holding the slamming button

	//Paper


	//Scissors
	private float dashPower = 1500f;

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		slamming = false;

	}

    // Update is called once per frame
    void FixedUpdate()
    {
		//when the player is holding the slam button
		if (slamming)
		{
			//Debug.Log("slamming now");
			rb.AddForce(Vector2.down * slamPower); //keep applying the downward force times the slamming power

			//when the player finally reaches the ground
			if (GetComponent<Controller_Movement>().isGrounded())
			{
				slamming = false;
			}
		}
	}

	//funciton used to read in when player hits the slam button
	public void characterAbility(InputAction.CallbackContext context)
	{
		if (context.action.triggered)
		{
			Character characterType = GetComponent<RPS_Switching>().character;

			if (characterType == Character.rock)
			{
				//rb.AddForce(Vector2.down * slamPower); //old code
				slamming = true;
				Debug.Log("Slam");
			}
			else if (characterType == Character.paper)
			{

			}
			else if (characterType == Character.scissors)
			{
				bool facingR = GetComponent<Controller_Movement>().isFacingRight;
				if (facingR) {
					rb.AddForce(Vector2.right * dashPower);
				}
                else
                {
					rb.AddForce(Vector2.left * dashPower);
				}
			}


		}

	}
}
