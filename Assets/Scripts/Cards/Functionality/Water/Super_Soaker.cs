using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Super_Soaker : CardDisplay
{
    public override bool OnPlay(List<Slime> _activeTeam)
    {
        if (UnityEngine.Random.value > rawCardStats.GetHost().GetAccuracy(rawCardStats.GetAccuracy()))
            return false;

        if (_activeTeam.Count == 1)
        {
            _activeTeam[0].ApplyDamage(rawCardStats);
            return true;
        }

        int index = UnityEngine.Random.Range(0 , _activeTeam.Count);
        int direction = 0;
        if (index == 0)
            direction = 1;
        else if (index == _activeTeam.Count - 1)
            direction = -1;
        else
            direction = UnityEngine.Random.Range(0,2) == 0 ? 1 : -1;

        _activeTeam[index].ApplyDamage(rawCardStats);

        index += direction;
        if (index >= 0 && index < _activeTeam.Count)
            _activeTeam[index].ApplyDamage(rawCardStats);

        return true;
    }


}
