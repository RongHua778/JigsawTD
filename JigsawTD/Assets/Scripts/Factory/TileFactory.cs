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
    [SerializeField] List<TurretTile> turretTiles;
    [SerializeField] GameObject ground;
    private Dictionary<int, GameTile> tileDIC;

    [SerializeField] float[] elementTileChance;
    int playerLevel;
    public int LevelTileChance { get => playerLevel; set => playerLevel = value; }

    [SerializeField] GroundTile groundTile = default;
    [SerializeField] GameTile spawnPoint = default;
    [SerializeField] GameTile destinationTile = default;
    [SerializeField] TurretFactory turretFactory;

    float[] level1 = { 0f, 0.75f, 0.25f, 0f, 0f, 0f };
    float[] level2 = { 0f, 0.75f, 0.25f, 0f, 0f, 0f };
    float[] level3 = { 0f, 0.75f, 0.25f, 0f, 0f, 0f };
    float[] level4 = { 0f, 0.75f, 0.25f, 0f, 0f, 0f };
    float[] level5 = { 0f, 0.75f, 0.25f, 0f, 0f, 0f };
    float[] level6 = { 0f, 0.75f, 0.25f, 0f, 0f, 0f };

    public void InitializeFactory()
    {

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
        //这里的element可以随意填
        return CreateInstance(GetRandomElementTile(0,0)).GetComponent<BasicTile>();
    }
    //get random带有塔的tile
    public GameTile GetRandomTile()
    {
        int element = StaticData.RandomNumber(elementTileChance);
        //要根据playerlevel变化
        int level = StaticData.RandomNumber(level1);
        GameObject temp = CreateInstance(GetRandomElementTile(level, element));
        Debug.Log(playerLevel);
        return temp.GetComponent<TurretTile>();
    }

    public GameObject GetRandomElementTile(int quality,int element)
    {
        if (quality < 0 || quality > StaticData.maxQuality) quality = 1;
        if (quality == 0)
        {
            return ground;
        }
        else
        {
            return turretFactory.GetTurret(quality,element);
        }
    }
}
