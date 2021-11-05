using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    public int GameLevel;
    public int GameExp;
    public Dictionary<string, bool> UnlockInfoDIC;
    //public Dictionary<string, ItemLockInfo> SaveItemDIC;

    //public void Initialize()
    //{
    //    SetBasicSave();
    //}

    //private void SetBasicSave()
    //{
    //    GameExp = 0;
    //    GameLevel = 0;
    //    UnlockInfoDIC.Add("GOLD", false);
    //    UnlockInfoDIC.Add("WOOD", false);
    //    UnlockInfoDIC.Add("WATER", false);
    //    UnlockInfoDIC.Add("FIRE", false);
    //    UnlockInfoDIC.Add("DUST", false);


    //    //SaveItemDIC = new Dictionary<string, ItemLockInfo>();

    //    //ItemLockInfo element1 = new ItemLockInfo("GOLD", false);
    //    //ItemLockInfo element2 = new ItemLockInfo("WOOD", false);
    //    //ItemLockInfo element3 = new ItemLockInfo("WATER", false);
    //    //ItemLockInfo element4 = new ItemLockInfo("FIRE", false);
    //    //ItemLockInfo element5 = new ItemLockInfo("DUST", false);
    //    //SaveItemDIC.Add(element1.itemName, element1);
    //    //SaveItemDIC.Add(element2.itemName, element2);
    //    //SaveItemDIC.Add(element3.itemName, element3);
    //    //SaveItemDIC.Add(element4.itemName, element4);
    //    //SaveItemDIC.Add(element5.itemName, element5);

    //    //ItemLockInfo refactor11 = new ItemLockInfo("CONSTRUCTOR", false);
    //    //ItemLockInfo refactor12 = new ItemLockInfo("RAPIDER", true);
    //    //ItemLockInfo refactor21 = new ItemLockInfo("SCATTER", false);
    //    //ItemLockInfo refactor22 = new ItemLockInfo("COOPORATIVE", true);
    //    //ItemLockInfo refactor31 = new ItemLockInfo("ROTARY", false);
    //    //ItemLockInfo refactor32 = new ItemLockInfo("SUPER", true);
    //    //ItemLockInfo refactor41 = new ItemLockInfo("MORTAR", false);
    //    //ItemLockInfo refactor42 = new ItemLockInfo("SNOW", true);
    //    //ItemLockInfo refactor51 = new ItemLockInfo("SNIPER", false);
    //    //ItemLockInfo refactor52 = new ItemLockInfo("BOOMERRANG", true);
    //    //ItemLockInfo refactor61 = new ItemLockInfo("ULTRA", false);
    //    //ItemLockInfo refactor62 = new ItemLockInfo("CORE", true);
    //    //SaveItemDIC.Add(refactor11.itemName, refactor11);
    //    //SaveItemDIC.Add(refactor12.itemName, refactor12);
    //    //SaveItemDIC.Add(refactor21.itemName, refactor21);
    //    //SaveItemDIC.Add(refactor22.itemName, refactor22);
    //    //SaveItemDIC.Add(refactor31.itemName, refactor31);
    //    //SaveItemDIC.Add(refactor32.itemName, refactor32);
    //    //SaveItemDIC.Add(refactor41.itemName, refactor41);
    //    //SaveItemDIC.Add(refactor42.itemName, refactor42);
    //    //SaveItemDIC.Add(refactor51.itemName, refactor51);
    //    //SaveItemDIC.Add(refactor52.itemName, refactor52);
    //    //SaveItemDIC.Add(refactor61.itemName, refactor61);
    //    //SaveItemDIC.Add(refactor62.itemName, refactor62);

    //    //ItemLockInfo mark01 = new ItemLockInfo("STUNTRAP", false);
    //    //ItemLockInfo mark02 = new ItemLockInfo("EXPLOSIONTRAP", false);
    //    //ItemLockInfo mark03 = new ItemLockInfo("EXECUTETRAP", false);
    //    //ItemLockInfo mark04 = new ItemLockInfo("IMITATETRAP", false);
    //    //ItemLockInfo mark05 = new ItemLockInfo("PROMOTETRAP", false);
    //    //ItemLockInfo mark06 = new ItemLockInfo("BLINKTRAP", false);
    //    //ItemLockInfo mark07 = new ItemLockInfo("BONUSTRAP", true);
    //    //SaveItemDIC.Add(mark01.itemName, mark01);
    //    //SaveItemDIC.Add(mark02.itemName, mark02);
    //    //SaveItemDIC.Add(mark03.itemName, mark03);
    //    //SaveItemDIC.Add(mark04.itemName, mark04);
    //    //SaveItemDIC.Add(mark05.itemName, mark05);
    //    //SaveItemDIC.Add(mark06.itemName, mark06);
    //    //SaveItemDIC.Add(mark07.itemName, mark07);
    //}

    public void UnlockBonus(string bo)
    {
        //if (SaveItemDIC.ContainsKey(bo))
        //    SaveItemDIC[bo].isLock = false;
    }

    public void AddExp(int exp)
    {
        if (GameLevel >= LevelManager.Instance.GameLevels.Length)
            return;
        int need = LevelManager.Instance.GameLevels[GameLevel].ExpRequire - GameExp;
        if (exp >= need)
        {
            foreach(var item in LevelManager.Instance.GameLevels[GameLevel].UnlockItems)
            {
                UnlockBonus(item.Name);
            }
            GameLevel++;
            GameExp = 0;
            AddExp(exp - need);
        }
        else
        {
            GameExp += exp;
        }
    }

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
