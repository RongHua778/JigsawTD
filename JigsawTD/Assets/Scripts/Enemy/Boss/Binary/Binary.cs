using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binary : Enemy
{
    public static Binary FirstBinary;
    public Binary m_brother;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> path)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify, path);
        if (FirstBinary == null)
        {
            FirstBinary = this;
            this.PathOffset = - 0.25f;

            m_brother = GameManager.Instance.SpawnEnemy(EnemyType.Binary, 0, Intensify, 0.25f) as Binary;
            m_brother.m_brother = this;
            FirstBinary = null;
        }
    }


    protected override void OnDie()
    {
        if (m_brother != null)
        {
            m_brother.SpeedIntensify += 4f;
            m_brother.ProgressFactor = m_brother.Speed * m_brother.Adjust;
            m_brother.m_brother = null;
            m_brother = null;
        }

    }



}
