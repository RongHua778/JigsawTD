using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/ContentFactory", fileName = "GameContentFactory")]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField]
    ContentAttribute emptyAtt = default;
    [SerializeField]
    ContentAttribute destinationAtt = default;
    [SerializeField]
    ContentAttribute spawnPointAtt = default;

    [SerializeField]
    TurretAttribute[] elementTurrets = default;
    [SerializeField]
    TurretAttribute[] compositeTurrets = default;
    [SerializeField]
    TrapAttribute[] trapAtts = default;


    public GameTileContent GetNormalContent(GameTileContentType type)
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



    GameTileContent Get(GameObject prefab)
    {
        GameTileContent instance = CreateInstance(prefab).GetComponent<GameTileContent>();
        return instance;
    }


}
