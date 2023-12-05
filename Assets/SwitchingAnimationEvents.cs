using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingAnimationEvents : MonoBehaviour
{
    [SerializeField] private RPS_Switching rpsSwitching;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("RPS CHARACTER: " + rpsSwitching.character);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // animation event for the end of the animation
    private void changeCharacterAnimationEnd()
    {
        
        rpsSwitching.changeCharacter();
        rpsSwitching.swapSprites(true, rpsSwitching.character); //change sprite to new character and idle
        rpsSwitching.animator.SetBool("Switching", false);
        //Debug.Log("CHANGED");
    }
}
