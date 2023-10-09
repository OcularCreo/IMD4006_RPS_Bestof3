using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject weaponGFX;
	[SerializeField] private GameObject weaponPivot;
	[SerializeField] private GameObject playerCharacter;

    private bool canHit = false;
	private bool facingRight = true;
	private bool hitting = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown("a") && facingRight) {
			Flip();
			facingRight = false;
		}

		if (Input.GetKeyDown("d") && !facingRight)
		{
			Flip();
			facingRight = true;
		}

		if (Input.GetKeyDown("e") && !hitting)
		{
			StartCoroutine(StartCooldown());
		}

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("Check");
		if (!canHit && collision != playerCharacter && collision.gameObject.GetComponent<Combat>() != null)
		{
			canHit = true;
			Debug.Log("Entered Range");
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (canHit && collision != playerCharacter && collision.gameObject.GetComponent<Combat>() != null)
		{
			canHit = false;
			Debug.Log("Left Range");
		}
	}

	private void Flip() {
		transform.position = new Vector2(-transform.position.x, transform.position.y);
		transform.Rotate(0, 180, 0);
	}

	private IEnumerator StartCooldown() {
		hitting = true;
		weaponPivot.transform.Rotate(0, 0, -90);
		yield return new WaitForSeconds(playerCharacter.GetComponent<Combat>().attackSpeed);
		weaponPivot.transform.Rotate(0, 0, 90);
		hitting = false;
	}
}
