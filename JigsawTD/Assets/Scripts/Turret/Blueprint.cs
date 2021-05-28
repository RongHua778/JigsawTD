using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public TurretAttribute CompositeTurretAttribute;
    public bool CanBuild => CheckBuildable();
    List<Composition> compositions = new List<Composition>();
    public List<Composition> Compositions { get => compositions; set => compositions = value; }

    public float CompositeAttackDamage;
    public float CompositeAttackSpeed;
    public float CompositeSlowRate;
    public float CompositeCriticalRate;
    public float CompositeSputteringRange;

    public void SetCompositeValues()
    {
        CompositeAttackDamage = CompositeAttackSpeed = CompositeSlowRate = CompositeCriticalRate = CompositeSputteringRange = 0;
        foreach (Composition com in Compositions)
        {
            switch ((Element)com.elementRequirement)
            {
                case Element.Gold:
                    CompositeAttackDamage += StaticData.Instance.GoldAttackIntensify * com.levelRequirement;
                    break;
                case Element.Wood:
                    CompositeAttackSpeed += StaticData.Instance.WoodSpeedIntensify * com.levelRequirement;
                    break;
                case Element.Water:
                    CompositeSlowRate += StaticData.Instance.WaterSlowIntensify * com.levelRequirement;
                    break;
                case Element.Fire:
                    CompositeCriticalRate += StaticData.Instance.FireCriticalIntensify * com.levelRequirement;
                    break;
                case Element.Dust:
                    CompositeSputteringRange += StaticData.Instance.DustSputteringIntensify * com.levelRequirement;
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
            GameEvents.Instance.RemoveGameTile(com.turretTile);
        }
    }

    //检测每个配方是否存在在场上的方法
    public void CheckElement()
    {
        List<GameBehavior> temp = GameManager.Instance.turrets.behaviors;
        for (int i = 0; i < compositions.Count; i++)
        {
            compositions[i].obtained = false;
            for (int j = 0; j < temp.Count; j++)
            {
                Turret turret = temp[j] as Turret;
                if (compositions[i].elementRequirement == (int)turret.Element &&
                    compositions[i].levelRequirement == turret.Quality)
                {
                    compositions[i].obtained = true;
                    compositions[i].turretTile = turret.m_TurretTile;
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
