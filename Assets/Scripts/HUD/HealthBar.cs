using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void ChangeMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }

    public void ChangeCurrentHealth(float currentHealth)
    {
        slider.value = currentHealth;
    }

    public void Initialize(float currentHealth)
    {
        ChangeMaxHealth(currentHealth);
        ChangeCurrentHealth(currentHealth);
    }
}
