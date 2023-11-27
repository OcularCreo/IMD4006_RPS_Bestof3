using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Slider timeSlider;
    

    public void setTime(float timeLeft){
        
        timeSlider.value = timeLeft;
        
    }
    public void setMaxTime(float maxTime){
        
        timeSlider.maxValue = maxTime;
    }

}
