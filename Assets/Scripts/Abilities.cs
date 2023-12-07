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

	//OLD ROCK
	/*private float slamPower = 50f;      //variable used to determine how strong slams are
	public bool slamming = false;*/              //tells us if the player his holding the slamming button

	private float slamPower = 1000f;
	public int slamDamage = 15;
	private bool canSlam = true;
	public bool slamming = false;
	private float slamCooldown = 3f;
	private float slamDamageTimer = 0.3f;

	//Paper
	private float jumpPower = 700f;
	public int jumpDamage = 5;
	private bool canJump = true;
	public bool jumping = false;
	private float jumpCooldown = 3f;
	private float jumpDamageTimer = 0.3f;

	//Scissors
	private float dashPower = 1500f;
	public int dashDamage = 10;
	private bool canDash = true;
	public bool dashing = false;
	private float dashCooldown = 3f;
	private float dashDamageTimer = 0.3f;

	//Particles
	[SerializeField] private ParticleSystem abilityReadyParticle;

	// Start is called before the first frame update
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		//OLD ROCK
		//when the player is holding the slam button
		/*if (slamming)
		{
			//Debug.Log("slamming now");
			rb.AddForce(Vector2.down * slamPower); //keep applying the downward force times the slamming power

			//when the player finally reaches the ground
			if (GetComponent<Controller_Movement>().isGrounded())
			{
				slamming = false;
			}
		}*/

		

		if (GetComponent<Controller_Movement>().isGrounded())
		{
			if (canSlam == false) {
				Instantiate(abilityReadyParticle, gameObject.transform.position, gameObject.transform.rotation);
			}
			if (canJump == false)
			{
				Instantiate(abilityReadyParticle, gameObject.transform.position, gameObject.transform.rotation);
			}
			//Rock Ability Reset
			canSlam = true;
			slamming = false;
			//Paper Ability Reset
			canJump = true;
		}

	}

	//funciton used to read in when player hits the slam button
	public void CharacterAbility(InputAction.CallbackContext context)
	{
		if (context.action.triggered && GetComponent<RPS_Switching>().gameManager.state == GameState.battle)
		{
			Character characterType = GetComponent<RPS_Switching>().character;

			//OLD ROCK
			/*if (characterType == Character.rock)
			{
				//rb.AddForce(Vector2.down * slamPower); //old code
				slamming = true;
				Debug.Log("Slam");
			}*/

			//Rock
			if (characterType == Character.rock && canSlam)
			{
				canSlam = false;
				rb.velocity = Vector2.zero;
				rb.AddForce(Vector2.down * slamPower);

				slamming = true;

				//StartCoroutine(SlamDamageTime());
			}

			//Paper
			else if (characterType == Character.paper && canJump)
			{
				canJump = false;
				rb.velocity = Vector2.zero;
				rb.AddForce(Vector2.up * jumpPower);

				StartCoroutine(JumpDamageTime());
			}

			//Scissors
			else if (characterType == Character.scissors && canDash)
			{
				canDash = false;
				bool facingR = GetComponent<Controller_Movement>().isFacingRight;
				rb.velocity = Vector2.zero;

				if (facingR) {
					rb.AddForce(Vector2.right * dashPower);
				}
                else
                {
					rb.AddForce(Vector2.left * dashPower);
				}

				StartCoroutine(DashDamageTime());
				StartCoroutine(DashTime());
			}


		}

	}

	//Rock
	/*private IEnumerator SlamDamageTime()
	{
		slamming = true;

		yield return new WaitForSeconds(slamDamageTimer);

		slamming = false;
	}*/

	//Paper
	private IEnumerator JumpDamageTime()
	{
		jumping = true;

		yield return new WaitForSeconds(jumpDamageTimer);

		jumping = false;
	}

	//Scissors
	private IEnumerator DashTime()
	{
		canDash = false;

		yield return new WaitForSeconds(dashCooldown);

		canDash = true;

		Instantiate(abilityReadyParticle, gameObject.transform.position, gameObject.transform.rotation);
	}

	private IEnumerator DashDamageTime()
	{
		dashing = true;

		yield return new WaitForSeconds(dashDamageTimer);

		dashing = false;
	}
}
