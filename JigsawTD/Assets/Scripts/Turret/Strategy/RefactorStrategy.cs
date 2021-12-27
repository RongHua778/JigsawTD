using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RefactorStrategy : StrategyBase
{
    List<Composition> compositions = new List<Composition>();
    public List<Composition> Compositions { get => compositions; set => compositions = value; }//初始元素组合
    public RefactorStrategy(TurretAttribute attribute, int quality, List<Composition> initCompositions) : base(attribute, quality)
    {
        this.Compositions = initCompositions;
        SortCompositions();

        //初始技能
        GetTurretSkills();
        //配方自带元素技能
        GetFirstElementSkill();
    }

    private void GetFirstElementSkill()
    {
        List<int> elements = new List<int>();
        foreach (var com in Compositions)
        {
            elements.Add(com.elementRequirement);
        }
        ElementSkill effect = TurretEffectFactory.GetElementSkill(elements);
        AddElementSkill(effect);
    }

    public void SortCompositions()//重新排序
    {
        int temp;
        for (int i = 0; i < compositions.Count - 1; i++)
        {
            for (int j = 0; j < compositions.Count - 1 - i; j++)
            {
                if (compositions[j].elementRequirement > compositions[j + 1].elementRequirement)
                {
                    temp = compositions[j + 1].elementRequirement;
                    compositions[j + 1].elementRequirement = compositions[j].elementRequirement;
                    compositions[j].elementRequirement = temp;
                }
                else if (compositions[j].elementRequirement == compositions[j + 1].elementRequirement)
                {
                    if (compositions[j].qualityRequeirement > compositions[j + 1].qualityRequeirement)
                    {
                        temp = compositions[j + 1].qualityRequeirement;
                        compositions[j + 1].qualityRequeirement = compositions[j].qualityRequeirement;
                        compositions[j].qualityRequeirement = temp;
                    }
                }
            }
        }
    }

    public void CheckElement()
    {
        List<IGameBehavior> temp = GameManager.Instance.elementTurrets.behaviors.ToList();
        int perfectCount = GameRes.PerfectElementCount;
        for (int i = 0; i < compositions.Count; i++)
        {
            compositions[i].obtained = false;
            compositions[i].isPerfect = false;
            for (int j = 0; j < temp.Count; j++)
            {
                ElementTurret turret = temp[j] as ElementTurret;
                StrategyBase strategy = turret.Strategy;
                if (compositions[i].elementRequirement == (int)(strategy.Attribute.element) &&
                    compositions[i].qualityRequeirement == strategy.Quality)
                {
                    compositions[i].obtained = true;
                    compositions[i].turret = turret;
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

    public void RefactorTurret()
    {
        //建造合成塔，移除所有配方
        foreach (Composition com in Compositions)
        {
            if (!com.isPerfect)
                ObjectPool.Instance.UnSpawn(com.turret.m_GameTile);
            else
                GameRes.PerfectElementCount--;
        }

        //所有防御塔重新检查侦测效果
        GameManager.Instance.CheckDetectSkill();

    }

    public void PreviewElements(bool value)
    {
        foreach (var com in Compositions)
        {
            if (!com.isPerfect)
                com.turret.m_GameTile.Highlight(value);
        }
    }

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
