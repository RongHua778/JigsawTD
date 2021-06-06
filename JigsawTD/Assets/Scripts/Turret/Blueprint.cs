using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Blueprint
{
    public TurretAttribute CompositeTurretAttribute;
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
                    CompositeAttackDamage += StaticData.GoldAttackIntensify * com.levelRequirement;
                    break;
                case Element.Wood:
                    CompositeAttackSpeed += StaticData.WoodSpeedIntensify * com.levelRequirement;
                    break;
                case Element.Water:
                    CompositeSlowRate += StaticData.WaterSlowIntensify * com.levelRequirement;
                    break;
                case Element.Fire:
                    CompositeCriticalRate += StaticData.FireCriticalIntensify * com.levelRequirement;
                    break;
                case Element.Dust:
                    CompositeSputteringRange += StaticData.DustSputteringIntensify * com.levelRequirement;
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
            ObjectPool.Instance.UnSpawn(com.turretTile.gameObject);
        }
    }

    //检测每个配方是否存在在场上的方法
    public void CheckElement()
    {
        List<IGameBehavior> temp = GameManager.Instance.turrets.behaviors.ToList();
        
        for (int i = 0; i < compositions.Count; i++)
        {
            compositions[i].obtained = false;
            for (int j = 0; j < temp.Count; j++)
            {
                ElementTurret turret = temp[j] as ElementTurret;
                if (compositions[i].elementRequirement == (int)turret.Element &&
                    compositions[i].levelRequirement == turret.Quality)
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
