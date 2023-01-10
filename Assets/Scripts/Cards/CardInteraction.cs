using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedOn = eventData.pointerPress;
        SlimeCard piece = clickedOn.GetComponent<SlimeCard>();
        if (piece != null)
        {
            Debug.Log($"Clicked on {piece.CardName.text}");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
    }

    void FellStingerEnterBattleField()
    {

    }
}
