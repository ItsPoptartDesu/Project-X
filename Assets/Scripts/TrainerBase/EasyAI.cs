using System.Collections.Generic;
using System.Linq;
public class EasyAI : BaseAI
{
    public EasyAI() : base()
    {

    }
    public override CardDisplay MakeDecision()
    {
        CardDisplay selectedCard = Hand.Where(x => x.rawCardStats.GetCost() <= ManaDisplayReference.GetCurrentMana()).FirstOrDefault();
        return selectedCard;
    }
    /*
        while (currentState != GameState.GameOver)
        {
            switch (currentState)
            {
                case GameState.PlayerTurn:
                    PlayerTurn();
                    break;
                case GameState.AITurn:
                    AITurn();
                    break;
            }
        }
     */
}
