using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˹���ֻ����turret�ĺ�̨�߼���quality��element���ϳ������䷽�ȵȡ�������sprite��Ⱦ��tileλ�÷��õ�
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
