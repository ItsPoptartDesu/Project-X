using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GeneAllele
{
    D,
    R1,
    R2,
}
[Serializable]
public class Gene
{
    public GeneAllele Allele;
    public CardComponentType Part;

    private static readonly Dictionary<GeneAllele , float> genePercentages = new Dictionary<GeneAllele , float>()
    {
        { GeneAllele.D, 0.375f },
        { GeneAllele.R1, 0.09375f },
        { GeneAllele.R2, 0.03125f },
    };

    public float GetChildPercentage()
    {
        return genePercentages[Allele];
    }
}



public class BreedingManager : MonoBehaviour
{
    public void Breed(Slime _mom , Slime _dad)
    {
        //Dictionary<CardComponentType , float> map = new Dictionary<CardComponentType , float>();
        //foreach(var s in _mom.GetActiveParts())
        //{
        //    foreach(Gene g in s.GetAlleles())
        //    {
        //        if (!map.ContainsKey(g.Part.CardComponentType))
        //            map.Add(g.Part.CardComponentType ,g.GetChildPercentage());
        //        else
        //            map[g.Part.CardComponentType] += g.GetChildPercentage();
        //    }
        //}
        //foreach (var s in _dad.GetActiveParts())
        //{
        //    foreach (Gene g in s.GetAlleles())
        //    {
        //        if (!map.ContainsKey(g.Part.CardComponentType))
        //            map.Add(g.Part.CardComponentType , g.GetChildPercentage());
        //        else
        //            map[g.Part.CardComponentType] += g.GetChildPercentage();
        //    }
        //}
        //float f = UnityEngine.Random.Range(0f , 1f);
        //var sortedMap = map.OrderByDescending(x => x.Value).ToList();
        //foreach (var s in sortedMap)
        //{
        //    if (f <= s.Value)
        //    {
        //        Debug.Log("Selected Skill: " + s.Key);
        //        break;
        //    }
        //}
    }
}
