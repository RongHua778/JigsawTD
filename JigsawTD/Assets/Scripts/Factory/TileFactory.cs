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

    [SerializeField] float[] elementTileChance;
    int playerLevel;
    public int PlayerLevel { get => playerLevel; set => playerLevel = value; }

    [SerializeField] GroundTile groundTile = default;
    [SerializeField] GameTile spawnPoint = default;
    [SerializeField] GameTile destinationTile = default;
    [SerializeField] TurretFactory turretFactory;

    float[,] levelChance = new float[6, 5]
    {
        { 0.75f, 0.25f, 0f, 0f, 0f },
        { 0.6f, 0.3f, 0.1f, 0f, 0f },
        { 0.5f, 0.35f, 0.15f, 0.05f, 0f },
        { 0.38f, 0.4f, 0.2f, 0.1f, 0.02f },
        { 0.19f, 0.35f, 0.25f, 0.15f, 0.06f },
        { 0.1f, 0.3f, 0.3f, 0.2f, 0.1f }
    };
    public void InitializeFactory()
    {

    }

    public GameTile GetComposedTile(Blueprint blueprint)
    {
        GameObject temp=turretFactory.GetComposedTurret(blueprint);
        return temp.GetComponent<TurretTile>();
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
        float[] levelC = new float[5];
        for(int i = 0; i < 5; i++)
        {
            levelC[i] = levelChance[playerLevel-1, i];
        }
        int level = StaticData.RandomNumber(levelC)+1;
        GameObject temp =GetRandomElementTile(level, element);
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
            return turretFactory.GetBasicTurret(quality,element);
        }
    }
}
