using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    //set slider maxValue
    public void setMaxHealth(int maxHealth){
        
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    //set slider current value
    public void setHealth(int health){
        slider.value = health;
    }
}
