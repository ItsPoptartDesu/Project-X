using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePiece : MonoBehaviour
{
    [SerializeField]
    private ESlimePart eSlimePart;
    protected SpriteRenderer myRenderer;
    private SO_SlimePart basePart;
    public float PowerModifier = 0;
    public int CostModifier = 0;

    public void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBasePart(SO_SlimePart _soSP)
    {
        basePart = _soSP;
    }
    public bool CompareSlimePart(ESlimePart _other) { return (eSlimePart == _other) ? true : false; }
    public ESlimePart GetESlimePart() { return eSlimePart; }
    public ESlimePart GetSlimePart() { return basePart.SlimePart; }
    public ESlimeType GetSlimeType() { return basePart.SlimeType; }
    public string GetSlimePartName() { return basePart.PartName; }
    public Sprite GetSlimeSprite() { return basePart.ImgToDisplay; }
    public float GetPower() { return basePart.Power + PowerModifier; }
    public float GetCost() { return basePart.Cost + CostModifier; }
    public void UpdateSlimePart(SO_SlimePart _part)
    {
        basePart = _part;
        RefreshRenderer();
    }
    public void RefreshRenderer()
    {
        myRenderer.sprite = basePart.ImgToDisplay;
    }
}
