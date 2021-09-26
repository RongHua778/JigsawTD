using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/ContentFactory", fileName = "GameContentFactory")]
public class TileContentFactory : GameObjectFactory
{
    [SerializeField] ContentAttribute emptyAtt = default;
    [SerializeField] ContentAttribute spawnPointAtt = default;
    [SerializeField] ContentAttribute destinationAtt = default;

    [SerializeField]
    TurretAttribute[] elementTurrets = default;
    [SerializeField]
    TurretAttribute[] compositeTurrets = default;
    [SerializeField]
    List<TrapAttribute> trapAtts = default;
    [SerializeField]
    List<TurretBaseAttribute> turretBaseAtts = default;

    private List<TurretAttribute> BattleElements;
    private List<TurretAttribute> BattleComposites;

    private Dictionary<Element, TurretAttribute> ElementDIC;
    private Dictionary<string, TurretAttribute> CompositeDIC;
    private Dictionary<string, TrapAttribute> TrapDIC;
    private Dictionary<string, TurretBaseAttribute> TurretBaseDIC;

    public void Initialize()
    {

        ElementDIC = new Dictionary<Element, TurretAttribute>();
        TrapDIC = new Dictionary<string, TrapAttribute>();
        CompositeDIC = new Dictionary<string, TurretAttribute>();
        TurretBaseDIC = new Dictionary<string, TurretBaseAttribute>();

        BattleElements = new List<TurretAttribute>();
        BattleComposites = new List<TurretAttribute>();

        foreach (TurretAttribute attribute in compositeTurrets)
        {
            CompositeDIC.Add(attribute.Name, attribute);

        }
        foreach (var attribute in trapAtts)
        {
            TrapDIC.Add(attribute.Name, attribute);
        }
        foreach (var attribute in turretBaseAtts)
        {
            TurretBaseDIC.Add(attribute.Name, attribute);
        }
        foreach (var att in elementTurrets)
        {
            ElementDIC.Add(att.element, att);
        }
        SetBattleElements();
    }

    private void SetBattleElements()
    {
        foreach (var select in Game.Instance.SaveData.SaveSelectedElement)
        {
            if (select.isSelect)
            {
                BattleElements.Add(ElementDIC[(Element)Enum.Parse(typeof(Element), select.turretName)]);
            }
        }
    }


    //*******终点
    public GameTileContent GetDestinationPoint()
    {
        TrapContent content = Get(destinationAtt.ContentPrefab) as TrapContent;
        content.TrapAttribute = destinationAtt as TrapAttribute;
        return content;
    }
    //*******起点
    public GameTileContent GetSpawnPoint()
    {
        TrapContent content = Get(spawnPointAtt.ContentPrefab) as TrapContent;
        content.TrapAttribute = spawnPointAtt as TrapAttribute;
        return content;
    }

    public GameTileContent GetBasicContent(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Empty:
                return Get(emptyAtt.ContentPrefab);
        }
        Debug.Assert(false, "Unsupported Type" + type);
        return null;
    }

    //元素塔*******************

    public ElementTurret GetRandomElementTurret(int playerLevel)
    {
        float[] qualityC = new float[5];
        for (int i = 0; i < 5; i++)
        {
            qualityC[i] = StaticData.QualityChances[playerLevel - 1, i];
        }
        int quality = StaticData.RandomNumber(qualityC) + 1;
        TurretAttribute attribute = BattleElements[UnityEngine.Random.Range(0, StaticData.elementN)];
        ElementTurret content = Get(attribute.ContentPrefab) as ElementTurret;
        content.Strategy = new StrategyBase(attribute, quality);
        content.Strategy.m_Turret = content;
        content.Strategy.SetQualityValue();
        content.InitializeTurret();
        return content;
    }

    public ElementTurret GetElementTurret(Element element, int quality)
    {
        TurretAttribute attribute = ElementDIC[element];
        ElementTurret content = Get(attribute.ContentPrefab) as ElementTurret;
        content.Strategy = new StrategyBase(attribute, quality);
        content.Strategy.m_Turret = content;
        content.Strategy.SetQualityValue();
        content.InitializeTurret();
        return content;
    }

    public TurretAttribute GetElementAttribute(Element element)
    {
        TurretAttribute attribute = ElementDIC[element];
        return attribute;
    }

    public TurretAttribute GetRandomElementAttribute(bool isBattleElement = true)
    {
        return isBattleElement ?
            BattleElements[UnityEngine.Random.Range(0, BattleElements.Count)] : 
            ElementDIC[(Element)UnityEngine.Random.Range(0, ElementDIC.Count)];
    }
    //合成塔***********
    public CompositeTurret GetCompositeTurret(Blueprint bluePrint)
    {
        CompositeTurret content = Get(bluePrint.CompositeTurretAttribute.ContentPrefab) as CompositeTurret;
        content.Strategy = bluePrint.ComStrategy;
        bluePrint.ComStrategy.m_Turret = content;
        content.InitializeTurret();
        return content;

    }

    public TurretAttribute GetCompositeTurretByName(string name)
    {
        if (CompositeDIC.ContainsKey(name))
        {
            return CompositeDIC[name];
        }
        Debug.LogWarning("没有对应名字的合成塔");
        return null;
    }

    public void PlayerLevelUp(int level)
    {
        List<SelectInfo> list = Game.Instance.SaveData.SaveRareDIC[level];
        foreach (var item in list)
        {
            if (item.isSelect)
            {
                BattleComposites.Add(CompositeDIC[item.turretName]);
            }
        }
    }

    public TurretAttribute GetRandomCompositeAtt()
    {
        return BattleComposites[UnityEngine.Random.Range(0, BattleComposites.Count)];
    }

    //陷阱*************

    public TrapContent GetTrapContentByName(string name)
    {
        if (TrapDIC.ContainsKey(name))
        {
            TrapContent content = Get(TrapDIC[name].ContentPrefab) as TrapContent;
            content.TrapAttribute = TrapDIC[name];
            return content;
        }
        Debug.LogWarning("没有对应名字的陷阱");
        return null;
    }


    public TrapContent GetRandomTrapContent()
    {
        int index = UnityEngine.Random.Range(0, trapAtts.Count);
        TrapContent content = Get(trapAtts[index].ContentPrefab) as TrapContent;
        content.TrapAttribute = trapAtts[index];
        return content;
    }


    //基座*************

    public TurretBaseContent GetTurretBaseContent(string name)
    {
        if (TurretBaseDIC.ContainsKey(name))
        {
            return Get(TurretBaseDIC[name].ContentPrefab) as TurretBaseContent;
        }
        Debug.LogWarning("没有对应名字的基座");
        return null;
    }

    public TurretBaseContent GetRandomTurretBase()
    {
        int index = UnityEngine.Random.Range(0, turretBaseAtts.Count);
        TurretBaseContent content = Get(turretBaseAtts[index].ContentPrefab) as TurretBaseContent;
        content.m_TurretBaseAttribute = turretBaseAtts[index];
        return content;
    }

    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = CreateInstance(prefab) as GameTileContent;
        return instance;
    }




}
