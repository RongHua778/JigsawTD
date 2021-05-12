using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffInfo
{
    public EnemyBuffName EnemyBuffName;
    public float KeyValue;
    public float Duration;
    public BuffInfo(EnemyBuffName name,float keyValue,float duration)
    {
        this.EnemyBuffName = name;
        this.KeyValue = keyValue;
        this.Duration = duration;
    }
}


[CreateAssetMenu(menuName = "Attribute/TrapAttribute", fileName = "TrapAttribute")]
public class TrapAttribute : LevelAttribute
{
    public List<BuffInfo> TrapInfos = new List<BuffInfo>();
}
