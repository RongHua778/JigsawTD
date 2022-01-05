using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Binary : Boss
{

    public static Binary FirstBinary;
    private Binary m_brother;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset,float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify);
        if (FirstBinary == null)
        {
            SpeedIntensify = 0;
            FirstBinary = this;
            m_brother = GameManager.Instance.SpawnEnemy(EnemyType.Binary, 0, Intensify) as Binary;
            m_brother.m_brother = this;
            FirstBinary = null;
            m_brother.SpeedIntensify = 0;
        }
    }


    protected override void OnDie()
    {
        base.OnDie();
        if (m_brother != null)
        {
            m_brother.SpeedIntensify += 1f;
            m_brother.ProgressFactor = m_brother.Speed * m_brother.Adjust;
            m_brother.m_brother = null;
            m_brother = null;
        }

    }




}
