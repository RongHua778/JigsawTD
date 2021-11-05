using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/EnemyAttribute", fileName = "EnemyAttribute")]
public class EnemyAttribute : ContentAttribute
{
    public EnemyType EnemyType;
    public int InitCount;
    public float CountIncrease;
    public float Health;
    public float Speed;
    public float CoolDown;
    public int ReachDamage;
    public string BackGround;

    public override void MenuShowTips(Vector2 pos)
    {
        base.MenuShowTips(pos);
        MenuUIManager.Instance.ShowEnemyInfoTips(this, pos);
    }
}
