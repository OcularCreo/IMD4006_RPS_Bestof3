using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TMPro;
using Unity.Mathematics;
using System;
//using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem;
//using UnityEditor.Build.Content;

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

	public Manager gameManager;


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
	[SerializeField] private ParticleSystem abilityHitParticles;


	[SerializeField] private AudioSource source;
	[SerializeField] private Sound sound;



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

		source = GetComponent<AudioSource>();
		
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
			/*if(GetComponent<Movement>().playersCollided && GetComponent<Movement>().slammed) // if players have collided and slam has been done
            {
				// enemy takes damage
				HitEnemy(GetComponent<RPS_Switching>().character, enemy.GetComponent<RPS_Switching>().character);
				slamOver = true;
			}
            if (!GetComponent<Movement>().slammed) // if slam is over, reset slamOver
            {
				slamOver = false;
            }*/

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
			if (!hitting && GetComponent<RPS_Switching>().gameManager.state != GameState.RPS && !GetComponent<Abilities>().abilityInUse)
			{
				StartCoroutine(AttackAnimation());

                if (hitting && canHit && attackActive)
                {
                    //Figure out how to hit enemy
                    // alreadyHit -> prevents being hit multiple times by 1 attack
                    if (!alreadyHit)
                    {
                        alreadyHit = true;
                        HitEnemy(gameObject, enemy, characterDamage);
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

				source.PlayOneShot(sound.soundEffect_punch);
			}
			else if (GetComponent<RPS_Switching>().character == Character.scissors)
			{
				GetComponent<PlayerGFX>().scissorsAttack.SetActive(true);
				GetComponent<PlayerGFX>().scissorsIdle.SetActive(false);
				
				source.PlayOneShot(sound.soundEffect_cut);
			}
			else if (GetComponent<RPS_Switching>().character == Character.paper)
			{
				GetComponent<PlayerGFX>().paperAttack.SetActive(true);
				GetComponent<PlayerGFX>().paperIdle.SetActive(false);

				source.PlayOneShot(sound.soundEffect_rip);
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

				source.PlayOneShot(sound.soundEffect_punch);
			}
			else if (GetComponent<RPS_Switching>().character == Character.scissors)
			{
				GetComponent<PlayerGFX>().scissorsAttack.SetActive(false);
				GetComponent<PlayerGFX>().scissorsIdle.SetActive(true);

				source.PlayOneShot(sound.soundEffect_cut);
			}
			else if (GetComponent<RPS_Switching>().character == Character.paper)
			{
				GetComponent<PlayerGFX>().paperAttack.SetActive(false);
				GetComponent<PlayerGFX>().paperIdle.SetActive(true);

				source.PlayOneShot(sound.soundEffect_rip);
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

	private void HitEnemy(GameObject thisPlayer, GameObject enemyPlayer, int damage)
	{
		//hit enemy

		int enemyHealth = enemy.GetComponent<Combat>().health;
		// int enemyHealth = Combat.health;
		int damageDealt = 0;

		//Get Characters
		Character thisPlayerCharacter = thisPlayer.GetComponent<RPS_Switching>().character;
		Character enemyPlayerCharacter = enemyPlayer.GetComponent<RPS_Switching>().character;

		//Scissors to Rock
		if (thisPlayerCharacter == Character.scissors && enemyPlayerCharacter == Character.rock)
		{
			damageDealt = (int)(damage * disadvantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleLittle, enemy.transform.position, enemy.transform.rotation);
		}
		//Scissors to Paper
		else if (thisPlayerCharacter == Character.scissors && enemyPlayerCharacter == Character.paper)
		{
			damageDealt = (int)(damage * advantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleBig, enemy.transform.position, enemy.transform.rotation);
		}
		//Rock to Paper
		else if (thisPlayerCharacter == Character.rock && enemyPlayerCharacter == Character.paper)
		{
			damageDealt = (int)(damage * disadvantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleLittle, enemy.transform.position, enemy.transform.rotation);
		}
		//Rock to Scissors
		else if (thisPlayerCharacter == Character.rock && enemyPlayerCharacter == Character.scissors)
		{
			damageDealt = (int)(damage * advantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleBig, enemy.transform.position, enemy.transform.rotation);
		}
		//Paper to Rock
		else if (thisPlayerCharacter == Character.paper && enemyPlayerCharacter == Character.rock)
		{
			damageDealt = (int)(damage * advantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleBig, enemy.transform.position, enemy.transform.rotation);
		}
		//Paper to Scissors
		else if (thisPlayerCharacter == Character.paper && enemyPlayerCharacter == Character.scissors)
		{
			damageDealt = (int)(damage * disadvantageMultiplier);
			//Paricle Effect
			Instantiate(hitParticleLittle, enemy.transform.position, enemy.transform.rotation);
		}
		else {
			damageDealt = damage;
			//Paricle Effect
			Instantiate(hitParticle, enemy.transform.position, enemy.transform.rotation);
		}

		//Send Damage
		enemy.GetComponent<Combat>().takeDamage(damageDealt);
	}

	public void Die()
	{
        lives -= 1;
		healthBar_thisCharacter.GetComponent<HealthBar>().loseLife(lives);
		//livesUI.text = lives.ToString();
		//Debug.Log("Life Lost");
		if (lives <= 0)
		{
			gameManager.GetComponent<Manager>().EndGame(GetComponent<RPS_Switching>().player);
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
			//Slam Damage
			HitEnemy(gameObject, enemy, GetComponent<Abilities>().slamDamage);
			GetComponent<Abilities>().slamming = false;

			//Particles
			var debuffP = Instantiate(abilityHitParticles, enemy.transform.position, enemy.transform.rotation);
			debuffP.transform.parent = enemy.gameObject.transform;

			//GFX
			GetComponent<Abilities>().swapSprites(true, Character.rock);
		}

		//Paper
		if (gameObject.GetComponent<Abilities>().jumping && collision.gameObject.tag == "Player")
		{
			//Jump Damage
			HitEnemy(gameObject, enemy, GetComponent<Abilities>().jumpDamage);

			//Particles
			var debuffP = Instantiate(abilityHitParticles, enemy.transform.position, enemy.transform.rotation);
			debuffP.transform.parent = enemy.gameObject.transform;

			//GFX
			GetComponent<Abilities>().swapSprites(true, Character.paper);
		}

		//Scissors
		if (gameObject.GetComponent<Abilities>().dashing && collision.gameObject.tag == "Player")
		{
			//Dash Damage
			HitEnemy(gameObject, enemy, GetComponent<Abilities>().dashDamage);
			
			//Particles
			var debuffP = Instantiate(abilityHitParticles, enemy.transform.position, enemy.transform.rotation);
			debuffP.transform.parent = enemy.gameObject.transform;

			//GFX
			GetComponent<Abilities>().swapSprites(true, Character.scissors);
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
            //enemy = collision.gameObject;
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

	//function called when the player is recivig damage
	//includes knocback
	public void takeDamage(int dmg)
	{
		KnockbackEnemy();

		//Update Health
		//only apply damage to other player when in battle stage
		if (gameManager.state == GameState.battle)
		{
            //Double damage if debuff is active
            if (doubleDamage)
            {
                health = health - (dmg * 2);

            }
			//deal normal damage if there is no buff
            else
            {
                health = health - dmg;
            }

			//update the health bar UI
            healthBar_thisCharacter.GetComponent<HealthBar>().setHealth(health);

			
        }
		
	}

	private void KnockbackEnemy() 
	{
		//knockback player (disable player movement for small amount of time to prevent them from cancelling motion of knock-back)
		StartCoroutine(KnockbackTimer());


        //change vector direction (postive or negative) depending on the where enemy is facing
        float enemyFacing = (enemy.GetComponent<Controller_Movement>().isFacingRight ? 1.0f : -1f);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
		knockbackMultiplier = Mathf.Pow((((float)maxHealth - (float)health) / (float)maxHealth) + 1, 2f);
		float kbValue = knockback * knockbackMultiplier;
		//Debug.Log(kbValue);
		GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.x + (kbValue * enemyFacing), rb.velocity.y + kbValue);

	}

	//Delay how long player cannot move while being knocked back
	private IEnumerator KnockbackTimer()
	{
		GetComponent<Controller_Movement>().isBeingKnockedBack = true;

		yield return new WaitForSeconds(knockbackTime);

		GetComponent<Controller_Movement>().isBeingKnockedBack = false;

	}

	public void setRespawnPoints(GameObject respawnPointsObject) {
        respawnPoints = respawnPointsObject.GetComponentsInChildren<Transform>();
    }
}
