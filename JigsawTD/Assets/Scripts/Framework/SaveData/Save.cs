using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Save
{
    public Dictionary<string, bool> UnlockInfoDIC;

    //¡Ÿ ±”Œœ∑±£¥Ê
    public List<ContentStruct> SaveContents;
}


[Serializable]
public class ContentStruct
{
    public int ContentType;
    public int posX;
    public int posY;
    public int Direction;
    public string ContentName;
    //Turret
    public int Element;
    public int Quality;
    public List<List<int>> SkillList;
    public double[] InitModifies;

    //public ContentStruct(GameTileContentType contentType, Vector2 contentPos, string contentName, int quality = 0, List<List<int>> skills = null, float[] initModify = null)
    //{
    //    this.ContentType = contentType;
    //    this.ContentPos = contentPos;
    //    this.ContentName = contentName;
    //    this.Quality = quality;
    //    this.SkillList = skills;
    //    this.InitModifies = initModify;
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
