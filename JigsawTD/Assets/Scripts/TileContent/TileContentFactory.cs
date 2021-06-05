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

    private Dictionary<Element, TurretAttribute> ElementDIC;
    private Dictionary<string, TrapAttribute> TrapDIC;

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
        foreach (TurretAttribute attribute in compositeTurrets)
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
        foreach (var attribute in elementTurrets)
        {
            ElementDIC.Add(attribute.element, attribute);
        }
        foreach(var attribute in trapAtts)
        {
            TrapDIC.Add(attribute.Name, attribute);
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
        }
        Debug.Assert(false, "Unsupported Type" + type);
        return null;
    }


    public ElementTurret GetRandomElementTurret(int playerLevel)
    {
        int element = Random.Range(0, 5);
        float[] qualityC = new float[5];
        for (int i = 0; i < 5; i++)
        {
            qualityC[i] = StaticData.Instance.QualityChances[playerLevel - 1, i];
        }
        int quality = StaticData.RandomNumber(qualityC) + 1;
        TurretAttribute attribute = ElementDIC[(Element)element];
        ElementTurret content= Get(attribute.ContentPrefab) as ElementTurret;
        content.Quality = quality;
        return content;
    }

    public ElementTurret GetElementTurret(Element element,int quality)
    {
        TurretAttribute attribute = ElementDIC[element];
        ElementTurret content = Get(attribute.ContentPrefab) as ElementTurret;
        content.Quality = quality;
        return content;
    }

    public TrapContent GetTrapContent(string name)
    {
        if (TrapDIC.ContainsKey(name))
        {
            return Get(TrapDIC[name].ContentPrefab) as TrapContent;
        }
        Debug.LogWarning("没有对应名字的陷阱");
        return null;
    }

    public TrapContent GetRandomTrapContent()
    {
        int index = Random.Range(0, trapAtts.Count);
        return Get(trapAtts[index].ContentPrefab) as TrapContent;
    }

    private GameTileContent Get(GameObject prefab)
    {
        GameTileContent instance = CreateInstance(prefab).GetComponent<GameTileContent>();
        return instance;
    }


}
