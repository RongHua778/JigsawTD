using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretInfo
{
    public int AttackRange;
    public int ForbidRange;
    public float AttackDamage;
    public float AttackSpeed;
    public float BulletSpeed;
    public float SputteringRange;
    public float CriticalRate;
    public List<AttackEffectInfo> AttackEffects = new List<AttackEffectInfo>();
    public List<PoloEffect> PoloEffects = new List<PoloEffect>();
}

[System.Serializable]
public class PoloEffect
{
    public PoloEffectType EffectType;
    public float KeyValue;
}

public enum PoloEffectType
{
    RangeIntensify,AttackIntensify
}
public enum RangeType
{
    Circle, HalfCircle, Line
}

[CreateAssetMenu(menuName = "Attribute/TurretAttribute", fileName = "TurretAttribute")]
public class TurretAttribute : LevelAttribute
{
    public RangeType RangeType;
    public List<TurretInfo> TurretLevels = new List<TurretInfo>();

    public override void Upgrade()
    {
        base.Upgrade();
    }



}