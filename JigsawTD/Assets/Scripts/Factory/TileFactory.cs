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
        TrapTile trap=CreateInstance(trapTile[index].gameObject).GetComponent<TrapTile>();
        int direction = Random.Range(0, 4);
        trap.gameObject.transform.localRotation= Quaternion.Euler(new Vector3(0, 0, 90*direction));
        Transform t = trap.gameObject.transform.GetChild(0);
        t.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90 * direction));
        return trap;
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
