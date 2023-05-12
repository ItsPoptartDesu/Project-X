using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trainer" , menuName = "TrainerStatus")]
public class TrainerStatus : ScriptableObject
{
    public string TrainerName;
    public bool hasBeenBattled;
}
