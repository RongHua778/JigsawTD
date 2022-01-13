using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Steamworks;

public struct ModeData
{
    public int ModeID;
    //输出最高重构塔前1+第一名防御塔所用词条
    public int Wave;
    //3-6条数据
    public StrategyBase HighestTurretStrategy;
    public string HighestTurretName;
    public int HighestTurretDamage;
    public List<string> HighestTurretSkills;
    //首次通关前失败次数
    public int BeforeLost;
}

public class UnityAnalystics
{
    //在特定模式胜利前一共输了多少把
    //public static void AddModeWinCount(bool win, int modeID, int count = 1)
    //{
    //    if (!win)//输了
    //    {
    //        int final = PlayerPrefs.GetInt("ModeLost" + modeID, 0) + count;
    //        PlayerPrefs.SetInt("ModeLost" + modeID, final);

    //        //上传一次模式失败
    //        AnalyticsResult result = Analytics.CustomEvent("ModeLost" + modeID);
    //        Debug.Log("ModeLost-Upload:" + result + " ModeID:" + modeID);
    //    }
    //    else
    //    {
    //        int final = PlayerPrefs.GetInt("ModeWin" + modeID, 0) + count;
    //        if (final > 0 && final < 2)//该模式第一次获胜,上传数据
    //        {
    //            var dic = new Dictionary<string, object>();
    //            int value = PlayerPrefs.GetInt("ModeLost" + modeID, 0);
    //            dic["ModeID=" + modeID] = value;
    //            AnalyticsResult result = Analytics.CustomEvent("ModeLostBeforeWin", dic);
    //            Debug.Log("ModeLostBeforeWin-Upload:" + result + " Value:" + value);
    //        }
    //        PlayerPrefs.SetInt("ModeWin" + modeID, final);

    //        //上传一次模式胜利
    //        AnalyticsResult result2 = Analytics.CustomEvent("ModeWin" + modeID);
    //        Debug.Log("ModeWin-Upload:" + result2 + " ModeID:" + modeID);
    //    }
    //}

    public static void EnableAnalysitics()
    {
        Analytics.enabled = true;
    }

    public static void UploadModeData(ModeData modeData)
    {
        try
        {
            var dic = new Dictionary<string, object>();
            dic["UserName"] = SteamFriends.GetPersonaName();
            dic["Wave"] = modeData.Wave;
            if (modeData.BeforeLost != 999)//如果是首次胜利，上传该数据
                dic["BeforeLost"] = modeData.BeforeLost;

            if (modeData.HighestTurretStrategy != null)
            {
                dic[modeData.HighestTurretStrategy.Attribute.Name] = modeData.HighestTurretStrategy.TotalDamage;
                //第一名防御塔所用技能

                for (int i = 1; i < modeData.HighestTurretStrategy.TurretSkills.Count; i++)
                {
                    ElementSkill skill = modeData.HighestTurretStrategy.TurretSkills[i] as ElementSkill;
                    string key = "";
                    foreach (var item in skill.Elements)
                    {
                        key += item;
                    }
                    dic[key] = 1;
                }

            }
            AnalyticsResult result = Analytics.CustomEvent("Mode" + modeData.ModeID, dic);
            Debug.Log("ModeData-Upload:" + result + "ModeID:" + modeData.ModeID);
        }
        catch
        {
            Debug.Log("Uploaddata Unsucessful");
        }

    }

    //合成特定技能的次数，检测使用频率
    //public static void AddSkillCount(List<int> elements)
    //{
    //    string key = "";
    //    foreach (var item in elements)
    //    {
    //        key += item;
    //    }
    //    AnalyticsResult result = Analytics.CustomEvent(key);
    //    Debug.Log("Skill-Upload:" + result + " Skill:" + key);
    //}

    ////合成塔落地次数
    //public static void AddRefactorTurretCount(string turret)
    //{
    //    AnalyticsResult result = Analytics.CustomEvent(turret);
    //    Debug.Log("Turret-Upload:" + result + " Turret:" + turret);
    //}


}
