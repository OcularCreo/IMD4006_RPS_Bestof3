using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcons : MonoBehaviour
{
    [SerializeField] private GameObject vulnerableIcon;
    [SerializeField] private GameObject lowLivesIcon;
    [SerializeField] private GameObject loeHealthIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableVulnerableIcon() {
        vulnerableIcon.SetActive(true);
    }

    public void EnableLowLivesIcon()
    {
        lowLivesIcon.SetActive(true);
    }
    public void EnableLowHealthIcon()
    {
        loeHealthIcon.SetActive(true);
    }

    public void DisableVulnerableIcon()
    {
        vulnerableIcon.SetActive(false);
    }

    public void DisableLowLivesIcon()
    {
        lowLivesIcon.SetActive(false);
    }
    public void DisableLowHealthIcon()
    {
        loeHealthIcon.SetActive(false);
    }
}
