using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/TileFactory", fileName = "TileFactory")]
public class TileFactory : ScriptableObject
{

    [SerializeField] List<GameTile> tiles = new List<GameTile>();
    private Dictionary<int, GameTile> tileDIC = new Dictionary<int, GameTile>();
    
    public void InitializeFactory()
    {
        tileDIC.Clear();
        foreach(GameTile tile in tiles)
        {
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
        return Instantiate(tileDIC[Random.Range(0, tileDIC.Count - 1)]);
    }

}
