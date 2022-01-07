using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BuffableEntity : MonoBehaviour
{
    public List<EnemyBuff> TileBuffs = new List<EnemyBuff>();
    public List<EnemyBuff> TimeBuffs = new List<EnemyBuff>();
    public Enemy Enemy { get; set; }

    private void Awake()
    {
        Enemy = this.GetComponent<Enemy>();
    }

    private void Update()
    {
        TimeTick();
    }

    private void TimeTick()
    {
        var effects = TimeBuffs.ToList().GetEnumerator();
        while (effects.MoveNext())
        {
            effects.Current.Tick(Time.deltaTime);
            if (effects.Current.IsFinished)
            {
                TimeBuffs.Remove(effects.Current);
            }
        }
    }

    public void TileTick()
    {
        foreach (var buff in TileBuffs.ToList())
        {
            buff.Tick(1);
            if (buff.IsFinished)
            {
                TileBuffs.Remove(buff);
            }
        }
    }

    public void AddBuff(BuffInfo buffInfo, float intensify = 1)
    {
        EnemyBuff newBuff = EnemyBuffFactory.GetBuff((int)buffInfo.EnemyBuffName);
        if (newBuff.IsTimeBase)
        {
            if (!newBuff.IsStackable)
            {
                foreach (var buff in TimeBuffs)
                {
                    if (buff.BuffName == newBuff.BuffName)
                    {
                        return;
                    }
                }
            }
            TimeBuffs.Add(newBuff);
        }
        else
        {
            if (!newBuff.IsStackable)
            {
                foreach (var buff in TileBuffs)
                {
                    if (buff.BuffName == newBuff.BuffName)
                    {
                        return;
                    }
                }
            }
            TileBuffs.Add(newBuff);
        }
        newBuff.ApplyBuff(Enemy, buffInfo.KeyValue * intensify, buffInfo.Duration);
    }

    public void RemoveAllBuffs()
    {
        TimeBuffs.Clear();
        TileBuffs.Clear();
    }

}
