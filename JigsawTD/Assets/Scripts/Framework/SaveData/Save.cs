using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    //��ѡ��ս��Ԫ��
    public List<int> SaveSelectedElement = new List<int>();
    //��ѡ��ϳ���

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
