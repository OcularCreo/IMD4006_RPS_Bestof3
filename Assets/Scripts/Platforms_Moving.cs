using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms_Moving : MonoBehaviour
{
    GameObject trexPlatform;
    GameObject trexPlatform_mouthOpen;

    // Start is called before the first frame update
    void Start()
    {
        trexPlatform = GameObject.Find("trex");
        trexPlatform_mouthOpen = GameObject.Find("trex mouth open");
        trexPlatform_mouthOpen.SetActive(false); // set second platform to inactive at start
    }

    // Update is called once per frame
    void Update()
    {
        // if press down t on keyboard, play animation and change to trex mouth open
        if (Input.GetKeyDown("t"))
        {
            StartCoroutine(movePlatform(trexPlatform, trexPlatform_mouthOpen, 30, -1));
        }
        // if press down g, play reverse animation and change back to trex
        if (Input.GetKeyDown("g"))
        {
            StartCoroutine(movePlatform(trexPlatform_mouthOpen, trexPlatform, 30, 1));
        }
    }

    // function to rotate the platform, then switch to the other image of the platform
    private IEnumerator movePlatform(GameObject platform1, GameObject platform2, int rotateAmount, int rotateDirection)
    {
        for (int i = 0; i < rotateAmount; i++)
        {
            platform1.transform.Rotate(0, 0, rotateDirection, Space.Self); // rotate visible platform
            yield return new WaitForSeconds(0.01f);
        }
        platform1.SetActive(false); // set visible platform to inactive
        platform1.transform.Rotate(0, 0, -(rotateDirection) * rotateAmount, Space.Self); // set platform to initial position
        platform2.SetActive(true); // set the other (invisible) platform to active
    }
}
