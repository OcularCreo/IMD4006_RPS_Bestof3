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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            StartCoroutine(movePlatform(trexPlatform, trexPlatform_mouthOpen, 30, -1));
        }
        if (Input.GetKeyDown("g"))
        {
            StartCoroutine(movePlatform(trexPlatform_mouthOpen, trexPlatform, 30, 1));
        }
    }

    private IEnumerator movePlatform(GameObject platform1, GameObject platform2, int rotateAmount, int rotateDirection)
    {
        for (int i = 0; i < rotateAmount; i++)
        {
            platform1.transform.Rotate(0, 0, rotateDirection, Space.Self);
            yield return new WaitForSeconds(0.01f);
        }
        platform1.GetComponent<Renderer>().enabled = false;
        platform2.GetComponent<Renderer>().enabled = true;
    }
}
