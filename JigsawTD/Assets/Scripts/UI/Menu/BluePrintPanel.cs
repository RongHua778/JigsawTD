using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrintPanel : MonoBehaviour
{

    [SerializeField] RareSet[] rareSets = default;

    public void Initialize()
    {
        foreach (var rare in rareSets)
        {
            rare.InitializeSlots(this);
        }
    }


    public bool CheckSets()
    {
        foreach (var rare in rareSets)
        {
            if (!rare.CheckSet())
                return false;
        }
        return true;
    }


}
