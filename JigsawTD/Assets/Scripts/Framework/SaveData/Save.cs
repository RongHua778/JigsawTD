using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    //已选择战斗元素
    public List<int> SaveSelectedElement = new List<int>();
    //已选择合成塔

}

public class ElementSelect
{
    public int id;
    public bool isSelect;
    public ElementSelect(int id,bool select)
    {
        this.id = id;
        this.isSelect = select;
    }
}
