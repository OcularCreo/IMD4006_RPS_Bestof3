using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int characterDamage = 5;
    [SerializeField] private int dmgMultiplier = 1;
	[SerializeField] public float attackSpeed = 0.3f;

	[SerializeField] private GameObject weapon;

	private bool canHit = false;
	private bool hitting = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		CheckForHitAnimation();

		if (hitting == true && canHit == true) {
			//Figure out how to hit enemy
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

		if (Input.GetKeyDown("e") && !hitting)
		{
			StartCoroutine(StartCooldown());
		}
	}

	private IEnumerator StartCooldown()
	{
		hitting = true;
		weapon.GetComponent<Weapon>().weaponPivot.transform.Rotate(0, 0, -90);
		yield return new WaitForSeconds(attackSpeed);
		weapon.GetComponent<Weapon>().weaponPivot.transform.Rotate(0, 0, 90);
		hitting = false;
	}
}
