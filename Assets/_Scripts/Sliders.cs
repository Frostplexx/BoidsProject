using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    // Sliders here

    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider hungerSlider;

    public void Start()
    {
        healthSlider.value = 100;
        staminaSlider.value = 100;
        hungerSlider.value = 100;
    }

    // Setting sliders
    public void SetHealth(int health)
    {
        healthSlider.value = health;
        Debug.Log("adadad");
    }

    public void SetStamina(int stamina)
    {
        staminaSlider.value = stamina;
    }

    public void SetHunger(int hunger)
    {
        hungerSlider.value = hunger;
    }
}
