using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    int totalLevel;
    int compositionN;
    List<Composition> compositions = new List<Composition>();
    public List<Composition> Compositions { get => compositions; set => compositions = value; }
    public int CompositionN { get => compositionN; set => compositionN = value; }
    public int TotalLevel { get => totalLevel; set => totalLevel = value; }

    //检测每个配方是否存在在场上的方法
    public void CheckElement()
    {
        List<Turret> temp = GameManager.Instance.turretsElements;
        for (int i = 0; i < compositions.Count; i++)
        {
            for (int j = 0; j < temp.Count; j++)
            {
                if (compositions[i].elementRequirement == (int)temp[j].Element &&
                    compositions[i].levelRequirement == temp[j].Quality)
                {
                    compositions[i].obtained = true;
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
