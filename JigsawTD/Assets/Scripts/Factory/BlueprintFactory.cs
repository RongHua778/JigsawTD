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
        strategy.GetTurretSkills();
        blueprint.ComStrategy = strategy;
        return blueprint;
    }

    public Blueprint GetSpecificBluePrint(TurretAttribute attribute,int element1,int element2,int element3)
    {
        Blueprint blueprint = new Blueprint();
        blueprint.CompositeTurretAttribute = attribute;
        Composition c1 = new Composition(1, element1);
        Composition c2 = new Composition(1, element2);
        Composition c3 = new Composition(1, element3);
        blueprint.Compositions.Add(c1);
        blueprint.Compositions.Add(c2);
        blueprint.Compositions.Add(c3);
        blueprint.SetBluePrintIntensify();
        StrategyComposite strategy = new StrategyComposite(attribute, blueprint, 1, null);
        strategy.SetQualityValue();
        strategy.GetTurretSkills();
        blueprint.ComStrategy = strategy;
        return blueprint;
    }


}
