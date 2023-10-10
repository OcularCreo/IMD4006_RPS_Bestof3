using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Combat : MonoBehaviour
{
	[SerializeField] private int lives = 3;
	[SerializeField] private int health = 100;
    [SerializeField] private int characterDamage = 5;
    [SerializeField] private float advantageMultiplier = 1.5f;
	[SerializeField] private float disadvantageMultiplier = 0.5f;
	[SerializeField] public float attackSpeed = 0.3f;

	[SerializeField] private GameObject weapon;
	[SerializeField] private GameObject respawnPointsObject;
	private Transform[] respawnPoints;

	private bool canHit = false;
	private bool hitting = false;
	private bool alreadyHit = false;

	public GameObject enemy;

	void Start()
    {
		respawnPoints = respawnPointsObject.GetComponentsInChildren<Transform>(); ;
	}

    void Update()
    {
		//If player attacks play animation
		CheckForHitAnimation();

		//If the player is attacking and is in range of the other player
		if (hitting == true && canHit == true) {
			//Figure out how to hit enemy
			if (!alreadyHit) {
				HitEnemy(GetComponent<RPS_Switching>().character, enemy.GetComponent<RPS_Switching>().character);
				alreadyHit = true;
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
			if (Input.GetKeyDown("e") && !hitting)
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

	private IEnumerator StartCooldown()
	{
		hitting = true;
		weapon.GetComponent<Weapon>().weaponPivot.transform.Rotate(0, 0, -90);
		yield return new WaitForSeconds(attackSpeed);
		weapon.GetComponent<Weapon>().weaponPivot.transform.Rotate(0, 0, 90);
		hitting = false;
		alreadyHit = false;
	}

	private void HitEnemy(Character thisPlayer, Character enemyPlayer)
	{
		//hit enemy

		int enemyHealth = enemy.GetComponent<Combat>().health;

		//Scissors to Rock
		if (thisPlayer == Character.scissors && enemyPlayer == Character.rock)
		{
			enemyHealth -= (int)(characterDamage * disadvantageMultiplier);
		}
		//Scissors to Paper
		else if (thisPlayer == Character.scissors && enemyPlayer == Character.paper)
		{
			enemyHealth -= (int)(characterDamage * advantageMultiplier);
		}
		//Rock to Paper
		else if (thisPlayer == Character.rock && enemyPlayer == Character.paper)
		{
			enemyHealth -= (int)(characterDamage * disadvantageMultiplier);
		}
		//Rock to Scissors
		else if (thisPlayer == Character.rock && enemyPlayer == Character.scissors)
		{
			enemyHealth -= (int)(characterDamage * advantageMultiplier);
		}
		//Paper to Rock
		else if (thisPlayer == Character.paper && enemyPlayer == Character.rock)
		{
			enemyHealth -= (int)(characterDamage * advantageMultiplier);
		}
		//Paper to Scissors
		else if (thisPlayer == Character.paper && enemyPlayer == Character.scissors)
		{
			enemyHealth -= (int)(characterDamage * disadvantageMultiplier);
		}
		else {
			enemyHealth -= characterDamage;
		}

		enemy.GetComponent<Combat>().health = enemyHealth;
		//Debug.Log("Enemy health: " + enemy.GetComponent<Combat>().health);
	}

	public void Die()
	{
		lives -= 1;
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
		health = 100;

		int randSpawn = Random.Range(1, respawnPoints.Length);
		Debug.Log(randSpawn);

		gameObject.transform.position = respawnPoints[randSpawn].gameObject.transform.position;
	}
}
