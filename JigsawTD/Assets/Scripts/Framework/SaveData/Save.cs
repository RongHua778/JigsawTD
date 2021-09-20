using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    //已选择战斗元素
    public List<ElementSelect> SaveSelectedElement = new List<ElementSelect>();
    //已选择合成塔

}

public class ElementSelect
{
    public int id;
    public bool isLock;
    public bool isSelect;
    public ElementSelect(int id,bool isLock, bool select)
    {
        this.id = id;
        this.isLock = isLock;
        this.isSelect = select;
    }
}
