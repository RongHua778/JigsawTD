using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BasicTileType
{
    None, SpawnPoint, Destination, Ground, Turret, Trap
}
[CreateAssetMenu(menuName = "Factory/TileFactory", fileName = "TileFactory")]
public class TileFactory : GameObjectFactory
{
    private GameTileContentFactory m_ContentFactory;

    [SerializeField] GameObject ground;
    [SerializeField] GroundTile groundTile = default;
    [SerializeField] GameTile spawnPoint = default;
    [SerializeField] GameTile destinationTile = default;
    [SerializeField] float[] elementTileChance;

    [SerializeField] GameObject basicTilePrefab = default;

    [SerializeField] TrapAttribute[] trapAttributes;


    public void Initialize()
    {
        m_ContentFactory = GameManager.Instance.ContentFactory;
    }

    public GameTile BuildNormalTile(GameTileContentType contentType)
    {
        BasicTile basicTile = ObjectPool.Instance.Spawn(basicTilePrefab).GetComponent<BasicTile>();
        GameTileContent content = m_ContentFactory.GetBasicContent(contentType);
        content.transform.SetParent(basicTile.transform);
        basicTile.Content = content;
        return basicTile;
    }


    public GameTile BuildTrapTile()
    {
        return null;
    }

    public GameTile BuildTurretTile()
    {
        return null;

    }

    public TrapTile GetImportantTile(BasicTileType tileType)
    {
        switch (tileType)
        {
            case BasicTileType.SpawnPoint:
                return CreateInstance(spawnPoint.gameObject).GetComponent<TrapTile>();
            case BasicTileType.Destination:
                return CreateInstance(destinationTile.gameObject).GetComponent<TrapTile>();
        }
        return null;
    }
    //�յ�tile
    public GroundTile GetGroundTile()
    {
        return CreateInstance(groundTile.gameObject).GetComponent<GroundTile>();

    }
    //��Ϸ�еĵذ�
    public BasicTile GetBasicTile()
    {
        return CreateInstance(ground).GetComponent<BasicTile>();
    }

    public TrapTile GetRandomTrap()
    {
        int index = Random.Range(0,trapAttributes.Length);
        TrapTile trap = CreateInstance(trapAttributes[index].ContentPrefab).GetComponent<TrapTile>();
        trap.tileType.rotation = DirectionExtensions.GetRandomRotation();
        return trap;
    }

    public TrapTile GetTrapByName(string name)
    {
        foreach(var attribute in trapAttributes)
        {
            if (attribute.Name == name)
            {
                TrapTile trap = CreateInstance(attribute.ContentPrefab).GetComponent<TrapTile>();
                trap.tileType.rotation = DirectionExtensions.GetRandomRotation();
                return trap;
            }
        }
        Debug.LogWarning("û��������ֵ�TRAP");
        return null;
    }


    //******************turrettile����**************

    public TurretTile GetBasicTurret(int quality, int element)
    {
        TurretAttribute attribute = GameManager.Instance.GetElementAttribute((Element)element);
        GameObject temp = CreateInstance(attribute.ContentPrefab);
        TurretTile tile = temp.GetComponent<TurretTile>();
        tile.Initialize(attribute,quality);
        return tile;
    }
    public GameTile GetRandomElementTile()
    {
        int playerLevel = LevelUIManager.Instance.PlayerLevel;
        int element = StaticData.RandomNumber(elementTileChance);
        float[] levelC = new float[5];
        for (int i = 0; i < 5; i++)
        {
            levelC[i] = StaticData.Instance.LevelChances[playerLevel - 1, i];
        }
        int level = StaticData.RandomNumber(levelC) + 1;
        GameTile temp = GetBasicTurret(level, element);
        return temp;
    }

    public TurretTile GetCompositeTurretTile(TurretAttribute attribute)
    {
        GameObject temp = CreateInstance(attribute.ContentPrefab);
        TurretTile tile = temp.GetComponent<TurretTile>();
        tile.Initialize(attribute, 1);
        return tile;
    }

}
