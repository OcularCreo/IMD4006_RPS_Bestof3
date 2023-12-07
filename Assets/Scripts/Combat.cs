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
using UnityEngine.InputSystem;

public class Combat : MonoBehaviour
{
	//[SerializeField] private TextMeshProUGUI healthUI;
	//[SerializeField] private TextMeshProUGUI livesUI;

	[SerializeField] public int lives = 3;
	[SerializeField] private int maxHealth = 100;
	[SerializeField] public int health;
    [SerializeField] protected int characterDamage = 5;
    [SerializeField] private float advantageMultiplier = 1.5f;
	[SerializeField] private float disadvantageMultiplier = 0.5f;
	private float attackSpeed = 0.3f;
	private float attackActiveTime = 0.1f;
	private bool attackActive = false;

	private float knockback = 6f;
	private float knockbackMultiplier;
	private float knockbackTime = 0.5f;

	private bool doubleDamage = false;
	private float debuffTime = 3.0f;

	//[SerializeField] private GameObject weapon;
	//[SerializeField] public GameObject respawnPointsObject;
	private Transform[] respawnPoints;

	private bool canHit = false;
	private bool hitting = false;
	public bool alreadyHit = false;

	public bool slamOver = false;

	public bool hasRespawned = true;

	public GameObject enemy;


	// [SerializeField] private GameObject health_bar;

	// private float healthBarNum;
	// private float healthBarLocalPosition;
	
	//HEALTH BAR
	[SerializeField] public GameObject healthBar_thisCharacter;
	//[SerializeField] private HealthBar healthBar_enemyCharacter;

	

	[SerializeField] private ParticleSystem hitParticle;
	[SerializeField] private ParticleSystem hitParticleBig;
	[SerializeField] private ParticleSystem hitParticleLittle;
	[SerializeField] private ParticleSystem debuffParticles;



    void Start()
    {
		health = maxHealth;
		//respawnPoints = respawnPointsObject.GetComponentsInChildren<Transform>();
		//healthUI.text = health.ToString();
		//livesUI.text = lives.ToString();

		//set both character health bar to maxvalue
		healthBar_thisCharacter.GetComponent<HealthBar>().setHealth(maxHealth);
	
		//healthBar_enemyCharacter.setMaxHealth(maxHealth);

		
		// healthBarNum = health_bar.GetComponent<Transform>().localScale.x;
        // healthBarLocalPosition = health_bar.GetComponent<Transform>().position.x;
		// opponentHealth = health_bar.transform.parent.gameObject.GetComponent<Combat>().health;
		
	}

    void FixedUpdate()
    {
		//Debug.Log("combat scsript: "+ health);
		if (GetComponent<RPS_Switching>().gameManager.state != GameState.RPS){ 
		
			//If player attacks play animation
			//CheckForHitAnimation();

			//If the player is attacking and is in range of the other player
			// Hitting -> Is the animation playing
			// canHit -> Is the enemy in the hitbox
			// attackActive -> Within attack time frame
			/*if (hitting && canHit && attackActive) {
				//Figure out how to hit enemy
				// alreadyHit -> prevents being hit multiple times by 1 attack
				if (!alreadyHit) {
					HitEnemy(GetComponent<RPS_Switching>().character, enemy.GetComponent<RPS_Switching>().character);
					alreadyHit = true;
				}
			}*/



			/*if (gameObject.GetComponent<Controller_Movement>().slamming)
			{

			}*/

			// Slam does damage
			if(GetComponent<Movement>().playersCollided && GetComponent<Movement>().slammed) // if players have collided and slam has been done
            {
				// enemy takes damage
				HitEnemy(GetComponent<RPS_Switching>().character, enemy.GetComponent<RPS_Switching>().character);
				slamOver = true;
			}
            if (!GetComponent<Movement>().slammed) // if slam is over, reset slamOver
            {
				slamOver = false;
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

	public void onAttack(InputAction.CallbackContext context)
	{
		//when the player lifts the trigger
		/*if (context.canceled)
		{
			slamming = false;
		}*/

		//when the player presses B attack
		if (context.action.triggered)
		{
			if (!hitting && GetComponent<RPS_Switching>().gameManager.state != GameState.RPS)
			{
				StartCoroutine(AttackAnimation());

                if (hitting && canHit && attackActive)
                {
                    //Figure out how to hit enemy
                    // alreadyHit -> prevents being hit multiple times by 1 attack
                    if (!alreadyHit)
                    {
                        alreadyHit = true;
                        HitEnemy(GetComponent<RPS_Switching>().character, enemy.GetComponent<RPS_Switching>().character);
                    }
                }
            }
		}

	}

	//If player attacks play attack animation
	/*private void CheckForHitAnimation()
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
	}*/

	// Animation for the attack
	private IEnumerator AttackAnimation()
	{
		hitting = true;
		attackActive = true;


		if (GetComponent<RPS_Switching>().player == Player.P1) 
		{
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
			attackActive = false;

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

		if (GetComponent<RPS_Switching>().player == Player.P2)
		{
			//Animate attack
			//weapon.GetComponent<Weapon>().weaponPivot.transform.Rotate(0, 0, -90);
			if (GetComponent<RPS_Switching>().character == Character.rock)
			{
				GetComponent<PlayerGFX>().rockAttack2.SetActive(true);
				GetComponent<PlayerGFX>().rockIdle2.SetActive(false);
			}
			else if (GetComponent<RPS_Switching>().character == Character.scissors)
			{
				GetComponent<PlayerGFX>().scissorsAttack2.SetActive(true);
				GetComponent<PlayerGFX>().scissorsIdle2.SetActive(false);
			}
			else if (GetComponent<RPS_Switching>().character == Character.paper)
			{
				GetComponent<PlayerGFX>().paperAttack2.SetActive(true);
				GetComponent<PlayerGFX>().paperIdle2.SetActive(false);
			}

			yield return new WaitForSeconds(attackActiveTime);
			attackActive = false;

			yield return new WaitForSeconds(attackSpeed);

			//animate idle
			//weapon.GetComponent<Weapon>().weaponPivot.transform.Rotate(0, 0, 90);
			if (GetComponent<RPS_Switching>().character == Character.rock)
			{
				GetComponent<PlayerGFX>().rockAttack2.SetActive(false);
				GetComponent<PlayerGFX>().rockIdle2.SetActive(true);
			}
			else if (GetComponent<RPS_Switching>().character == Character.scissors)
			{
				GetComponent<PlayerGFX>().scissorsAttack2.SetActive(false);
				GetComponent<PlayerGFX>().scissorsIdle2.SetActive(true);
			}
			else if (GetComponent<RPS_Switching>().character == Character.paper)
			{
				GetComponent<PlayerGFX>().paperAttack2.SetActive(false);
				GetComponent<PlayerGFX>().paperIdle2.SetActive(true);
			}

            hitting = false;
            alreadyHit = false;
        }

		
	}

	private void HitEnemy(Character thisPlayer, Character enemyPlayer)
	{
		//hit enemy

		int enemyHealth = enemy.GetComponent<Combat>().health;
		// int enemyHealth = Combat.health;
		int damageDealt = 0;

		//Scissors to Rock
		if (thisPlayer == Character.scissors && enemyPlayer == Character.rock)
		{
			damageDealt = (int)(characterDamage * disadvantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleLittle, enemy.transform.position, enemy.transform.rotation);
		}
		//Scissors to Paper
		else if (thisPlayer == Character.scissors && enemyPlayer == Character.paper)
		{
			damageDealt = (int)(characterDamage * advantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleBig, enemy.transform.position, enemy.transform.rotation);
		}
		//Rock to Paper
		else if (thisPlayer == Character.rock && enemyPlayer == Character.paper)
		{
			damageDealt = (int)(characterDamage * disadvantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleLittle, enemy.transform.position, enemy.transform.rotation);
		}
		//Rock to Scissors
		else if (thisPlayer == Character.rock && enemyPlayer == Character.scissors)
		{
			damageDealt = (int)(characterDamage * advantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleBig, enemy.transform.position, enemy.transform.rotation);
		}
		//Paper to Rock
		else if (thisPlayer == Character.paper && enemyPlayer == Character.rock)
		{
			damageDealt = (int)(characterDamage * advantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleBig, enemy.transform.position, enemy.transform.rotation);
		}
		//Paper to Scissors
		else if (thisPlayer == Character.paper && enemyPlayer == Character.scissors)
		{
			damageDealt = (int)(characterDamage * disadvantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleLittle, enemy.transform.position, enemy.transform.rotation);
		}
		else {
			damageDealt = characterDamage;
			//Paricle Effect
			Instantiate(hitParticle, enemy.transform.position, enemy.transform.rotation);
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

		//healtBar();
	}

	public void Die()
	{
        lives -= 1;
		healthBar_thisCharacter.GetComponent<HealthBar>().loseLife(lives);
		//livesUI.text = lives.ToString();
		//Debug.Log("Life Lost");
		if (lives <= 0)
		{
			GetComponent<RPS_Switching>().gameManager.GetComponent<Manager>().EndGame(GetComponent<RPS_Switching>().player);
			gameObject.SetActive(false);
		}
		else {
			Respawn();
        }

		/*if (lives < 2) {
            GetComponent<PlayerIcons>().EnableLowLivesIcon();
        }*/
	}

	public void Respawn()
	{
		if (respawnPoints == null) {
            setRespawnPoints(GameObject.FindGameObjectWithTag("RespawnPoints"));
        }
		//Debug.Log("Respawn");
		health = maxHealth;
		//healthUI.text = health.ToString();
		healthBar_thisCharacter.GetComponent<HealthBar>().setHealth(health);

		/*GetComponent<PlayerIcons>().DisableLowHealthIcon();*/

		GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

		int randSpawn = UnityEngine.Random.Range(1, respawnPoints.Length);
		//Debug.Log(randSpawn);

		gameObject.transform.position = respawnPoints[randSpawn].gameObject.transform.position;

		hasRespawned = true;
	}

	// Damage taken for switching characters - will change
	public void switchDamage()
	{
		//health = health - 20;
		//healthUI.text = health.ToString();

		//Start Particles
		var debuffP = Instantiate(debuffParticles, this.transform.position, this.transform.rotation);
		debuffP.transform.parent = gameObject.transform;
		//Start Coroutine
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
		//OLD ROCK
        /*if(gameObject.GetComponent<Abilities>().slamming && collision.gameObject.tag == "Player")
		{

			Debug.Log("Slammed an enemy");

			if(gameObject.GetComponent<RPS_Switching>().character == Character.rock)
			{
                enemy.GetComponent<Combat>().takeDamage(15);
            } else if (gameObject.GetComponent<RPS_Switching>().character == Character.scissors) {
                enemy.GetComponent<Combat>().takeDamage(10);
            }

			//gameObject.GetComponent<Controller_Movement>().slamming = false;
		}*/

		//Rock
		if (gameObject.GetComponent<Abilities>().slamming && collision.gameObject.tag == "Player")
		{
			Debug.Log("Slammed into enemy");
			enemy.GetComponent<Combat>().takeDamage(GetComponent<Abilities>().slamDamage);
		}

		//Paper
		if (gameObject.GetComponent<Abilities>().jumping && collision.gameObject.tag == "Player")
		{
			Debug.Log("Jumped into enemy");
			enemy.GetComponent<Combat>().takeDamage(GetComponent<Abilities>().jumpDamage);
		}

		//Scissors
		if (gameObject.GetComponent<Abilities>().dashing && collision.gameObject.tag == "Player")
		{
			Debug.Log("Dashed into enemy");
			enemy.GetComponent<Combat>().takeDamage(GetComponent<Abilities>().dashDamage);
		}

		
	}

    private void OnTriggerEnter2D(Collider2D collision)
	{


		//Debug.Log("Check in");
		if (collision.gameObject.GetComponent<Combat>() != null)
		{
			CanHitEnterRange();

            /*if (!canHit)
            {
                //Debug.Log("Entered Range");
                canHit = true;
            }*/
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
		if (enemy.GetComponent<Controller_Movement>().isFacingRight == true)
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
			//when do damage 
			
		}
		else {
			health = health - dmg;
			//Debug.Log(dmg);
		}
		//healthUI.text = health.ToString();
		//Debug.Log("Enemy health: " + enemy.GetComponent<Combat>().health);

		/*if (health < 20) {
            GetComponent<PlayerIcons>().EnableLowHealthIcon();
        }*/

		healthBar_thisCharacter.GetComponent<HealthBar>().setHealth(health);
		
	}

	private IEnumerator KnockbackTimer()
	{
		GetComponent<Controller_Movement>().isBeingKnockedBack = true;
		//Debug.Log(GetComponent<Movement>().isBeingKnockedBack);

		yield return new WaitForSeconds(knockbackTime);

		GetComponent<Controller_Movement>().isBeingKnockedBack = false;
		//Debug.Log(GetComponent<Movement>().isBeingKnockedBack);
	}


	// private void healtBar(){

	//         if(opponentHealth >20 && opponentHealth < 100){
	//         healthBarNum = health_bar.GetComponent<Transform>().localScale.x - (1-(opponentHealth/100f));
	//         healthBarLocalPosition =  (health/100f);
	//         health_bar.GetComponent<Transform>().localScale = new Vector2(healthBarNum,health_bar.GetComponent<Transform>().localScale.y);
	//         health_bar.GetComponent<Transform>().position = new Vector2(GetComponent<Transform>().position.x - healthBarLocalPosition,health_bar.GetComponent<Transform>().position.y);
	//         //Debug.Log(gameObject.name + "is hit" + opponentHealth);
	// 		}
	// 		else if(opponentHealth < 20){
	// 		health_bar.GetComponent<Transform>().localScale = new Vector2(0,health_bar.GetComponent<Transform>().localScale.y);
	// 		}
	// 		else if(opponentHealth == 100){
	// 		health_bar.GetComponent<Transform>().localScale = new Vector2(3f,health_bar.GetComponent<Transform>().localScale.y);
	// 		}

	// }

	public void setRespawnPoints(GameObject respawnPointsObject) {
        respawnPoints = respawnPointsObject.GetComponentsInChildren<Transform>();
    }
}
