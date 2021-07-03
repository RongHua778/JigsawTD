using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(menuName = "Factory/BlueprintFactory", fileName = "blueprintFactory")]
public class BlueprintFactory : GameObjectFactory
{
    public Blueprint GetRandomBluePrint(TurretAttribute attribute)
    {
        Blueprint blueprint = new Blueprint();
        blueprint.CompositeTurretAttribute = attribute;
        int[] compositionLevel = StaticData.GetSomeRandoms(attribute.totalLevel, attribute.elementNumber);
        for (int i = 0; i < attribute.elementNumber; i++)
        {
            Composition c = new Composition(compositionLevel[i], Random.Range(0, StaticData.elementN));
            blueprint.Compositions.Add(c);
        }
        blueprint.SetBluePrintIntensify();
        StrategyComposite strategy = new StrategyComposite(attribute, blueprint, 1, null);
        strategy.SetQualityValue();
        blueprint.ComStrategy = strategy;
        return blueprint;
    }


}
