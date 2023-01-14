using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public void SetMaxHealth(int _health)
    {
        healthBar.maxValue = _health;
        healthBar.value = _health;
        healthBar.minValue = 0;
    }
    public void SetHealth(int _health)
    {
        healthBar.value = _health;
    }
    public void ToggleHealthBar(bool _on)
    {
        healthBar.gameObject.SetActive(_on);
    }
}
