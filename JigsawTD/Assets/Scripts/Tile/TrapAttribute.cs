using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrapInfo
{
    public EnemyBuffName EnemyBuffName;
    public float KeyValue;
    public int TileCount;
}


[CreateAssetMenu(menuName = "Attribute/TrapAttribute", fileName = "TrapAttribute")]
public class TrapAttribute : LevelAttribute
{
    public List<TrapInfo> TrapInfos = new List<TrapInfo>();
}
