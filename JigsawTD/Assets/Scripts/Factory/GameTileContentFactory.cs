using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/ContentFactory", fileName = "GameContentFactory")]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField]
    GameTileContent emptyPrefab = default;
    [SerializeField]
    GameTileContent destinationPrefab = default;
    [SerializeField]
    GameTileContent spawnPointPrefab = default;
    [SerializeField]
    GameTileContent turretPrefab = default;
    [SerializeField]
    GameTileContent rockPrefab = default;

    public void Reclaim(GameTileContent content)
    {
        //Debug.Assert(content.OriginFactory == this, "Wrong factory reclaimed!");
        //content.OriginFactory = null;
        ObjectPool.Instance.UnSpawn(content.gameObject);
    }

    public GameTileContent Get(GameTileContentType type)
    {
        switch (type)
        {
            case GameTileContentType.Empty:
                return Get(emptyPrefab);
            case GameTileContentType.Destination:
                return Get(destinationPrefab);
            case GameTileContentType.SpawnPoint:
                return Get(spawnPointPrefab);
            case GameTileContentType.Turret:
                return Get(turretPrefab);
            case GameTileContentType.Rock:
                return Get(rockPrefab);
        }
        Debug.Assert(false, "Unsupported Type" + type);
        return null;
    }

    GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = CreateInstance(prefab.gameObject).GetComponent<GameTileContent>();
        //instance.OriginFactory = this;
        return instance;
    }


}
