using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Mana : MonoBehaviour
{
    public Sprite[] img = new Sprite[2];
    [SerializeField]
    Image myImage;

    public void TurnOffMana()
    {
        myImage.sprite = img[1];
    }
    public void TurnOnMana()
    {
        myImage.sprite = img[0];
    }
}
