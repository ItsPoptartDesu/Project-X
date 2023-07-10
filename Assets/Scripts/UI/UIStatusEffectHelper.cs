using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIStatusEffectHelper : MonoBehaviour
{
    [HideInInspector]
    public Sprite StatusEffectIcon { get { return GetComponent<Image>().sprite; } set { GetComponent<Image>().sprite = value; } }

    public void AddStatusIcon(Sprite _effect)
    {
        GetComponent<Image>().sprite = _effect;
    }
}
