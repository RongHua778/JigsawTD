using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˹���ֻ����turret�ĺ�̨�߼���quality��element���ϳ������䷽�ȵȡ�������sprite��Ⱦ��tileλ�÷��õ�
[CreateAssetMenu(menuName = "Factory/TurretFactory", fileName = "TurretFactory")]
public class TurretAttributeFactory : GameObjectFactory
{
    [Header("������")]
    public List<TurretAttribute> CompositionAttributes;
    public List<TurretAttribute> ElementAttributes;

    private List<TurretAttribute> Rare1Turrets;
    private List<TurretAttribute> Rare2Turrets;
    private List<TurretAttribute> Rare3Turrets;
    private Dictionary<Element, TurretAttribute> ElementDIC;
    public void InitializeFacotory()
    {
        Rare1Turrets = new List<TurretAttribute>();
        Rare2Turrets = new List<TurretAttribute>();
        Rare3Turrets = new List<TurretAttribute>();
        ElementDIC = new Dictionary<Element, TurretAttribute>();

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
        Debug.LogWarning(element + "û�ж�Ӧ��Ԫ��attribute");
        return null;
    }

    public TurretAttribute GetRandomCompositionTurret()
    {
        return CompositionAttributes[Random.Range(0, CompositionAttributes.Count)];
    }

    public TurretAttribute GetRandomCompositionTurretByLevel()
    {
        int level = LevelUIManager.Instance.PlayerLevel;
        float[] rare = new float[3];
        for (int i = 0; i < 3; i++)
        {
            rare[i] = StaticData.Instance.RareChances[level - 1, i];
        }
        int final = StaticData.RandomNumber(rare);
        switch (final)
        {
            case 0:
                return Rare1Turrets[Random.Range(0, Rare1Turrets.Count)];
            case 1:
                return Rare2Turrets[Random.Range(0, Rare2Turrets.Count)];
            case 2:
                return Rare3Turrets[Random.Range(0, Rare3Turrets.Count)];
            default:
                Debug.Log("����ĸ���");
                return null;
        }
    }

    //���ڲ���
    public TurretAttribute TestGetCompositeByName(string name)
    {
        foreach (TurretAttribute attribute in CompositionAttributes)
        {
            if (attribute.Name == name)
            {
                return attribute;
            }
        }
        Debug.LogWarning("�����˴��������");
        return null;
    }

}
