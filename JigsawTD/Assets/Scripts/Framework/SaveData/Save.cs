using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    //��ѡ��ս��Ԫ��
    public List<ElementSelect> SaveSelectedElement = new List<ElementSelect>();
    //��ѡ��ϳ���

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
