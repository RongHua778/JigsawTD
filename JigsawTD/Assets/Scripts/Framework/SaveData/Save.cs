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
            new SelectInfo(0, false, true),
            new SelectInfo(0, false, true)
        };
        SaveRareDIC.Add(1, SaveSelectedRare1);

        SaveSelectedRare2 = new List<SelectInfo>()
        {
            new SelectInfo(0, false, true),
            new SelectInfo(0, false, true)
        };
        SaveRareDIC.Add(2, SaveSelectedRare2);

        SaveSelectedRare3 = new List<SelectInfo>()
        {
            new SelectInfo(0, false, true),
            new SelectInfo(0, false, true)
        };
        SaveRareDIC.Add(3, SaveSelectedRare3);

        SaveSelectedRare4 = new List<SelectInfo>()
        {
            new SelectInfo(0, false, true),
            new SelectInfo(0, true, false)
        };
        SaveRareDIC.Add(4, SaveSelectedRare4);

        SaveSelectedRare5 = new List<SelectInfo>()
        {
            new SelectInfo(0, false, true),
            new SelectInfo(0, true, false)
        };
        SaveRareDIC.Add(5, SaveSelectedRare5);

        SaveSelectedRare6 = new List<SelectInfo>()
        {
            new SelectInfo(0, false, true),
            new SelectInfo(0, true, false)
        };
        SaveRareDIC.Add(6, SaveSelectedRare6);
    }

    public void SetInitElements()
    {
        SelectInfo select0 = new SelectInfo(0, false, true);
        SelectInfo select1 = new SelectInfo(1, false, true);
        SelectInfo select2 = new SelectInfo(2, false, true);
        SelectInfo select3 = new SelectInfo(3, false, true);
        SelectInfo select4 = new SelectInfo(4, true, false);
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

}

public class SelectInfo
{
    public int id;
    public bool isLock;
    public bool isSelect;
    public SelectInfo(int id, bool isLock, bool select)
    {
        this.id = id;
        this.isLock = isLock;
        this.isSelect = select;
    }
}
