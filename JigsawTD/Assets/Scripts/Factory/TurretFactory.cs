using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˹���ֻ����turret�ĺ�̨�߼���quality��element���ϳ������䷽�ȵȡ�������sprite��Ⱦ��tileλ�÷��õ�
[CreateAssetMenu(menuName = "Factory/TurretFactory", fileName = "TurretFactory")]
public class TurretFactory : GameObjectFactory
{

    //public GameObject test;
    //[SerializeField] float[] elementTileChance;

    //private GameObject GetBasicTurret(int quality,int element)
    //{
    //    GameObject temp = CreateInstance(StaticData.Instance.GetElementsAttributes((Element)element).TilePrefab);
    //    temp.GetComponentInChildren<Turret>().Quality = quality;
    //    return temp;
    //}

    //public GameObject GetComposedTurret(TurretAttribute attribute)
    //{
    //    GameObject temp = CreateInstance(attribute.TilePrefab);
    //    return temp;
    //}


    ////�������һ������Ԫ����
    //public GameTile GetRandomBasicTurret()
    //{
    //    int playerLevel = LevelUIManager.Instance.PlayerLevel;
    //    int element = StaticData.RandomNumber(elementTileChance);
    //    float[] levelC = new float[5];
    //    for (int i = 0; i < 5; i++)
    //    {
    //        levelC[i] = StaticData.Instance.LevelChances[playerLevel-1, i];
    //    }
    //    int level = StaticData.RandomNumber(levelC) + 1;
    //    GameObject temp = GetBasicTurret(level, element);
    //    return temp.GetComponent<GameTile>();
    //}

    //public GameObject GetTurret()
    //{
    //    switch (GameManager.Instance.playerManager.PlayerWish)
    //    {
    //        //case PlayerWish.none:
    //        //    return GetNoneTurret();
    //        //case PlayerWish.Element:
    //        //    return GetRandomBasicTurret();
    //        //case PlayerWish.Composition:
    //        //    return GetComposedTurret();
    //    }
    //    Debug.LogWarning("��֪���������");
    //    return null;
    //}
}
