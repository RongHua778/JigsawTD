using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此工厂只负责turret的后台逻辑如quality，element，合成所需配方等等。不负责sprite渲染和tile位置放置等
[CreateAssetMenu(menuName = "Factory/TurretFactory", fileName = "TurretFactory")]
public class TurretFactory : GameObjectFactory
{
    public GameObject[] basicTurrets;
    public GameObject[] composedTurrets;
    [SerializeField] float[] elementTileChance;
    float[,] levelChance = new float[6, 5]
{
        { 0.75f, 0.25f, 0f, 0f, 0f },
        { 0.6f, 0.3f, 0.1f, 0f, 0f },
        { 0.5f, 0.35f, 0.15f, 0.05f, 0f },
        { 0.38f, 0.4f, 0.2f, 0.1f, 0.02f },
        { 0.19f, 0.35f, 0.25f, 0.15f, 0.06f },
        { 0.1f, 0.3f, 0.3f, 0.2f, 0.1f }
};
    private GameObject GetBasicTurret(int quality,int element)
    {
        GameObject temp = CreateInstance(basicTurrets[element]);
        temp.GetComponentInChildren<Turret>().Quality = quality;
        return temp;
    }

    private GameObject GetComposedTurret()
    {
        GameObject temp = CreateInstance(composedTurrets[0]);
        Blueprint blueprint = GameManager.Instance.playerManager.BlueprintInBuilding;
        temp.GetComponentInChildren<Turret>().Compositions = new List<Composition>(blueprint.Compositions);
        return temp;
    }

    //随机返回一个基础元素塔
    private GameObject GetRandomBasicTurret()
    {
        int playerLevel = GameManager.Instance.playerManager.PlayerLevel;
        int element = StaticData.RandomNumber(elementTileChance);
        float[] levelC = new float[5];
        for (int i = 0; i < 5; i++)
        {
            levelC[i] = levelChance[playerLevel-1, i];
        }
        int level = StaticData.RandomNumber(levelC) + 1;
        GameObject temp = GetBasicTurret(level, element);
        return temp;
    }

    public GameObject GetTurret()
    {
        switch (GameManager.Instance.playerManager.PlayerWish)
        {
            case PlayerWish.Element:
                return GetRandomBasicTurret();
            case PlayerWish.Composition:
                return GetComposedTurret();
        }
        Debug.LogWarning("不知道玩家想干嘛！");
        return null;
    }
}
