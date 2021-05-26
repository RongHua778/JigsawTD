using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Factory/BlueprintFactory", fileName = "blueprintFactory")]
public class BlueprintFactory : GameObjectFactory
{
    public Blueprint GetComposedTurret(TurretAttribute attribute)
    {
        Blueprint blueprint = new Blueprint();
        blueprint.CompositeTurretAttribute = attribute;
        //blueprint.TotalLevel = attribute.totalLevel;
        //blueprint.CompositionN = attribute.elementNumber;
        int[] compositionLevel = StaticData.GetSomeRandoms(attribute.totalLevel, attribute.elementNumber);
        for (int i = 0; i < attribute.elementNumber; i++)
        {
            Composition c = new Composition(compositionLevel[i], Random.Range(0, StaticData.elementN));
            blueprint.Compositions.Add(c);
        }
        return blueprint;
    }

    public List<Blueprint> GetBluePrints(int bluePrintN)
    {
        List<Blueprint> result = new List<Blueprint>();
        //for (int i = 0; i < bluePrintN; i++)
        //{
        //    result.Add(GetComposedTurret(2, 1));
        //}
        return result;
    }
}
