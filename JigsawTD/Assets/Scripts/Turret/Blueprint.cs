using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public TurretAttribute CompositeTurretAttribute;
    //int totalLevel;
    //int compositionN;
    public bool CanBuild => CheckBuildable();
    List<Composition> compositions = new List<Composition>();
    public List<Composition> Compositions { get => compositions; set => compositions = value; }
    //public int CompositionN { get => compositionN; set => compositionN = value; }
    //public int TotalLevel { get => totalLevel; set => totalLevel = value; }

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
