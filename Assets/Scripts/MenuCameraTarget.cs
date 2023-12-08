using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraTarget : MonoBehaviour
{
    public GameObject player1;
	public GameObject player2;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player1 != null && player2 != null) 
        {
			transform.position = new Vector2((player1.transform.position.x + player2.transform.position.x)/2f, transform.position.y);
        }
    }
}
