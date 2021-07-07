using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier : Enemy
{
    [SerializeField]protected float armorIntensify;
    float armor;
    protected List<Aircraft> aircrafts=new List<Aircraft>();
    public override EnemyType EnemyType => EnemyType.AircraftCarrier;
    public float Armor { get => armor; set => armor = value; }

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify);
        Armor = intensify * armorIntensify;
        EnemySkills = new List<Skill>();
        EnemySkills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Aircraft, this));
    }

    public virtual void AddAircraft(Aircraft a)
    {
        aircrafts.Add(a);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        for (int i = 0; i < aircrafts.Count; i++)
        {
            aircrafts[i].CurrentHealth = 0;
        }
        aircrafts.Clear();
    }
}
