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
    [SerializeField] TurretFactory turretFactory;

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
    //get带有塔的tile
    public GameTile GetTurretTile()
    {
        GameObject temp =turretFactory.GetTurret();
        return temp.GetComponent<TurretTile>();
    }

}
