using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˹���ֻ����turret�ĺ�̨�߼���quality��element���ϳ������䷽�ȵȡ�������sprite��Ⱦ��tileλ�÷��õ�
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
