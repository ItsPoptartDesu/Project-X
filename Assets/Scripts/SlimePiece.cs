using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePiece : MonoBehaviour
{
    protected SpriteRenderer myRenderer;
    public Sprite ToBeRendered
    {
        get { return myRenderer.sprite; }
        set { myRenderer.sprite = value; }
    }
    public Slime_Part whichPart;
    public string SlimePartName { get { return myRenderer.sprite.name; } }
    public void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }
}
