using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePiece : MonoBehaviour
{
    [SerializeField]
    private ESlimePart eSlimePart;
    protected SpriteRenderer myRenderer;
    private SO_SlimePart basePart;
    private Slime Host;
    public CardComponentType GetCardType() { return basePart.CardComponentType; }
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
    public Sprite GetSlimeSprite() { return basePart.SlimeArt; }
    public Sprite GetCardArt() { return basePart.CardArt; }
    public int GetPower() { return basePart.Power; }
    public int GetCost() { return basePart.Cost; }
    public float GetStatusEffectProbability() { return basePart.StatusEffectProbability; }
    public void SetHost(Slime _s) { Host = _s; }
    public Slime GetHost() { return Host; }
    public void UpdateSlimePart(SO_SlimePart _part)
    {
        basePart = _part;
        RefreshRenderer();
    }
    public void RefreshRenderer()
    {
        if (basePart.SlimeArt == null)
            return;
        myRenderer.sprite = basePart.SlimeArt;
    }
    public void ToggleRenderer()
    {
        myRenderer.enabled = !myRenderer.enabled;
    }
}
