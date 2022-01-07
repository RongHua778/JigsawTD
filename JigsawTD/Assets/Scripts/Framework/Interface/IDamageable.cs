using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    IDamageStrategy DStrategy { get; set; }
}

public interface IBuffable
{
    IBuffStategy BStrategy { get; set; }
}

public interface IBuffStategy
{
    public List<EnemyBuff> TileBuffs { get; set; }
    public List<EnemyBuff> TimeBuffs { get; set; }

    float SlowRate { get; set; }
    float SlowResist { get; set; }
    float SpeedIntensify { get; set; }
    float DamageIntensify { get; set; }
    float TrapIntensify { get; set; }
    float CurrentFrost { get; set; }
    float MaxFrost { get; set; }
    float StunTime { get; set; }
    void ApplyBuff();
    void OnUpdate();
    void TimeTick();
    void TileTick();
    void ClearBuffs();
}

public interface IDamageStrategy
{
    string ExplosionSound { get; }
    float MaxHealth { get; }
    float CurrentHealth { get; }
    bool IsDie { get; }
    bool IsEnemy { get; }
    void ApplyDamage();
    void OnDie();
    void OnUpdate();
    void Reset();

}

public class EnemyBuffStategy : IBuffStategy
{
    public List<EnemyBuff> TileBuffs { get; set; }
    public List<EnemyBuff> TimeBuffs { get; set; }

    public float SlowRate { get; set; }

    public float SlowResist { get; set; }

    public float SpeedIntensify { get; set; }

    public float DamageIntensify { get; set; }

    public float TrapIntensify { get; set; }

    public float CurrentFrost { get; set; }

    public float MaxFrost { get; set; }

    public float StunTime { get; set; }

    public void ApplyBuff()
    {
        
    }

    public void ClearBuffs()
    {
        
    }

    public void OnUpdate()
    {
        
    }

    public void TileTick()
    {
        
    }

    public void TimeTick()
    {
        
    }
}
