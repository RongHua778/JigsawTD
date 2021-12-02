using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(menuName = "Attribute/EnemyAttribute", fileName = "EnemyAttribute")]
public class EnemyAttribute : ContentAttribute
{
    public EnemyType EnemyType;
    public bool IsBoss;
    public int InitCount;
    public float CountIncrease;
    public float Health;
    public float Speed;
    public float CoolDown;
    public string BackGround;
    [Header("Tips²ÎÊý")]
    public int HealthAtt;
    public int SpeedAtt;
    public int AmountAtt;
    public int ReachDamage;

    public override void MenuShowTips(Vector2 pos)
    {
        base.MenuShowTips(pos);
        MenuUIManager.Instance.ShowEnemyInfoTips(this, pos);
    }
}
