using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/ContentFactory", fileName = "GameContentFactory")]
public class TileContentFactory : GameObjectFactory
{
    [SerializeField] TrapAttribute emptyAtt = default;
    [SerializeField] TrapAttribute spawnPointAtt = default;
    [SerializeField] TrapAttribute destinationAtt = default;

    [SerializeField]
    TurretAttribute[] elementTurrets = default;
    [SerializeField]
    TurretAttribute[] compositeTurrets = default;
    [SerializeField]
    List<TrapAttribute> trapAtts = default;
    [SerializeField]
    List<TurretBaseAttribute> turretBaseAtts = default;

    //private List<TurretAttribute> BattleElements;
    //private List<TurretAttribute> BattleComposites;

    private Dictionary<ElementType, TurretAttribute> ElementDIC;
    private Dictionary<string, TurretAttribute> CompositeDIC;
    private Dictionary<string, TrapAttribute> TrapDIC;
    private Dictionary<string, TurretBaseAttribute> TurretBaseDIC;

    private List<TurretAttribute> Rare1Att;
    private List<TurretAttribute> Rare2Att;
    private List<TurretAttribute> Rare3Att;
    private List<TurretAttribute> Rare4Att;
    private List<TurretAttribute> Rare5Att;
    private List<TurretAttribute> Rare6Att;

    private List<TrapAttribute> BattleTraps;


    public void Initialize()
    {

        ElementDIC = new Dictionary<ElementType, TurretAttribute>();
        TrapDIC = new Dictionary<string, TrapAttribute>();
        CompositeDIC = new Dictionary<string, TurretAttribute>();
        TurretBaseDIC = new Dictionary<string, TurretBaseAttribute>();

        Rare1Att = new List<TurretAttribute>();
        Rare2Att = new List<TurretAttribute>();
        Rare3Att = new List<TurretAttribute>();
        Rare4Att = new List<TurretAttribute>();
        Rare5Att = new List<TurretAttribute>();
        Rare6Att = new List<TurretAttribute>();

        BattleTraps = new List<TrapAttribute>();



        foreach (TurretAttribute attribute in compositeTurrets)
        {
            if (!attribute.isLock)
            {
                switch (attribute.Rare)
                {
                    case 1:
                        Rare1Att.Add(attribute);
                        break;
                    case 2:
                        Rare2Att.Add(attribute);
                        break;
                    case 3:
                        Rare3Att.Add(attribute);
                        break;
                    case 4:
                        Rare4Att.Add(attribute);
                        break;
                    case 5:
                        Rare5Att.Add(attribute);
                        break;
                    case 6:
                        Rare6Att.Add(attribute);
                        break;
                }
            }
            CompositeDIC.Add(attribute.Name, attribute);

        }
        foreach (var attribute in trapAtts)
        {
            if (!attribute.isLock)
            {
                BattleTraps.Add(attribute);
            }
            TrapDIC.Add(attribute.Name, attribute);
        }
        foreach (var attribute in turretBaseAtts)
        {
            TurretBaseDIC.Add(attribute.Name, attribute);
        }
        foreach (var attribute in elementTurrets)
        {
            ElementDIC.Add(attribute.element, attribute);
        }
    }


    //*******终点
    public GameTileContent GetDestinationPoint()
    {
        TrapContent content = Get(destinationAtt.Prefab) as TrapContent;
        content.Initialize(destinationAtt);
        return content;
    }
    //*******起点
    public GameTileContent GetSpawnPoint()
    {
        TrapContent content = Get(spawnPointAtt.Prefab) as TrapContent;
        content.Initialize(spawnPointAtt);
        return content;
    }

    public GameTileContent GetBasicContent(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Empty:
                return Get(emptyAtt.Prefab) as GameTileContent;
        }
        Debug.Assert(false, "Unsupported Type" + type);
        return null;
    }

    //元素塔*******************

    public ElementTurret GetRandomElementTurret()
    {
        float[] qualityC = new float[5];
        for (int i = 0; i < 5; i++)
        {
            qualityC[i] = StaticData.QualityChances[GameRes.ModuleLevel - 1, i];
        }
        int quality = StaticData.RandomNumber(qualityC) + 1;
        TurretAttribute attribute = elementTurrets[UnityEngine.Random.Range(0, StaticData.elementN)];
        ElementTurret content = Get(attribute.Prefab) as ElementTurret;
        content.Strategy = new StrategyBase(attribute, quality);
        content.Strategy.m_Turret = content;
        content.Strategy.SetQualityValue();
        content.InitializeTurret();
        return content;
    }

    public ElementTurret GetElementTurret(ElementType element, int quality)
    {
        TurretAttribute attribute = ElementDIC[element];
        ElementTurret content = Get(attribute.Prefab) as ElementTurret;
        content.Strategy = new StrategyBase(attribute, quality);
        content.Strategy.m_Turret = content;
        content.Strategy.SetQualityValue();
        content.InitializeTurret();
        return content;
    }

    public TurretAttribute GetElementAttribute(ElementType element)
    {
        TurretAttribute attribute = ElementDIC[element];
        return attribute;
    }

    public TurretAttribute GetRandomElementAttribute(bool isBattleElement = true)
    {
        return isBattleElement ?
            elementTurrets[UnityEngine.Random.Range(0, StaticData.elementN)] :
            ElementDIC[(ElementType)UnityEngine.Random.Range(0, ElementDIC.Count)];
    }
    //合成塔***********
    public CompositeTurret GetCompositeTurret(Blueprint bluePrint)
    {
        CompositeTurret content = Get(bluePrint.CompositeTurretAttribute.Prefab) as CompositeTurret;
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



    public TurretAttribute GetRandomCompositeAtt()
    {
        float[] rareChance = new float[6];
        for (int i = 0; i < 6; i++)
        {
            rareChance[i] = StaticData.RareChances[GameRes.ModuleLevel - 1, i];
        }
        int rare = StaticData.RandomNumber(rareChance) + 1;
        TurretAttribute atrributeToReturn = null;
        switch (rare)
        {
            case 1:
                atrributeToReturn = Rare1Att[UnityEngine.Random.Range(0, Rare1Att.Count)];
                break;
            case 2:
                atrributeToReturn = Rare2Att[UnityEngine.Random.Range(0, Rare2Att.Count)];
                break;
            case 3:
                atrributeToReturn = Rare3Att[UnityEngine.Random.Range(0, Rare3Att.Count)];
                break;
            case 4:
                atrributeToReturn = Rare4Att[UnityEngine.Random.Range(0, Rare4Att.Count)];
                break;
            case 5:
                atrributeToReturn = Rare5Att[UnityEngine.Random.Range(0, Rare5Att.Count)];
                break;
            case 6:
                atrributeToReturn = Rare6Att[UnityEngine.Random.Range(0, Rare6Att.Count)];
                break;
        }
        Debug.Assert(atrributeToReturn != null, "没有可以返回的配方");
        return atrributeToReturn;
    }

    //陷阱*************

    public TrapContent GetTrapContentByName(string name)
    {
        if (TrapDIC.ContainsKey(name))
        {
            TrapContent content = Get(TrapDIC[name].Prefab) as TrapContent;
            content.Initialize(TrapDIC[name]);
            return content;
        }
        Debug.LogWarning("没有对应名字的陷阱");
        return null;
    }


    public TrapContent GetRandomTrapContent()
    {
        TrapAttribute att = BattleTraps[UnityEngine.Random.Range(0, BattleTraps.Count)];
        TrapContent content = Get(att.Prefab) as TrapContent;
        content.Initialize(att);
        return content;
    }


    //基座*************

    public TurretBaseContent GetTurretBaseContent(string name)
    {
        if (TurretBaseDIC.ContainsKey(name))
        {
            return Get(TurretBaseDIC[name].Prefab) as TurretBaseContent;
        }
        Debug.LogWarning("没有对应名字的基座");
        return null;
    }

    public TurretBaseContent GetRandomTurretBase()
    {
        int index = UnityEngine.Random.Range(0, turretBaseAtts.Count);
        TurretBaseContent content = Get(turretBaseAtts[index].Prefab) as TurretBaseContent;
        content.m_TurretBaseAttribute = turretBaseAtts[index];
        return content;
    }

    private ReusableObject Get(ReusableObject prefab)
    {
        ReusableObject instance = CreateInstance(prefab);
        return instance;
    }




}
