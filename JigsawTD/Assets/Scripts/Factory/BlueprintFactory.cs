using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Factory/BlueprintFactory", fileName = "blueprintFactory")]
public class BlueprintFactory : GameObjectFactory
{
    public Blueprint GetComposedTurret(int compositionN, int totalLevel)
    {
        Blueprint blueprint = new Blueprint();
        blueprint.TotalLevel = totalLevel;
        blueprint.CompositionN = compositionN;
        int[] compositionLevel = StaticData.GetSomeRandoms(totalLevel, compositionN);
        for (int i = 0; i < compositionN; i++)
        {
            Composition c = new Composition(compositionLevel[i], Random.Range(0, StaticData.elementN));
            blueprint.Compositions.Add(c);
        }
        return blueprint;
    }

    public List<Blueprint> GetBluePrints(int bluePrintN)
    {
        List<Blueprint> result = new List<Blueprint>();
        for (int i = 0; i < bluePrintN; i++)
        {
            result.Add(GetComposedTurret(2, 1));
        }
        return result;
    }
}
