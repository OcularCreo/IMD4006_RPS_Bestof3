using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //[SerializeField] private Slider slider;
    [SerializeField] private Image healthMeter;
	[SerializeField] private GameObject heart1;
	[SerializeField] private GameObject heart2;
	[SerializeField] private GameObject heart3;
	//set slider maxValue
	public void setMaxHealth(int maxHealth){
        
        //slider.maxValue = maxHealth;
        //slider.value = maxHealth;


    }

    //set slider current value
    public void setHealth(int health){
        //slider.value = health;
        healthMeter.fillAmount = (float)health/100;
        Debug.Log((float)health / 100);

	}

    public void loseLife(int livesLeft) {
		if (livesLeft < 3)
		{
			heart1.SetActive(false);

		}
		if (livesLeft < 2) 
        {
			heart2.SetActive(false);

		}
        if (livesLeft < 1)
        {
			heart3.SetActive(false);
		}
    }
}
