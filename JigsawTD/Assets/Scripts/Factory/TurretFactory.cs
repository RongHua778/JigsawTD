using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//此工厂只负责turret的后台逻辑如quality，element，合成所需配方等等。不负责sprite渲染和tile位置放置等
[CreateAssetMenu(menuName = "Factory/TurretFactory", fileName = "TurretFactory")]
public class TurretFactory : GameObjectFactory
{
    public GameObject[] turrets;

    public GameObject GetTurret(int quality,int element)
    {
        turrets[element].GetComponentInChildren<Turret>().Quality = quality;
        return turrets[element];
    }
}
