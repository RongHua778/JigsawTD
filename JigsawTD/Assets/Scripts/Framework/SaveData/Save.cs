using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    //public int GameLevel;
    //public int GameExp;
    public Dictionary<string, bool> UnlockInfoDIC;


    //public void UnlockBonus(string bo)
    //{
    //    //if (SaveItemDIC.ContainsKey(bo))
    //    //    SaveItemDIC[bo].isLock = false;
    //}

    //public void AddExp(int exp)
    //{
    //    if (GameLevel >= LevelManager.Instance.GameLevels.Length)
    //        return;
    //    int need = LevelManager.Instance.GameLevels[GameLevel].ExpRequire - GameExp;
    //    if (exp >= need)
    //    {
    //        foreach(var item in LevelManager.Instance.GameLevels[GameLevel].UnlockItems)
    //        {
    //            UnlockBonus(item.Name);
    //        }
    //        GameLevel++;
    //        GameExp = 0;
    //        AddExp(exp - need);
    //    }
    //    else
    //    {
    //        GameExp += exp;
    //    }
    //}

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
