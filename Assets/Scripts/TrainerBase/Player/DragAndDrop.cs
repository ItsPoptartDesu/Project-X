using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
{
    private Vector2 startPos;
    private Transform card;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        card = transform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(card as RectTransform, eventData.position, eventData.pressEventCamera, out startPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        Vector2 pointerPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(card as RectTransform, eventData.position, eventData.pressEventCamera, out pointerPos);
        card.localPosition = pointerPos - startPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        card = null;
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        GameObject clickedOn = eventData.pointerPress;
        SlimeCard card = clickedOn.GetComponent<SlimeCard>();
        ((NPC_BattleSystem)LevelManager.Instance.currentLevel).AddCardToActionQueue(card);
        if (card != null)
        {
            Debug.Log($"Clicked on {card.CardName.text}");
        }
    }
}