using System.Collections.Generic;
using System.Linq;
public enum AI_LEVEL
{
    EASY,
    MEDIUM,
    HARD,
}

public abstract class BaseAI
{
    public UI_ManaDisplay ManaDisplayReference;
    public Queue<CardDisplay> Deck = new Queue<CardDisplay>();
    public List<CardDisplay> Hand = new List<CardDisplay>();
    public List<CardDisplay> Discard = new List<CardDisplay>();

    public abstract CardDisplay MakeDecision();
    public int GetCurrentMana () { return ManaDisplayReference.GetCurrentMana (); }
    public bool OutOfMana()
    {
        CardDisplay Card = Hand.Where(x => x.rawCardStats.GetCost() <= ManaDisplayReference.GetCurrentMana()).FirstOrDefault();
        if (Card == null)
            return true;
        return false;
    }
}
