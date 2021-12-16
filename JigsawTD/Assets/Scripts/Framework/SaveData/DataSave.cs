using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DataSave
{
    public Dictionary<string, bool> UnlockInfoDIC;

    public void SaveData(Dictionary<string, bool> unlockDic)
    {
        UnlockInfoDIC = unlockDic;
    }
}

[Serializable]
public class GameSave
{
    public bool HasLastGame => SaveContents != null && SaveContents.Count > 0;

    public List<BlueprintStruct> SaveBluePrints=new List<BlueprintStruct>();

    public GameResStruct SaveRes;

    //临时游戏保存
    public List<ContentStruct> SaveContents = new List<ContentStruct>();

    public List<EnemySequenceStruct> SaveSequences=new List<EnemySequenceStruct>();

    public void ClearGame()
    {
        SaveBluePrints.Clear();
        SaveRes = null;
        SaveContents.Clear();
        SaveSequences.Clear();
    }

    public void SaveGame(List<BlueprintStruct> saveBlueprints, GameResStruct saveRes, List<ContentStruct> saveContents, List<EnemySequenceStruct> saveSequences)
    {
        SaveBluePrints = saveBlueprints;
        SaveRes = saveRes;
        SaveContents = saveContents;
        SaveSequences = saveSequences;
    }
}

[Serializable]
public class BlueprintStruct
{
    public string Name;
    public List<int> ElementRequirements;
    public List<int> QualityRequirements;
    //public List<Composition> Compositions;

}

[Serializable]
public class EnemySequenceStruct
{
    public List<EnemySequence> SequencesList;
}


[Serializable]
public class GameResStruct
{
    public int Mode;//0=standard,1=Endless
    public int Coin;
    public int Wave;
    public int BuildCost;
    public int SwitchTrapCost;
    public int CurrentLife;
    public int MaxLife;
    public float BuildDiscount;
    public int SystemLevel;
    public int SystemUpgradeCost;
    public int TotalRefactor;
    public int TotalCooporative;
    public int TotalDamage;
    public int ShopCapacity;
    public int NextRefreshTurn;
    public int PefectElementCount;
    public bool DrawThisTurn;
    //游戏时长
}

[Serializable]
public class ContentStruct
{
    public int ContentType;

    public Vector2Int Pos;
    public int Direction;
    public string ContentName;
    //Turret
    public int Element;
    public int Quality;

    public int ElementSlotCount;
    public Dictionary<string, List<int>> SkillList;
}

[Serializable]
public class ItemLockInfo
{
    public string itemName;
    public bool isLock;
    public ItemLockInfo(string itemname, bool isLock)
    {
        this.itemName = itemname;
        this.isLock = isLock;
    }
}
