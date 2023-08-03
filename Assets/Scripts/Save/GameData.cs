using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameData
{
    public List<TrainerStatus> Trainers;
    public GameData()
    {
        ActiveTeam = new List<Slime>();
        Trainers = new List<TrainerStatus>();
    }
    private SaveSlotData LastSave;
    public List<Slime> GetActiveTeam() { return ActiveTeam; }
    private void SetLastSave(SaveSlotData _FromFile)
    {
        LastSave = _FromFile;
    }
    public SaveSlotData GetLastSave() { return LastSave; }
    private List<Slime> ActiveTeam;
    private void AddSlimeToTeam(Slime _slime)
    {
        ActiveTeam.Add(_slime);
    }
    public void SaveSlimes(List<Slime> _activeTeam)
    {
        ActiveTeam.Clear();
        foreach (Slime s in _activeTeam)
            ActiveTeam.Add(s);
    }

    public void TransferData(SaveSlotData _mySaveData)
    {
        SetLastSave(_mySaveData);
        foreach (JsonSlimeInfo s in _mySaveData.ActiveTeam.SavedSlime)
        {
            GameObject mySlime = ObjectManager.Instance.GenerateSlime(s);
            ObjectManager.Instance.GetActivePlayer().AttachToSelf(mySlime.transform);
            Slime slimeComp = mySlime.GetComponent<Slime>();
            foreach (var p in slimeComp.GetActiveParts())
            {
                List<Gene> geneList = s.Genes.Where(x => ObjectManager.Instance.GetSlimePartFromCardType(x.Part) == p.GetESlimePart()).ToList();
                foreach (var g in geneList)
                {
                    p.Genes.Add(g.Allele , g);
                }
            }
            slimeComp.ToggleRenderers();
            AddSlimeToTeam(slimeComp);
        }
    }
}
