using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class HealthBar : MonoBehaviour
{
    public TextMeshProUGUI HealthText;
    public Slider healthBar;
    public TextMeshProUGUI ShieldText;
    public Transform StatusAttachPoint;

    //TODO could probably do some sort of cache system so i dont need to Instantiate
    //unless i hit a limit.
    public void AddStatusEffectIcon(StatusEffect _ToBeAdded)
    {
        GameObject icon = ObjectManager.Instance.GenerateStatusEffectIcon(_ToBeAdded);
        icon.transform.SetParent(StatusAttachPoint , false);
    }
    public void SetStats(int _health , int _shields)
    {
        healthBar.maxValue = _health;
        healthBar.value = _health;
        healthBar.minValue = 0;
        HealthText.text = _health.ToString();
        ShieldText.text = _shields.ToString();
    }
    public void SetHealth(Vector2 _HealthnShields)
    {
        healthBar.value = _HealthnShields.x;
        HealthText.text = _HealthnShields.x.ToString();
        ShieldText.text = _HealthnShields.y.ToString();
    }
    public void ToggleHealthBar(bool _on)
    {
        healthBar.gameObject.SetActive(_on);
    }
    public void Cleanse()
    {
        int childCount = StatusAttachPoint.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform t = StatusAttachPoint.GetChild(i);
            Destroy(t.gameObject);
        }
    }
}
