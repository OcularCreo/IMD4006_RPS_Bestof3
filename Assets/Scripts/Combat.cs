using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TMPro;
using Unity.Mathematics;
using System;
using UnityEditor.Experimental.GraphView;

public class Combat : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI healthUI;
	[SerializeField] private TextMeshProUGUI livesUI;

	[SerializeField] private int lives = 3;
	[SerializeField] private int maxHealth = 100;
	[SerializeField] private int health;
    [SerializeField] private int characterDamage = 5;
    [SerializeField] private float advantageMultiplier = 1.5f;
	[SerializeField] private float disadvantageMultiplier = 0.5f;
	private float attackSpeed = 0.3f;
	private float attackActiveTime = 0.1f;
	private bool attackActive = false;

	private float knockback = 3f;
	private float knockbackMultiplier;
	private float knockbackTime = 0.5f;

	private bool doubleDamage = false;
	private float debuffTime = 3.0f;

	//[SerializeField] private GameObject weapon;
	[SerializeField] private GameObject respawnPointsObject;
	private Transform[] respawnPoints;


    private bool canHit = false;
    private bool hitting = false;
    private bool alreadyHit = false;

    public GameObject enemy;

    void Start()
    {

		health = maxHealth;
		respawnPoints = respawnPointsObject.GetComponentsInChildren<Transform>();
		healthUI.text = health.ToString();
		livesUI.text = lives.ToString();
	}

    void Update()
    {
		if (GetComponent<RPS_Switching>().gameManager.state != GameState.RPS){ 
		
			//If player attacks play animation
			CheckForHitAnimation();

			//If the player is attacking and is in range of the other player
			if (hitting && canHit && attackActive) {
				//Figure out how to hit enemy
				if (!alreadyHit) {
					HitEnemy(GetComponent<RPS_Switching>().character, enemy.GetComponent<RPS_Switching>().character);
					alreadyHit = true;
				}
			}
		}

		if (health <= 0) {
			Die();
		}
    }

    //Entering and exiting range
    public void CanHitEnterRange()

	{
		if (!canHit)
		{
			//Debug.Log("Entered Range");
			canHit = true;
		}
	}

	public void CanHitExitRange()
	{
		if (canHit)
		{
			//Debug.Log("Left Range");
			canHit = false;
		}
	}

	//If player attacks play attack animation
	private void CheckForHitAnimation()
	{
		if (GetComponent<RPS_Switching>().player == Player.P1)
		{
			if (Input.GetKeyDown(KeyCode.Space) && !hitting)
			{
				StartCoroutine(StartCooldown());
			}
		}
		
		if (GetComponent<RPS_Switching>().player == Player.P2)
		{
			if (Input.GetKeyDown(KeyCode.Period) && !hitting)
			{
				StartCoroutine(StartCooldown());
			}
		}
	}

	// Animation for the attack
	private IEnumerator StartCooldown()
	{
		hitting = true;
		attackActive = true;

		//Animate attack
		//weapon.GetComponent<Weapon>().weaponPivot.transform.Rotate(0, 0, -90);
		if (GetComponent<RPS_Switching>().character == Character.rock)
		{
			GetComponent<PlayerGFX>().rockAttack.SetActive(true);
			GetComponent<PlayerGFX>().rockIdle.SetActive(false);
		}
		else if (GetComponent<RPS_Switching>().character == Character.scissors)
		{
			GetComponent<PlayerGFX>().scissorsAttack.SetActive(true);
			GetComponent<PlayerGFX>().scissorsIdle.SetActive(false);
		}
		else if (GetComponent<RPS_Switching>().character == Character.paper)
		{
			GetComponent<PlayerGFX>().paperAttack.SetActive(true);
			GetComponent<PlayerGFX>().paperIdle.SetActive(false);
		}

		yield return new WaitForSeconds(attackActiveTime);
		attackActive = false; ;

		yield return new WaitForSeconds(attackSpeed);

		//animate idle
		//weapon.GetComponent<Weapon>().weaponPivot.transform.Rotate(0, 0, 90);
		if (GetComponent<RPS_Switching>().character == Character.rock)
		{
			GetComponent<PlayerGFX>().rockAttack.SetActive(false);
			GetComponent<PlayerGFX>().rockIdle.SetActive(true);
		}
		else if (GetComponent<RPS_Switching>().character == Character.scissors)
		{
			GetComponent<PlayerGFX>().scissorsAttack.SetActive(false);
			GetComponent<PlayerGFX>().scissorsIdle.SetActive(true);
		}
		else if (GetComponent<RPS_Switching>().character == Character.paper)
		{
			GetComponent<PlayerGFX>().paperAttack.SetActive(false);
			GetComponent<PlayerGFX>().paperIdle.SetActive(true);
		}


		hitting = false;
		alreadyHit = false;
	}

	private void HitEnemy(Character thisPlayer, Character enemyPlayer)
	{
		//hit enemy

		int enemyHealth = enemy.GetComponent<Combat>().health;
		int damageDealt = 0;

		//Scissors to Rock
		if (thisPlayer == Character.scissors && enemyPlayer == Character.rock)
		{
			damageDealt = (int)(characterDamage * disadvantageMultiplier);
		}
		//Scissors to Paper
		else if (thisPlayer == Character.scissors && enemyPlayer == Character.paper)
		{
			damageDealt = (int)(characterDamage * advantageMultiplier);
		}
		//Rock to Paper
		else if (thisPlayer == Character.rock && enemyPlayer == Character.paper)
		{
			damageDealt = (int)(characterDamage * disadvantageMultiplier);
		}
		//Rock to Scissors
		else if (thisPlayer == Character.rock && enemyPlayer == Character.scissors)
		{
			damageDealt = (int)(characterDamage * advantageMultiplier);
		}
		//Paper to Rock
		else if (thisPlayer == Character.paper && enemyPlayer == Character.rock)
		{
			damageDealt = (int)(characterDamage * advantageMultiplier);
		}
		//Paper to Scissors
		else if (thisPlayer == Character.paper && enemyPlayer == Character.scissors)
		{
			damageDealt = (int)(characterDamage * disadvantageMultiplier);
		}
		else {
			damageDealt = characterDamage;
		}

		//enemyHealth -= damageDealt;
		//Debug.Log(damageDealt);
		//Debug.Log(enemyHealth);

		enemy.GetComponent<Combat>().takeDamage(damageDealt);

		/*//knockback
		Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
		float characterFacing = 1;
		Debug.Log(GetComponent<Movement>().facingRight);
		if (GetComponent<Movement>().facingRight == true)
		{
			characterFacing = 1;
		}
		else
		{
			characterFacing = -1;
		}
		enemyRb.velocity = new Vector2(enemyRb.velocity.x + (knockback * characterFacing), enemyRb.velocity.y + knockback);

		enemy.GetComponent<Combat>().health = enemyHealth;

        enemy.GetComponent<Combat>().healthUI.text = enemyHealth.ToString();
        //Debug.Log("Enemy health: " + enemy.GetComponent<Combat>().health);*/
	}

	public void Die()
	{
		lives -= 1;
		livesUI.text = lives.ToString();
		//Debug.Log("Life Lost");
		if (lives <= 0)
		{
			gameObject.SetActive(false);
		}
		else {
			Respawn();
		}
	}

	public void Respawn()
	{
		//Debug.Log("Respawn");
		health = maxHealth;
		healthUI.text = health.ToString();

		GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

		int randSpawn = UnityEngine.Random.Range(1, respawnPoints.Length);
		//Debug.Log(randSpawn);

		gameObject.transform.position = respawnPoints[randSpawn].gameObject.transform.position;
	}

	// Damage taken for switching characters - will change
	public void switchDamage()
	{
		//health = health - 20;
		//healthUI.text = health.ToString();

		StartCoroutine(StartDebuffTime());
	}

	private IEnumerator StartDebuffTime()
	{
		doubleDamage = true;
		yield return new WaitForSeconds(debuffTime);
		doubleDamage = false;
	}

	/*public void WeaponEnable() {
		weapon.SetActive(true);
	}

	public void WeaponDisable()
	{
		weapon.SetActive(false);
	}*/

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("Check in");
		if (collision.gameObject.GetComponent<Combat>() != null)
		{
			CanHitEnterRange();
			enemy = collision.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		//Debug.Log("Check out");
		if (collision.gameObject.GetComponent<Combat>() != null)
		{
			CanHitExitRange();
		}
	}

	public void takeDamage(int dmg)
	{
		//knockback
		StartCoroutine(KnockbackTimer());
		float enemyFacing = 1;
		//Debug.Log(GetComponent<Movement>().facingRight);
		if (enemy.GetComponent<Movement>().facingRight == true)
		{
			//right
			enemyFacing = 1;
		}
		else
		{
			//left
			enemyFacing = -1;
		}
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		knockbackMultiplier = Mathf.Pow((((float)maxHealth - (float)health) / (float)maxHealth) + 1, 2f);
		float kbValue = knockback * knockbackMultiplier;
		//Debug.Log(kbValue);
		GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x + (kbValue * enemyFacing), rb.velocity.y + kbValue);

		//Update Health
		if (doubleDamage)
		{
			health = health - (dmg * 2);
			//Debug.Log(dmg * 2);
		}
		else {
			health = health - dmg;
			//Debug.Log(dmg);
		}
		healthUI.text = health.ToString();
		//Debug.Log("Enemy health: " + enemy.GetComponent<Combat>().health);

		
	}

	private IEnumerator KnockbackTimer()
	{
		GetComponent<Movement>().isBeingKnockedBack = true;
		//Debug.Log(GetComponent<Movement>().isBeingKnockedBack);

		yield return new WaitForSeconds(knockbackTime);

		GetComponent<Movement>().isBeingKnockedBack = false;
		//Debug.Log(GetComponent<Movement>().isBeingKnockedBack);
	}

}

