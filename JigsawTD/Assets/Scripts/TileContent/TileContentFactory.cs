using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/ContentFactory", fileName = "GameContentFactory")]
public class TileContentFactory : GameObjectFactory
{
    [SerializeField] ContentAttribute groundAtt = default;
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

    private Dictionary<Element, TurretAttribute> ElementDIC;
    private Dictionary<string, TurretAttribute> CompositeDIC;
    private Dictionary<string, TrapAttribute> TrapDIC;
    private Dictionary<string, TurretBaseAttribute> TurretBaseDIC;

    private List<TurretAttribute> Rare1Turrets;
    private List<TurretAttribute> Rare2Turrets;
    private List<TurretAttribute> Rare3Turrets;



    public void Initialize()
    {
        Rare1Turrets = new List<TurretAttribute>();
        Rare2Turrets = new List<TurretAttribute>();
        Rare3Turrets = new List<TurretAttribute>();
        ElementDIC = new Dictionary<Element, TurretAttribute>();
        TrapDIC = new Dictionary<string, TrapAttribute>();
        CompositeDIC = new Dictionary<string, TurretAttribute>();
        TurretBaseDIC = new Dictionary<string, TurretBaseAttribute>();
        foreach (TurretAttribute attribute in compositeTurrets)
        {
            CompositeDIC.Add(attribute.Name, attribute);
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
        foreach (var attribute in elementTurrets)
        {
            ElementDIC.Add(attribute.element, attribute);
        }
        foreach (var attribute in trapAtts)
        {
            TrapDIC.Add(attribute.Name, attribute);
        }
        foreach(var attribute in turretBaseAtts)
        {
            TurretBaseDIC.Add(attribute.Name, attribute);
        }
    }

    public GameTileContent GetBasicContent(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Empty:
                return Get(emptyAtt.ContentPrefab);
            case GameTileContentType.Destination:
                return Get(destinationAtt.ContentPrefab);
            case GameTileContentType.SpawnPoint:
                return Get(spawnPointAtt.ContentPrefab);
            case GameTileContentType.Ground:
                return Get(groundAtt.ContentPrefab);
        }
        Debug.Assert(false, "Unsupported Type" + type);
        return null;
    }

    //??????*******************

    public ElementTurret GetRandomElementTurret(int playerLevel)
    {
        int element = Random.Range(0, 5);
        float[] qualityC = new float[5];
        for (int i = 0; i < 5; i++)
        {
            qualityC[i] = StaticData.QualityChances[playerLevel - 1, i];
        }
        int quality = StaticData.RandomNumber(qualityC) + 1;
        TurretAttribute attribute = ElementDIC[(Element)element];
        ElementTurret content = Get(attribute.ContentPrefab) as ElementTurret;
        content.Strategy = new StrategyElement(attribute, quality, (Element)element, content);
        content.Strategy.SetQualityValue();
        content.InitializeTurret();
        return content;
    }

    public ElementTurret GetElementTurret(Element element, int quality)
    {
        TurretAttribute attribute = ElementDIC[element];
        ElementTurret content = Get(attribute.ContentPrefab) as ElementTurret;
        content.Strategy = new StrategyElement(attribute, quality, element, content);
        content.Strategy.SetQualityValue();
        content.InitializeTurret();
        return content;
    }

    public TurretAttribute GetElementAttribute(Element element)
    {
        TurretAttribute attribute = ElementDIC[element];
        return attribute;
    }

    //??????***********
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
        Debug.LogWarning("????????????????????");
        return null;
    }

    public TurretAttribute GetRandomCompositeAttributeByLevel(int level)
    {
        TurretAttribute attributeToReturn = null;
        float[] chances = new float[3];
        for (int i = 0; i < 3; i++)
        {
            chances[i] = StaticData.Instance.RareChances[level - 1, i];
        }
        int random = StaticData.RandomNumber(chances);
        switch (random)
        {
            case 0:
                attributeToReturn = Rare1Turrets[Random.Range(0, Rare1Turrets.Count)];
                break;
            case 1:
                attributeToReturn = Rare2Turrets[Random.Range(0, Rare2Turrets.Count)];
                break;
            case 2:
                attributeToReturn = Rare3Turrets[Random.Range(0, Rare3Turrets.Count)];
                break;
        }
        Debug.Assert(attributeToReturn != null, "????????????????");
        return attributeToReturn;
    }

    //????*************

    public TrapContent GetTrapContentByName(string name)
    {
        if (TrapDIC.ContainsKey(name))
        {
            TrapContent content = Get(TrapDIC[name].ContentPrefab) as TrapContent;
            content.m_TrapAttribute = TrapDIC[name];
            return content;
        }
        Debug.LogWarning("??????????????????");
        return null;
    }


    public TrapContent GetRandomTrapContent()
    {
        int index = Random.Range(0, trapAtts.Count);
        TrapContent content = Get(trapAtts[index].ContentPrefab) as TrapContent;
        content.m_TrapAttribute = trapAtts[index];
        return content;
    }


    //????*************

    public TurretBaseContent GetTurretBaseContent(string name)
    {
        if (TurretBaseDIC.ContainsKey(name))
        {
            return Get(TurretBaseDIC[name].ContentPrefab) as TurretBaseContent;
        }
        Debug.LogWarning("??????????????????");
        return null;
    }

    public TurretBaseContent GetRandomTurretBase()
    {
        int index = Random.Range(0, turretBaseAtts.Count);
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
