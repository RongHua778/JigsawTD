using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    public Dictionary<string, ItemLockInfo> SaveItemDIC;
    ////已选择战斗元素
    //public List<SelectInfo> SaveSelectedElement;
    ////已选择合成塔
    //public List<SelectInfo> SaveSelectedRare1;
    //public List<SelectInfo> SaveSelectedRare2;
    //public List<SelectInfo> SaveSelectedRare3;
    //public List<SelectInfo> SaveSelectedRare4;
    //public List<SelectInfo> SaveSelectedRare5;
    //public List<SelectInfo> SaveSelectedRare6;

    //public Dictionary<int, List<SelectInfo>> SaveRareDIC;

    public void Initialize()
    {
        //SetInitElements();
        //SetInitRare();
        SetInitSave();
    }

    private void SetInitSave()
    {
        SaveItemDIC = new Dictionary<string, ItemLockInfo>();

        ItemLockInfo element1 = new ItemLockInfo("GOLD",false);
        ItemLockInfo element2 = new ItemLockInfo("WOOD", false);
        ItemLockInfo element3 = new ItemLockInfo("WATER", false);
        ItemLockInfo element4 = new ItemLockInfo("FIRE", false);
        ItemLockInfo element5 = new ItemLockInfo("DUST", false);
        SaveItemDIC.Add(element1.itemName, element1);
        SaveItemDIC.Add(element2.itemName, element2);
        SaveItemDIC.Add(element3.itemName, element3);
        SaveItemDIC.Add(element4.itemName, element4);
        SaveItemDIC.Add(element5.itemName, element5);

        ItemLockInfo refactor11 = new ItemLockInfo("CONSTRUCTOR", false);
        ItemLockInfo refactor12 = new ItemLockInfo("RAPIDER", true);
        ItemLockInfo refactor21 = new ItemLockInfo("SCATTER", false);
        ItemLockInfo refactor22 = new ItemLockInfo("COOPORATIVE", true);
        ItemLockInfo refactor31 = new ItemLockInfo("ROTARY", false);
        ItemLockInfo refactor32 = new ItemLockInfo("SUPER", true);
        ItemLockInfo refactor41 = new ItemLockInfo("MORTAR", false);
        ItemLockInfo refactor42 = new ItemLockInfo("SNOW", true);
        ItemLockInfo refactor51 = new ItemLockInfo("SNIPER", false);
        ItemLockInfo refactor52 = new ItemLockInfo("BOOMERRANG", true);
        ItemLockInfo refactor61 = new ItemLockInfo("ULTRA", false);
        ItemLockInfo refactor62 = new ItemLockInfo("CORE", true);
        SaveItemDIC.Add(refactor11.itemName, refactor11);
        SaveItemDIC.Add(refactor12.itemName, refactor12);
        SaveItemDIC.Add(refactor21.itemName, refactor21);
        SaveItemDIC.Add(refactor22.itemName, refactor22);
        SaveItemDIC.Add(refactor31.itemName, refactor31);
        SaveItemDIC.Add(refactor32.itemName, refactor32);
        SaveItemDIC.Add(refactor41.itemName, refactor41);
        SaveItemDIC.Add(refactor42.itemName, refactor42);
        SaveItemDIC.Add(refactor51.itemName, refactor51);
        SaveItemDIC.Add(refactor52.itemName, refactor52);
        SaveItemDIC.Add(refactor61.itemName, refactor61);
        SaveItemDIC.Add(refactor62.itemName, refactor62);

        ItemLockInfo mark01 = new ItemLockInfo("STUNTRAP", false);
        ItemLockInfo mark02 = new ItemLockInfo("EXPLOSIONTRAP", false);
        ItemLockInfo mark03 = new ItemLockInfo("EXECUTETRAP", false);
        ItemLockInfo mark04 = new ItemLockInfo("IMITATETRAP", false);
        ItemLockInfo mark05 = new ItemLockInfo("PROMOTETRAP", false);
        ItemLockInfo mark06 = new ItemLockInfo("BLINKTRAP", false);
        ItemLockInfo mark07 = new ItemLockInfo("BONUSTRAP", true);
        SaveItemDIC.Add(mark01.itemName, mark01);
        SaveItemDIC.Add(mark02.itemName, mark02);
        SaveItemDIC.Add(mark03.itemName, mark03);
        SaveItemDIC.Add(mark04.itemName, mark04);
        SaveItemDIC.Add(mark05.itemName, mark05);
        SaveItemDIC.Add(mark06.itemName, mark06);
        SaveItemDIC.Add(mark07.itemName, mark07);



    }

    //private void SetInitRare()
    //{
    //    SaveRareDIC = new Dictionary<int, List<SelectInfo>>();

    //    SaveSelectedRare1 = new List<SelectInfo>()
    //    {
    //        new SelectInfo("RAPIDER", false, true),
    //        new SelectInfo("CONSTRUCTOR", false, true)
    //    };
    //    SaveRareDIC.Add(1, SaveSelectedRare1);

    //    SaveSelectedRare2 = new List<SelectInfo>()
    //    {
    //        new SelectInfo("COOPORATIVE", false, true),
    //        new SelectInfo("SCATTER", false, true)
    //    };
    //    SaveRareDIC.Add(2, SaveSelectedRare2);

    //    SaveSelectedRare3 = new List<SelectInfo>()
    //    {
    //        new SelectInfo("ROTARY", false, true),
    //        new SelectInfo("SUPER", false, true)
    //    };
    //    SaveRareDIC.Add(3, SaveSelectedRare3);

    //    SaveSelectedRare4 = new List<SelectInfo>()
    //    {
    //        new SelectInfo("MORTAR", false, true),
    //        new SelectInfo("SNOW", false, true)
    //    };
    //    SaveRareDIC.Add(4, SaveSelectedRare4);

    //    SaveSelectedRare5 = new List<SelectInfo>()
    //    {
    //        new SelectInfo("SNIPER", false, true),
    //        new SelectInfo("BOOMERRANG",false, true)
    //    };
    //    SaveRareDIC.Add(5, SaveSelectedRare5);

    //    SaveSelectedRare6 = new List<SelectInfo>()
    //    {
    //        new SelectInfo("ULTRA", false, true),
    //        new SelectInfo("CORE", false, true)
    //    };
    //    SaveRareDIC.Add(6, SaveSelectedRare6);
    //}

    //public void SetInitElements()
    //{
    //    SelectInfo select0 = new SelectInfo("GOLD", false, true);
    //    SelectInfo select1 = new SelectInfo("WOOD", false, true);
    //    SelectInfo select2 = new SelectInfo("WATER", false, true);
    //    SelectInfo select3 = new SelectInfo("FIRE", false, true);
    //    SelectInfo select4 = new SelectInfo("DUST", false, true);
    //    SaveSelectedElement = new List<SelectInfo> { select0, select1, select2, select3, select4 };
    //}

    //public void SetTutorialElements()
    //{
    //    for (int i = 0; i < SaveSelectedElement.Count; i++)
    //    {
    //        if (i <= 3)
    //            SaveSelectedElement[i].isSelect = true;
    //        else
    //            SaveSelectedElement[i].isSelect = false;
    //    }
    //}

    //public void UnlockBonus(string bonusName)
    //{
    //    foreach (var item in SaveSelectedElement)
    //    {
    //        if (item.itemName == bonusName)
    //        {
    //            item.isLock = false;
    //            return;
    //        }
    //    }
    //    foreach (var list in SaveRareDIC.Values)
    //    {
    //        foreach (var item in list)
    //        {
    //            if (item.itemName == bonusName)
    //            {
    //                item.isLock = false;
    //                return;
    //            }
    //        }
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
