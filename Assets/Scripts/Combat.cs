using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int characterDamage = 5;
    [SerializeField] private int dmgMultiplier = 1;
	[SerializeField] public float attackSpeed = 0.3f;

	enum characterType {rock, paper, scissors};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
