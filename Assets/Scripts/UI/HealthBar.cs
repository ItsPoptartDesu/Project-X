using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI HealthText;
    public Slider healthBar;
    public void SetMaxHealth(int _health)
    {
        healthBar.maxValue = _health;
        healthBar.value = _health;
        healthBar.minValue = 0;
        HealthText.text = _health.ToString();
    }
    public void SetHealth(int _health)
    {
        healthBar.value = _health;
        HealthText.text = _health.ToString();
    }
    public void ToggleHealthBar(bool _on)
    {
        healthBar.gameObject.SetActive(_on);
    }
}
