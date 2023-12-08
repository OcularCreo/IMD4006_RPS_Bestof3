using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms_Moving : MonoBehaviour
{
    // platforms
    GameObject trexPlatform;
    GameObject trexPlatform_mouthOpen;
    GameObject crocPlatform;
    GameObject handsPlatform;

    private bool changed;
    private bool crocChanged;
    private bool handsChanged;

    [SerializeField] private Manager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        trexPlatform = GameObject.Find("trex");
        trexPlatform_mouthOpen = GameObject.Find("trex mouth open");
        trexPlatform_mouthOpen.SetActive(false); // set second platform to inactive at start

        crocPlatform = GameObject.Find("croc");

        handsPlatform = GameObject.Find("dino in hands");

        changed = false;
        crocChanged = false;
        handsChanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        // TREX
        if (!changed)
        {
            // trex animation once every 25 seconds, and not at the start of time
            if (Mathf.RoundToInt(Time.time) % 25 == 0 && Mathf.RoundToInt(Time.time) > 0)
            {
                StartCoroutine(rotatePlatformFull(trexPlatform, trexPlatform_mouthOpen, 30, -1));
                changed = true;
            }
        }
        else if (Mathf.RoundToInt(Time.time) % 25 == 1)
        {
            // 1 second later, set changed back to false
            changed = false;
        }

        // CROC
        if (!crocChanged)
        {
            // once every 35 seconds, croc animation
            if (Mathf.RoundToInt(Time.time) % 35 == 0 && Mathf.RoundToInt(Time.time) > 0)
            {
                StartCoroutine(translateHorizontalPlatform(crocPlatform, 50));
                crocChanged = true;
            }
        }
        else if (Mathf.RoundToInt(Time.time) % 35 == 1)
        {
            // 1 second later, set changed back to false
            crocChanged = false;
        }

        // HANDS
        // hands platform animation once every 40 seconds, and not at the start of time
        if (Mathf.RoundToInt(Time.time) % 40 == 0 && Mathf.RoundToInt(Time.time) > 0 && !handsChanged)
        {
            StartCoroutine(rotateHands(handsPlatform, 40, 1));
            handsChanged = true;
        }
        else if (Mathf.RoundToInt(Time.time) % 40 == 5 && Mathf.RoundToInt(Time.time) > 0 && handsChanged)
        {
            StartCoroutine(rotateHands(handsPlatform, 40, -1));
            handsChanged = false;
        }
    }

    // function to rotate the platform, then switch to the other image of the platform
    private IEnumerator rotatePlatform(GameObject platform1, GameObject platform2, int rotateAmount, int rotateDirection)
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

    private IEnumerator rotatePlatformFull(GameObject platform1, GameObject platform2, int rotateAmount, int rotateDirection)
    {
        StartCoroutine(rotatePlatform(platform1, platform2, rotateAmount, rotateDirection));
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(rotatePlatform(platform2, platform1, rotateAmount, -(rotateDirection)));
    }

    // horizontal platform movement
    private IEnumerator translateHorizontalPlatform(GameObject platform, int translateAmount)
    {
        // move slowly to the left
        for(int i = 0; i < translateAmount*5; i++)
        {
            platform.transform.Translate(-0.02f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        // move fast to the right
        for (int i = 0; i < translateAmount; i++)
        {
            platform.transform.Translate(0.1f, 0, 0);
            yield return new WaitForSeconds(0.001f);
        }
    }

    // rotate platform for hands platform
    private IEnumerator rotateHands(GameObject platform, int rotateAmount, int rotateDirection)
    {
        for (int i = 0; i < rotateAmount; i++)
        {
            platform.transform.Rotate(0, 0, rotateDirection, Space.Self); // rotate visible platform
            yield return new WaitForSeconds(0.01f);
        }
    }
}
