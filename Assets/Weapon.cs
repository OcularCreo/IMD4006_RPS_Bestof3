using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject weaponGFX;
	[SerializeField] public GameObject weaponPivot;
	[SerializeField] private GameObject playerCharacter;

	private bool facingRight = true;


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		CheckForFlip();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("Check in");
		if (collision.gameObject.GetComponent<Combat>() != null)
		{
			playerCharacter.GetComponent<Combat>().CanHitEnterRange();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		//Debug.Log("Check out");
		if (collision.gameObject.GetComponent<Combat>() != null)
		{
			playerCharacter.GetComponent<Combat>().CanHitExitRange();
		}
	}

	private void CheckForFlip() {

		if (playerCharacter.GetComponent<RPS_Switching>().player == Player.P1)
		{
			if (Input.GetKeyDown("a") && facingRight)
			{
				Flip();
				facingRight = false;
			}

			if (Input.GetKeyDown("d") && !facingRight)
			{
				Flip();
				facingRight = true;
			}
		}

		if (playerCharacter.GetComponent<RPS_Switching>().player == Player.P2)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow) && facingRight)
			{
				Flip();
				facingRight = false;
			}

			if (Input.GetKeyDown(KeyCode.RightArrow) && !facingRight)
			{
				Flip();
				facingRight = true;
			}
		}
		
	}

	private void Flip() {
		//Debug.Log(transform.localPosition);
		transform.localPosition = new Vector3(-transform.localPosition.x, 0, 0);
		transform.Rotate(0, 180, 0);
	}

	

}
