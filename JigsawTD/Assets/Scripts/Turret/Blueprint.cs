using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Blueprint
{
    public TurretAttribute CompositeTurretAttribute;
    public StrategyBase ComStrategy;
    List<Composition> compositions = new List<Composition>();
    public List<Composition> Compositions { get => compositions; set => compositions = value; }

    public void SortBluePrint()
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
        int perfectCount = GameRes.PerfectElementCount;
        for (int i = 0; i < compositions.Count; i++)
        {
            compositions[i].obtained = false;
            compositions[i].isPerfect = false;
            for (int j = 0; j < temp.Count; j++)
            {
                ElementTurret turret = temp[j] as ElementTurret;
                StrategyBase strategy = turret.Strategy;
                if (compositions[i].elementRequirement == (int)(strategy.m_Att.element) &&
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
