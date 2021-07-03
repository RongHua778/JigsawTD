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
                    CompositeAttackDamage += StaticData.GoldAttackIntensify * com.qualityRequeirement;
                    break;
                case Element.Wood:
                    CompositeAttackSpeed += StaticData.WoodSpeedIntensify * com.qualityRequeirement;
                    break;
                case Element.Water:
                    CompositeSlowRate += StaticData.WaterSlowIntensify * com.qualityRequeirement;
                    break;
                case Element.Fire:
                    CompositeCriticalRate += StaticData.FireCriticalIntensify * com.qualityRequeirement;
                    break;
                case Element.Dust:
                    CompositeSputteringRange += StaticData.DustSputteringIntensify * com.qualityRequeirement;
                    break;
                default:
                    break;
            }
        }
    }

    public void BuildBluePrint()
    {
        //建造合成塔，移除所有配方
        foreach(Composition com in Compositions)
        {
            ObjectPool.Instance.UnSpawn(com.turretTile);
        }
    }

    //检测每个配方是否存在在场上的方法
    public void CheckElement()
    {
        List<IGameBehavior> temp = GameManager.Instance.elementTurrets.behaviors.ToList();
        
        for (int i = 0; i < compositions.Count; i++)
        {
            compositions[i].obtained = false;
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
        }
    }

    //检查是否已满足可以建造的配方条件
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
