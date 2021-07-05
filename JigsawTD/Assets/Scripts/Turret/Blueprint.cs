using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Blueprint
{
    public TurretAttribute CompositeTurretAttribute;
    public StrategyComposite ComStrategy;
    List<Composition> compositions = new List<Composition>();
    public List<Composition> Compositions { get => compositions; set => compositions = value; }

    public float CompositeAttackDamage;
    public float CompositeAttackSpeed;
    public float CompositeSlowRate;
    public float CompositeCriticalRate;
    public float CompositeSputteringRange;

    public void SetBluePrintIntensify()
    {
        CompositeAttackDamage = CompositeAttackSpeed = CompositeSlowRate = CompositeCriticalRate = CompositeSputteringRange = 0;
        foreach (Composition com in Compositions)
        {
            switch ((Element)com.elementRequirement)
            {
                case Element.Gold:
                    //CompositeAttackDamage += StaticData.GoldAttackIntensify * com.qualityRequeirement;
                    CompositeAttackDamage += StaticData.GoldAttackIntensify;
                    break;
                case Element.Wood:
                    CompositeAttackSpeed += StaticData.WoodSpeedIntensify;
                    break;
                case Element.Water:
                    CompositeSlowRate += StaticData.WaterSlowIntensify;
                    break;
                case Element.Fire:
                    CompositeCriticalRate += StaticData.FireCriticalIntensify;
                    break;
                case Element.Dust:
                    CompositeSputteringRange += StaticData.DustSputteringIntensify;
                    break;
                default:
                    break;
            }
        }
    }

    public void BuildBluePrint()
    {
        //����ϳ������Ƴ������䷽
        foreach (Composition com in Compositions)
        {
            if (!com.isPerfect)
                ObjectPool.Instance.UnSpawn(com.turretTile);//ͬʱ�Ƴ��÷������Ĺ⻷Ч��
            else
                GameManager.Instance.GetPerfectElement(-1);
        }

        //���з��������¼�����Ч��
        GameManager.Instance.CheckDetectSkill();

    }

    //���ÿ���䷽�Ƿ�����ڳ��ϵķ���
    public void CheckElement()
    {
        List<IGameBehavior> temp = GameManager.Instance.elementTurrets.behaviors.ToList();
        int perfectCount = StaticData.PerfectElementCount;
        for (int i = 0; i < compositions.Count; i++)
        {
            compositions[i].obtained = false;
            compositions[i].isPerfect = false;
            for (int j = 0; j < temp.Count; j++)
            {
                ElementTurret turret = temp[j] as ElementTurret;
                StrategyElement strategy = turret.Strategy as StrategyElement;
                if (compositions[i].elementRequirement == (int)(strategy.Element) &&
                    compositions[i].qualityRequeirement == strategy.Quality)
                {
                    compositions[i].obtained = true;
                    compositions[i].turretTile = turret.m_GameTile;
                    temp.Remove(temp[j]);
                    break;
                }
            }
            if (perfectCount > 0 && !compositions[i].obtained)
            {
                compositions[i].obtained = true;
                compositions[i].isPerfect = true;
                perfectCount--;
            }
        }
    }


    //����Ƿ���������Խ�����䷽����
    public bool CheckBuildable()
    {
        bool result = true;
        for (int i = 0; i < compositions.Count; i++)
        {
            result = result && compositions[i].obtained;
        }
        return result;
    }
}
