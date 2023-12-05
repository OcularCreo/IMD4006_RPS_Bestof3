using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider timeSlider;

    private float maxTime;
	[SerializeField] private Image timeMeter;


	public void setTime(float timeLeft){
        
        //timeSlider.value = timeLeft;
		timeMeter.fillAmount = timeLeft / maxTime;

	}
    public void setMaxTime(float maxTimeNew){
        
        //timeSlider.maxValue = maxTime;

        maxTime = maxTimeNew;

    }

}
