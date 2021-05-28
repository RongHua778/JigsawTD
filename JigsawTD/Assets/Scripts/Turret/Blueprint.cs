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
                    CompositeAttackDamage += 0.1f * com.levelRequirement;
                    break;
                case Element.Wood:
                    CompositeAttackSpeed += 0.2f * com.levelRequirement;
                    break;
                case Element.Water:
                    CompositeSlowRate += 0.2f * com.levelRequirement;
                    break;
                case Element.Fire:
                    CompositeCriticalRate += 0.08f * com.levelRequirement;
                    break;
                case Element.Dust:
                    CompositeSputteringRange += 0.1f * com.levelRequirement;
                    break;
                default:
                    break;
            }
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
