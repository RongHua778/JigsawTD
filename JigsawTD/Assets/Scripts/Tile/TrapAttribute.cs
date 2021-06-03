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

[System.Serializable]
public class TrapInfo
{
    public List<BuffInfo> BuffInfos = new List<BuffInfo>();
}

[CreateAssetMenu(menuName = "Attribute/TrapAttribute", fileName = "TrapAttribute")]
public class TrapAttribute : ContentAttribute
{
    public Sprite TrapIcon;
    public List<BuffInfo> BuffInfos = new List<BuffInfo>();
    //public List<TrapInfo> LevelInfos = new List<TrapInfo>();
}
