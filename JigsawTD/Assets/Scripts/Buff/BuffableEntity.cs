using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BuffableEntity : MonoBehaviour
{
    public Dictionary<int, EnemyBuff> Buffs = new Dictionary<int, EnemyBuff>();
    public Enemy Enemy { get; set; }

    private void Awake()
    {
        Enemy = this.GetComponent<Enemy>();
    }

    public void Tick()
    {
        foreach (var buff in Buffs.Values.ToList())
        {
            buff.Tick();
            if (buff.IsFinished)
            {
                Buffs.Remove((int)buff.BuffName);
            }
        }
    }

    public void AddBuff(TrapInfo trap)
    {
        EnemyBuff newBuff = EnemyBuffFactory.GetBuff((int)trap.EnemyBuffName);
        if (Buffs.ContainsKey((int)newBuff.BuffName))
        {
            EnemyBuff buff = Buffs[(int)newBuff.BuffName];
            buff.ApplyBuff(Enemy, trap.KeyValue, trap.TileCount);
        }
        else
        {
            Buffs.Add((int)newBuff.BuffName, newBuff);
            newBuff.ApplyBuff(Enemy, trap.KeyValue, trap.TileCount);
        }
    }

    public void RemoveAllBuffs()
    {
        foreach (var buff in Buffs.Values.ToList())
        {
            buff.End();
        }
        Buffs.Clear();
    }

}
