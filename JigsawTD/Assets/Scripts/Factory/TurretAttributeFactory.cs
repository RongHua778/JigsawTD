using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此工厂只负责turret的后台逻辑如quality，element，合成所需配方等等。不负责sprite渲染和tile位置放置等
[CreateAssetMenu(menuName = "Factory/TurretFactory", fileName = "TurretFactory")]
public class TurretAttributeFactory : GameObjectFactory
{
    [Header("配置区")]
    public List<TurretAttribute> CompositionAttributes;
    public List<TurretAttribute> ElementAttributes;

    private List<TurretAttribute> Rare1Turrets = new List<TurretAttribute>();
    private List<TurretAttribute> Rare2Turrets = new List<TurretAttribute>();
    private List<TurretAttribute> Rare3Turrets = new List<TurretAttribute>();
    private Dictionary<Element, TurretAttribute> ElementDIC = new Dictionary<Element, TurretAttribute>();
    public void InitializeFacotory()
    {
        foreach (TurretAttribute attribute in CompositionAttributes)
        {
            switch (attribute.Rare)
            {
                case 1:
                    Rare1Turrets.Add(attribute);
                    break;
                case 2:
                    Rare2Turrets.Add(attribute);
                    break;
                case 3:
                    Rare3Turrets.Add(attribute);
                    break;
            }
        }
        foreach (var attribute in ElementAttributes)
        {
            ElementDIC.Add(attribute.element, attribute);
        }
    }

    public TurretAttribute GetElementsAttributes(Element element)
    {
        if (ElementDIC.ContainsKey(element))
        {
            return ElementDIC[element];
        }
        Debug.LogWarning(element + "没有对应的元素attribute");
        return null;
    }

    public TurretAttribute GetRandomCompositionTurret()
    {
        return CompositionAttributes[Random.Range(0, CompositionAttributes.Count)];
    }

    public TurretAttribute TestGetCompositeByName(string name)
    {
        foreach (TurretAttribute attribute in CompositionAttributes)
        {
            if (attribute.Name == name)
            {
                return attribute;
            }
        }
        Debug.LogWarning("输入了错误的名字");
        return null;
    }

}
