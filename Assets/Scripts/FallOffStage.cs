using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffStage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("Check in");
		if (collision.gameObject.GetComponent<Combat>() != null && collision.gameObject.GetComponent<Combat>().hasRespawned == true)
		{
            //collision.gameObject.GetComponent<Combat>().hasRespawned = false;
            //collision.gameObject.GetComponent<Combat>().Die();
            collision.gameObject.GetComponent<Combat>().health = 0;
            //Debug.Log("Call");

        }
	}
}
