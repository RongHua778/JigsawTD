using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    //已选择战斗元素
    public List<SelectInfo> SaveSelectedElement;
    //已选择合成塔
    public List<SelectInfo> SaveSelectedRare1;
    public List<SelectInfo> SaveSelectedRare2;
    public List<SelectInfo> SaveSelectedRare3;
    public List<SelectInfo> SaveSelectedRare4;
    public List<SelectInfo> SaveSelectedRare5;
    public List<SelectInfo> SaveSelectedRare6;

    public Dictionary<int, List<SelectInfo>> SaveRareDIC;

    public void Initialize()
    {
        SetInitElements();
        SetInitRare();
    }

    private void SetInitRare()
    {
        SaveRareDIC = new Dictionary<int, List<SelectInfo>>();

        SaveSelectedRare1 = new List<SelectInfo>()
        {
            new SelectInfo("RAPIDER", false, true),
            new SelectInfo("CONSTRUCTOR", false, true)
        };
        SaveRareDIC.Add(1, SaveSelectedRare1);

        SaveSelectedRare2 = new List<SelectInfo>()
        {
            new SelectInfo("COOPORATIVE", false, true),
            new SelectInfo("SCATTER", false, true)
        };
        SaveRareDIC.Add(2, SaveSelectedRare2);

        SaveSelectedRare3 = new List<SelectInfo>()
        {
            new SelectInfo("ROTARY", false, true),
            new SelectInfo("SUPER", false, true)
        };
        SaveRareDIC.Add(3, SaveSelectedRare3);

        SaveSelectedRare4 = new List<SelectInfo>()
        {
            new SelectInfo("MORTAR", false, true),
            new SelectInfo("SNOW", false, true)
        };
        SaveRareDIC.Add(4, SaveSelectedRare4);

        SaveSelectedRare5 = new List<SelectInfo>()
        {
            new SelectInfo("SNIPER", false, true),
            new SelectInfo("BOOMERRANG",false, true)
        };
        SaveRareDIC.Add(5, SaveSelectedRare5);

        SaveSelectedRare6 = new List<SelectInfo>()
        {
            new SelectInfo("ULTRA", false, true),
            new SelectInfo("CORE", false, true)
        };
        SaveRareDIC.Add(6, SaveSelectedRare6);
    }

    public void SetInitElements()
    {
        SelectInfo select0 = new SelectInfo("GOLD", false, true);
        SelectInfo select1 = new SelectInfo("WOOD", false, true);
        SelectInfo select2 = new SelectInfo("WATER", false, true);
        SelectInfo select3 = new SelectInfo("FIRE", false, true);
        SelectInfo select4 = new SelectInfo("DUST", false, true);
        SaveSelectedElement = new List<SelectInfo> { select0, select1, select2, select3, select4 };
    }

    public void SetTutorialElements()
    {
        for (int i = 0; i < SaveSelectedElement.Count; i++)
        {
            if (i <= 3)
                SaveSelectedElement[i].isSelect = true;
            else
                SaveSelectedElement[i].isSelect = false;
        }
    }

    public void UnlockBonus(string bonusName)
    {
        foreach (var item in SaveSelectedElement)
        {
            if (item.turretName == bonusName)
            {
                item.isLock = false;
                return;
            }
        }
        foreach (var list in SaveRareDIC.Values)
        {
            foreach (var item in list)
            {
                if (item.turretName == bonusName)
                {
                    item.isLock = false;
                    return;
                }
            }
        }
    }

}

[Serializable]
public class SelectInfo
{
    public string turretName;
    public bool isLock;
    public bool isSelect;
    public SelectInfo(string turretName, bool isLock, bool select)
    {
        this.turretName = turretName;
        this.isLock = isLock;
        this.isSelect = select;
    }
}
