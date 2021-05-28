using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(menuName = "Factory/BlueprintFactory", fileName = "blueprintFactory")]
public class BlueprintFactory : GameObjectFactory
{
    private List<TurretAttribute> compositeTurrets = new List<TurretAttribute>();

    public void InitializeFactory()
    {
        compositeTurrets = StaticData.Instance.CompositionAttributes.ToList();
    }

    public Blueprint GetRandomBluePrint()
    {
        return GetComposedTurret(compositeTurrets[Random.Range(0, compositeTurrets.Count)]);
    }

    public Blueprint GetComposedTurret(TurretAttribute attribute)
    {
        Blueprint blueprint = new Blueprint();
        blueprint.CompositeTurretAttribute = attribute;
        int[] compositionLevel = StaticData.GetSomeRandoms(attribute.totalLevel, attribute.elementNumber);
        for (int i = 0; i < attribute.elementNumber; i++)
        {
            Composition c = new Composition(compositionLevel[i], Random.Range(0, StaticData.elementN));
            blueprint.Compositions.Add(c);
        }
        blueprint.SetCompositeValues();
        return blueprint;
    }


}
