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
        int[] compositionLevel = StaticData.GetSomeRandoms(attribute.maxElementLevel, attribute.totalLevel, attribute.elementNumber);
        List<Composition> compositions = new List<Composition>();
        for (int i = 0; i < attribute.elementNumber; i++)
        {
            int element = (int)StaticData.Instance.ContentFactory.GetRandomElementAttribute().element;
            Composition c = new Composition(compositionLevel[i], element);
            compositions.Add(c);
        }
        blueprint.SortBluePrint(compositions, 1);
        return blueprint;
    }


    public Blueprint GetSpecificBluePrint(TurretAttribute attribute, int element1, int element2, int element3, int quality = 1)
    {
        Blueprint blueprint = new Blueprint();
        blueprint.CompositeTurretAttribute = attribute;
        List<Composition> compositions = new List<Composition>();

        Composition c1 = new Composition(1, element1);
        Composition c2 = new Composition(1, element2);
        Composition c3 = new Composition(1, element3);
        compositions.Add(c1);
        compositions.Add(c2);
        compositions.Add(c3);
        blueprint.SortBluePrint(compositions, quality);

        //CompositeBlueprint(attribute, blueprint);
        return blueprint;
    }

    //private void CompositeBlueprint(TurretAttribute attribute, Blueprint blueprint)
    //{
    //    blueprint.ComStrategy = new StrategyBase(attribute, 1);
    //    blueprint.ComStrategy.SetQualityValue();//属性设置
    //    blueprint.ComStrategy.GetTurretSkills();//初始技能

    //    //配方自带元素技能
    //    List<int> elements = new List<int>();
    //    foreach (var com in blueprint.Compositions)
    //    {
    //        elements.Add(com.elementRequirement);
    //    }
    //    ElementSkill effect = TurretEffectFactory.GetElementSkill(elements);
    //    blueprint.ComStrategy.AddElementSkill(effect);

    //}
}
