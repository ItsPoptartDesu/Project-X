using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trainer" , menuName = "TrainerStatus")]
public class TrainerStatus : ScriptableObject
{
    [SerializeField] public string TrainerName;
    [SerializeField] public bool HasBeenBattled;
    [SerializeField] public SlimeTeamInfo ActiveTeam;
    public TrainerStatus(string _name)
    {
        ActiveTeam = new SlimeTeamInfo();
        TrainerName = _name;
        HasBeenBattled = false;
    }
}
