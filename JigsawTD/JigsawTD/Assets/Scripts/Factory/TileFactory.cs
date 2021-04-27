using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/TileFactory", fileName = "TileFactory")]
public class TileFactory : ScriptableObject
{

    [SerializeField] List<GameTile> tiles = new List<GameTile>();
    private Dictionary<int, GameTile> tileDIC = new Dictionary<int, GameTile>();

    private List<GameTile> Level0Tiles = new List<GameTile>();
    private List<GameTile> Level1Tiles = new List<GameTile>();
    private List<GameTile> Level2Tiles = new List<GameTile>();
    private List<GameTile> Level3Tiles = new List<GameTile>();

    [SerializeField] float[] levelTileChance = new float[4];


    public void InitializeFactory()
    {
        tileDIC.Clear();
        foreach(GameTile tile in tiles)
        {
            switch (tile.TileLevel)
            {
                case 0:
                    Level0Tiles.Add(tile);
                    break;
                case 1:
                    Level1Tiles.Add(tile);
                    break;
                case 2:
                    Level2Tiles.Add(tile);
                    break;
                case 3:
                    Level3Tiles.Add(tile);
                    break;
                default:
                    Debug.Assert(false, "定义了错误的TILE等级");
                    break;
            }
            tileDIC.Add(tile.TileID, tile);
        }
    }

    public GameTile GetTile(int id)
    {
        if (tileDIC.ContainsKey(id))
        {
            return Instantiate(tileDIC[id]);
        }
        else
        {
            Debug.Assert(false, "没有对应ID的TIle");
            return null;
        }
    }

    public GameTile GetRandomTile()
    {
        int level = StaticData.RandomNumber(levelTileChance);
        return Instantiate(GetRandomLevelTile(level));
    }

    public GameTile GetRandomLevelTile(int level)
    {
        switch (level)
        {
            case 0:
                return Level0Tiles[Random.Range(0, Level0Tiles.Count)];
            case 1:
                return Level1Tiles[Random.Range(0, Level1Tiles.Count)];
            case 2:
                return Level2Tiles[Random.Range(0, Level2Tiles.Count)];
            case 3:
                return Level3Tiles[Random.Range(0, Level3Tiles.Count)];
        }
        Debug.Assert(false, "使用了错误的等级获取TILE");
        return null;
    }

}
