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
    public List<TurretEffectInfo> AttackEffects = new List<TurretEffectInfo>();
    public List<PoloEffect> PoloEffects = new List<PoloEffect>();
    [Header("美术资源设置")]
    public string TurretName;
    public Sprite Icon;
    public Sprite BaseSprite;
    public Sprite CannonSprite;
    public Vector2 ShootPointOffset;
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
    public Element element;
    public int totalLevel;
    public int elementNumber;
    public List<TurretInfo> TurretLevels = new List<TurretInfo>();
    public override void Upgrade()
    {
        base.Upgrade();
    }



}
