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
    public float SputteringRange;
    public float CriticalRate;
    public float SlowRate;
    [Header("美术资源设置")]
    public Sprite TurretIcon;
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
    RangeIntensify, AttackIntensify
}
public enum RangeType
{
    Circle, HalfCircle, Line
}

[CreateAssetMenu(menuName = "Attribute/TurretAttribute", fileName = "TurretAttribute")]
public class TurretAttribute : ContentAttribute
{
    [Header("基础参数")]
    public StrategyType StrategyType;
    public RangeType RangeType;
    public ElementType element;
    public Bullet Bullet;
    public float BulletSpeed;
    public AudioClip ShootSound;

    [Header("合成塔参数")]
    public int Rare;//稀有度
    public int totalLevel;
    public int elementNumber;
    public int maxElementLevel;//配方元素最大等级
    public TurretSkillName TurretSkill;
    public List<TurretInfo> TurretLevels = new List<TurretInfo>();

    public override void MenuShowTips(Vector2 pos)
    {
        base.MenuShowTips(pos);
        MenuUIManager.Instance.ShowTurretTips(this, pos);
    }

}
