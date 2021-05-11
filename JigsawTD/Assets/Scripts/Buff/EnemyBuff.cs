using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBuffName
{
    SlowDown, DealDamage, BreakShell, PathSlow, HealthBaseDamage
}
public abstract class EnemyBuff
{
    public abstract EnemyBuffName BuffName { get; }
    public bool IsFinished { get; set; }
    public abstract bool IsStackable { get; }
    public int Stacks;
    public int TileCount;
    public float KeyValue;
    public Enemy Target;

    public virtual void ApplyBuff(Enemy target, float keyValue, int tileCount)
    {
        this.Target = target;
        if (IsStackable)
        {
            Stacks += (int)keyValue;
            TileCount = tileCount;
            Affect();
        }
        else
        {
            if (KeyValue < keyValue)
            {
                KeyValue = keyValue;
                TileCount = tileCount;
                Affect();
            }
        }
    }


    public abstract void Affect();

    public virtual void Tick()//先TICK再Affect
    {
        TileCount--;
        if (TileCount <= 0)
        {
            End();
            IsFinished = true;
            return;
        }
    }

    public abstract void End();


}

public class SlowBuff : EnemyBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.SlowDown;
    public override bool IsStackable => false;

    public override void Affect()
    {
        Target.SlowRate = KeyValue;
    }
    public override void End()
    {
        Target.SlowRate = 0;
    }
}

public class PathSlow : EnemyBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.PathSlow;
    public override bool IsStackable => false;

    public override void Tick()//发生转向时移除BUFF
    {
        if (Target.DirectionChange != DirectionChange.None)
        {
            End();
            IsFinished = true;
            return;
        }
    }

    public override void Affect()
    {
        if (Target.Direction == Target.tileFrom.GetTileDirection())
            Target.PathSlow = KeyValue;
    }
    public override void End()
    {
        Target.PathSlow = 0;
    }
}

public class DealDamage : EnemyBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.DealDamage;

    public override bool IsStackable => false;

    public override void Affect()
    {
        Target.ApplyDamage(KeyValue);
    }

    public override void End()
    {

    }
}

public class BreakShell : EnemyBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.BreakShell;

    public override bool IsStackable => true;

    public override void Affect()
    {
        Target.BrokeShell = Stacks;
    }

    public override void End()
    {
        Target.BrokeShell = 0;
    }
}

public class HealthBaseDamage : EnemyBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.HealthBaseDamage;

    public override bool IsStackable => false;

    public override void Affect()
    {
        float damage = KeyValue * (Target.MaxHealth - Target.CurrentHealth);
        Debug.Log("Deal HelathBase Damge="+damage);
        Target.ApplyDamage(damage);
    }

    public override void End()
    {

    }
}
