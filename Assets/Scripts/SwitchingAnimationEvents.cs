using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SwitchingAnimationEvents : MonoBehaviour
{
    [SerializeField] private RPS_Switching rpsSwitching;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // animation event for the end of the animation
    private void changeCharacterAnimationEnd()
    {
        rpsSwitching.swapSprites(true, rpsSwitching.character); // go back to the idle sprite
        rpsSwitching.changeCharacter(); // change character in rps switching
        rpsSwitching.swapSprites(true, rpsSwitching.character); //change sprite to new character and idle
        rpsSwitching.animator.SetBool("Switching", false); // stop the switching animation
    }
}
