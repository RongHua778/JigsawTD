using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此工厂只负责turret的后台逻辑如quality，element，合成所需配方等等。不负责sprite渲染和tile位置放置等
[CreateAssetMenu(menuName = "Factory/TurretFactory", fileName = "TurretFactory")]
public class TurretFactory : GameObjectFactory
{
    public GameObject[] basicTurrets;
    public GameObject[] composedTurrets;


    public GameObject GetBasicTurret(int quality,int element)
    {
        GameObject temp = CreateInstance(basicTurrets[element]);
        temp.GetComponentInChildren<Turret>().Quality = quality;
        return temp;
    }

    public GameObject GetComposedTurret(Blueprint blueprint)
    {
        GameObject temp = CreateInstance(composedTurrets[0]);
        temp.GetComponentInChildren<Turret>().Compositions = new List<Composition>(blueprint.Compositions);
        return temp;
    }
}
