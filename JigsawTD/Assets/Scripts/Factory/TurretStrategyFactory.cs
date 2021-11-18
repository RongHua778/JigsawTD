using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(menuName = "Factory/BlueprintFactory", fileName = "blueprintFactory")]
public class TurretStrategyFactory : GameObjectFactory
{
    //new
    public RefactorStrategy GetRandomRefactorStrategy(TurretAttribute attribute)
    {
        int[] compositionLevel = StaticData.GetSomeRandoms(attribute.maxElementLevel, attribute.totalLevel, attribute.elementNumber);
        List<Composition> compositions = new List<Composition>();
        for (int i = 0; i < attribute.elementNumber; i++)
        {
            int element = (int)StaticData.Instance.ContentFactory.GetRandomElementAttribute().element;
            Composition c = new Composition(compositionLevel[i], element);
            compositions.Add(c);
        }
        RefactorStrategy strategy = new RefactorStrategy(attribute, 1, compositions);
        return strategy;
    }

    public RefactorStrategy GetSpecificRefactorStrategy(TurretAttribute attribute, int element1, int element2, int element3, int quality = 1)
    {
        List<Composition> compositions = new List<Composition>();
        Composition c1 = new Composition(1, element1);
        Composition c2 = new Composition(1, element2);
        Composition c3 = new Composition(1, element3);
        compositions.Add(c1);
        compositions.Add(c2);
        compositions.Add(c3);
        RefactorStrategy strategy = new RefactorStrategy(attribute, quality, compositions);
        return strategy;
    }


}
