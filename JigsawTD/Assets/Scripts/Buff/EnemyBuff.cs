using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBuffName
{
    SlowDown, DealDamage, BreakShell, DirectionSlow, HealthBaseDamage, DamageTarget, Stun, TileStun
}
public abstract class EnemyBuff
{
    public abstract EnemyBuffName BuffName { get; }
    public bool IsFinished { get; set; }
    public abstract bool IsTimeBase { get; }
    public abstract bool IsStackable { get; }
    public int Stacks;
    public float KeyValue;
    public Enemy Target;

    public virtual void ApplyBuff(Enemy target, float keyValue, float duration)
    {
        this.Target = target;
    }


    public abstract void Affect();

    public virtual void Tick(float delta)//先TICK再Affect
    {

    }

    public abstract void End();


}

public abstract class TimeBuff : EnemyBuff
{
    public float Duration;

    public override void ApplyBuff(Enemy target, float keyValue, float duration)
    {
        base.ApplyBuff(target, keyValue, duration);
        KeyValue = keyValue;
        Duration = duration;
        Affect();
    }
    public override void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }
}

public abstract class TileBuff : EnemyBuff
{
    public int TileCount;
    public override void ApplyBuff(Enemy target, float keyValue, float duration)
    {
        base.ApplyBuff(target, keyValue, duration);
        KeyValue = keyValue;
        TileCount = (int)duration;
        Affect();
    }
    public override void Tick(float delta)//先TICK再Affect
    {
        TileCount -= (int)delta;
        if (TileCount <= 0)
        {
            End();
            IsFinished = true;
            return;
        }
    }
}

public class SlowBuff : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.SlowDown;
    public override bool IsStackable => true;
    public override bool IsTimeBase => true;

    public override void Affect()
    {
        Target.SlowRate += KeyValue;
    }
    public override void End()
    {
        Target.SlowRate -= KeyValue;
    }
}

public class DirectionSlow : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.DirectionSlow;
    public override bool IsStackable => false;

    public override bool IsTimeBase => false;

    public override void Tick(float delta)//发生转向时移除BUFF
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
        //Debug.Log("SlowDown");
        if (Target.Direction == Target.tileFrom.GetTileDirection())
            Target.PathSlow += KeyValue;
    }
    public override void End()
    {
        Target.PathSlow -= KeyValue;
    }
}

public class DealDamage : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.DealDamage;

    public override bool IsStackable => true;

    public override bool IsTimeBase => false;

    public override void Affect()
    {
        //Target.ApplyDamage(KeyValue);
        Debug.Log("DealDamage" + KeyValue);
    }

    public override void End()
    {

    }
}

public class BreakShell : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.BreakShell;

    public override bool IsStackable => true;

    public override bool IsTimeBase => false;

    public override void Affect()
    {
        Target.BrokeShell += (int)KeyValue;
        Debug.Log("BreakShell" + (int)KeyValue);

    }

    public override void End()
    {
        Target.BrokeShell -= (int)KeyValue;
    }
}

public class HealthBaseDamage : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.HealthBaseDamage;

    public override bool IsStackable => false;

    public override bool IsTimeBase => false;

    public override void Affect()
    {
        float damage = KeyValue * (Target.MaxHealth - Target.CurrentHealth);
        Debug.Log("Deal HelathBase Damge=" + damage);
        //Target.ApplyDamage(damage);
    }

    public override void End()
    {

    }
}

public class DamageTarget : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.DamageTarget;

    public override bool IsTimeBase => false;

    public override bool IsStackable => false;


    public override void Affect()
    {
        float damageReturn;
        Target.ApplyDamage(Target.TargetDamageCounter * KeyValue, out damageReturn, true);
        ((TrapTile)(Target.tileFrom)).DamageAnalysis += (int)damageReturn;
        Target.TargetDamageCounter = 0;
        //Debug.Log("TriggerDamageTarget" + damageReturn);

    }

    public override void End()
    {

    }
}

public class TileStun : TileBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.TileStun;

    public override bool IsTimeBase => false;

    public override bool IsStackable => false;

    public override void Affect()
    {
        BuffInfo info = new BuffInfo(EnemyBuffName.Stun, Target.TileStunCounter * KeyValue, 0);
        Target.Buffable.AddBuff(info);
        Target.TileStunCounter = 0;
    }

    public override void End()
    {

    }
}

public class Stun : TimeBuff
{
    public override EnemyBuffName BuffName => EnemyBuffName.Stun;

    public override bool IsTimeBase => true;

    public override bool IsStackable => true;

    public override void Affect()
    {
        Target.StunTime += KeyValue;
        Debug.Log("TargetSutn" + KeyValue);
    }

    public override void End()
    {

    }
}
