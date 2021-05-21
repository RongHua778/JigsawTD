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
    //**************
    //[SerializeField] List<GameTile> tiles = new List<GameTile>();
    [SerializeField] List<TurretTile> turretTiles = new List<TurretTile>();
    [SerializeField] GameTile ground;
    //**************
    private Dictionary<int, GameTile> tileDIC;

    private List<GameTile> Level0Tiles;
    private List<GameTile> Level1Tiles;
    private List<GameTile> Level2Tiles;
    private List<GameTile> Level3Tiles;
    private List<GameTile> Level4Tiles;
    private List<GameTile> Level5Tiles;

    [SerializeField] float[] levelTileChance = new float[6];
    [SerializeField] GroundTile groundTile = default;
    [SerializeField] GameTile spawnPoint = default;
    [SerializeField] GameTile destinationTile = default;


    public void InitializeFactory()
    {
        //tileDIC = new Dictionary<int, GameTile>();
        Level1Tiles = new List<GameTile>();
        Level2Tiles = new List<GameTile>();
        Level3Tiles = new List<GameTile>();
        Level4Tiles = new List<GameTile>();
        Level5Tiles = new List<GameTile>();
        foreach (TurretTile tile in turretTiles)
        {
            switch (tile.tile.m_TurretAttribute.quality)
            {
                case 1:
                    Level1Tiles.Add(tile);
                    break;
                case 2:
                    Level2Tiles.Add(tile);
                    break;
                case 3:
                    Level3Tiles.Add(tile);
                    break;
                case 4:
                    Level4Tiles.Add(tile);
                    break;
                case 5:
                    Level5Tiles.Add(tile);
                    break;
                default:
                    Debug.Assert(false, "定义了错误的TILE等级");
                    break;
            }
            //tileDIC.Add(tile.TileID, tile);
        }
    }

    public GameTile GetTile(int id)
    {
        if (tileDIC.ContainsKey(id))
        {
            return CreateInstance(tileDIC[id].gameObject).GetComponent<GameTile>();
        }
        else
        {
            Debug.Assert(false, "没有对应ID的TIle");
            return null;
        }
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

    public GroundTile GetGroundTile()
    {
        return CreateInstance(groundTile.gameObject).GetComponent<GroundTile>();

    }
    public BasicTile GetBasicTile()
    {
        return CreateInstance(GetRandomLevelTile(0).gameObject).GetComponent<BasicTile>();
    }
    //get random塔
    public GameTile GetRandomTile()
    {
        int level = StaticData.RandomNumber(levelTileChance);
        //Debug.Log(level);
        return CreateInstance(GetRandomLevelTile(level).gameObject).GetComponent<TurretTile>();
    }

    public GameTile GetRandomLevelTile(int level)
    {
        switch (level)
        {
            case 0:
                return ground;
            case 1:
                return Level1Tiles[Random.Range(0, Level1Tiles.Count)];
            case 2:
                return Level2Tiles[Random.Range(0, Level2Tiles.Count)];
            case 3:
                return Level3Tiles[Random.Range(0, Level3Tiles.Count)];
            case 4:
                return Level4Tiles[Random.Range(0, Level4Tiles.Count)];
            case 5:
                return Level5Tiles[Random.Range(0, Level5Tiles.Count)];
        }
        Debug.Assert(false, "使用了错误的等级获取TILE");
        return null;
    }

}
