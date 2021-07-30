using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftCarrier : Enemy
{
    [SerializeField]protected float armorIntensify;
    float armor;
    List<Aircraft> aircrafts=new List<Aircraft>();
    public List<Aircraft> aircraftQueue=new List<Aircraft>();
    //float setQueueCD = 3f;
    //float setQueueCounter;
    public override EnemyType EnemyType => EnemyType.AircraftCarrier;
    public float Armor { get => armor; set => armor = value; }

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify);
        Armor = intensify * armorIntensify;
        Skills = new List<Skill>();
        Skills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Aircraft, this));
    }

    //public override bool GameUpdate()
    //{
    //    setQueueCounter += Time.deltaTime;
    //    if (setQueueCounter > setQueueCD)
    //    {
    //        setQueueCD = 0;
    //        SetQueue();
    //    }
    //    return base.GameUpdate();
    //}
    public virtual void AddAircraft(Aircraft a)
    {
        aircrafts.Add(a);
        if (!aircraftQueue.Contains(a))
        {
            aircraftQueue.Add(a);
        }
    }

    public void SetQueue()
    {
        for (int i = 0; i < aircraftQueue.Count; i++)
        {
            if (i == 0)
            {
                aircraftQueue[i].SetLeader();
            }
            else
            {
                aircraftQueue[i].SetPredecessor(aircraftQueue[i - 1]);
            }
        }
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
