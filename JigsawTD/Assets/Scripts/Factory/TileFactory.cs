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
    [SerializeField] GameObject ground;
    [SerializeField] GroundTile groundTile = default;
    [SerializeField] GameTile spawnPoint = default;
    [SerializeField] GameTile destinationTile = default;
    [SerializeField] float[] elementTileChance;

    [SerializeField] TrapAttribute[] trapAttributes;
    public void InitializeFactory()
    {

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
    //空的tile
    public GroundTile GetGroundTile()
    {
        return CreateInstance(groundTile.gameObject).GetComponent<GroundTile>();

    }
    //游戏中的地板
    public BasicTile GetBasicTile()
    {
        return CreateInstance(ground).GetComponent<BasicTile>();
    }

    public TrapTile GetRandomTrap()
    {
        int index = Random.Range(0,trapAttributes.Length);
        TrapTile trap = CreateInstance(trapAttributes[index].TilePrefab).GetComponent<TrapTile>();
        trap.tileType.rotation = DirectionExtensions.GetRandomRotation();
        return trap;
    }

    public TrapTile GetTrapByName(string name)
    {
        foreach(var attribute in trapAttributes)
        {
            if (attribute.Name == name)
            {
                TrapTile trap = CreateInstance(attribute.TilePrefab).GetComponent<TrapTile>();
                trap.tileType.rotation = DirectionExtensions.GetRandomRotation();
                return trap;
            }
        }
        Debug.LogWarning("没有这个名字的TRAP");
        return null;
    }


    //******************turrettile部分**************

    public GameTile GetBasicTurret(int quality, int element)
    {
        TurretAttribute attribute = GameManager.Instance.GetElementAttribute((Element)element);
        GameObject temp = CreateInstance(attribute.TilePrefab);
        TurretTile tile = temp.GetComponent<TurretTile>();
        tile.turret.m_TurretAttribute = attribute;
        tile.turret.Quality = quality;
        return temp.GetComponent<GameTile>();
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

    public GameTile GetCompositeTurretTile(TurretAttribute attribute)
    {
        GameObject temp = CreateInstance(attribute.TilePrefab);
        return temp.GetComponent<TurretTile>();
    }

}
