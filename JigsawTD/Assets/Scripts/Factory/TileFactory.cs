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
    [SerializeField] TrapTile[] trapTile = default;
    [SerializeField] GameTile spawnPoint = default;
    [SerializeField] GameTile destinationTile = default;
    [SerializeField] float[] elementTileChance;

    public void InitializeFactory()
    {

    }
    public GameTile GetBasicTile(BasicTileType tileType)
    {
        switch (tileType)
        {
            case BasicTileType.SpawnPoint:
                return CreateInstance(spawnPoint.gameObject).GetComponent<GameTile>();
            case BasicTileType.Destination:
                return CreateInstance(destinationTile.gameObject).GetComponent<GameTile>();
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
        int index = Random.Range(0,trapTile.Length);
        return CreateInstance(trapTile[index].gameObject).GetComponent<TrapTile>();
    }


    //******************turrettile部分**************

    private GameObject GetBasicTurret(int quality, int element)
    {
        GameObject temp = CreateInstance(StaticData.Instance.GetElementsAttributes((Element)element).TilePrefab);
        TurretTile tile = temp.GetComponent<TurretTile>();
        tile.turret.Quality = quality;
        return temp;
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
        GameObject temp = GetBasicTurret(level, element);
        return temp.GetComponent<GameTile>();
    }

    public GameTile GetCompositeTurretTile(TurretAttribute attribute)
    {
        GameObject temp = CreateInstance(attribute.TilePrefab);
        return temp.GetComponent<TurretTile>();
    }

}
