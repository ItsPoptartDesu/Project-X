using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardHelper : MonoBehaviour
{
    public TextMeshProUGUI CardName;
    public TextMeshProUGUI CardDescription;
    public TextMeshProUGUI CardAttack;
    public TextMeshProUGUI CardCost;
    public Image img;
    public GameObject CardFrontRoot;
    public GameObject CardBackRoot;
    public virtual void ToggleDisplayRoot(bool _on) { CardFrontRoot.SetActive(_on); }
    public virtual void ToggleCardBackRoot(bool _on) { CardBackRoot.SetActive(_on); }
}
