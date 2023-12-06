using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class backgroundScroller : MonoBehaviour
{
    private Transform camTransform;   //variable used to track the position of the main camera
    private Transform bgTransform;    //variable used to set the position of the background

    private Vector3 initialScale;
    private Renderer bgRenderer;

    private float initialCamSize;
    private Vector3 initialBgScale;

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        bgTransform = gameObject.GetComponent<Transform>();

        bgRenderer = gameObject.GetComponent<Renderer>();
        initialScale = bgRenderer.material.mainTextureScale;

        initialCamSize = Camera.main.orthographicSize;
        initialBgScale = bgTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //have the titling backgrond's material offset be set to the position of the camera
        bgRenderer.material.mainTextureOffset = new Vector2 (camTransform.position.x, camTransform.position.y);

        //move and scale the background plane relative to the camera
        bgTransform.position = new Vector2(camTransform.position.x, camTransform.position.y);

        //if the camera size changes
        if (initialCamSize != Camera.main.orthographicSize )
        {

            float scaleRatio = Camera.main.orthographicSize / initialCamSize;

            //bgTransform.localScale = new Vector3(initialBgScale.x * scaleRatio, initialBgScale.y * scaleRatio, initialBgScale.z * scaleRatio);

            //bgRenderer.material.mainTextureScale = new Vector2(initialScale.x * scaleRatio, initialScale.y * scaleRatio);

        }
        
    }

}
