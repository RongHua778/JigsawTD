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
    [Header("������Դ����")]
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
    [Header("��������")]
    public StrategyType StrategyType;
    public RangeType RangeType;
    public ElementType element;
    public Bullet Bullet;
    public float BulletSpeed;
    public AudioClip ShootSound;

    [Header("�ϳ�������")]
    public int Rare;//ϡ�ж�
    public int totalLevel;
    public int elementNumber;
    public int maxElementLevel;//�䷽Ԫ�����ȼ�
    public TurretSkillName TurretSkill;
    public List<TurretInfo> TurretLevels = new List<TurretInfo>();

    public override void MenuShowTips(Vector2 pos)
    {
        base.MenuShowTips(pos);
        MenuUIManager.Instance.ShowTurretTips(this, pos);
    }

}
