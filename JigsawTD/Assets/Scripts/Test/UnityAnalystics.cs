using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Steamworks;

public struct ModeData
{
    public int ModeID;
    //�������ع���ǰ1+��һ�����������ô���
    public int Wave;
    //3-6������
    public StrategyBase HighestTurretStrategy;
    public string HighestTurretName;
    public int HighestTurretDamage;
    public List<string> HighestTurretSkills;
    //�״�ͨ��ǰʧ�ܴ���
    public int BeforeLost;
}

public class UnityAnalystics
{
    //���ض�ģʽʤ��ǰһ�����˶��ٰ�
    //public static void AddModeWinCount(bool win, int modeID, int count = 1)
    //{
    //    if (!win)//����
    //    {
    //        int final = PlayerPrefs.GetInt("ModeLost" + modeID, 0) + count;
    //        PlayerPrefs.SetInt("ModeLost" + modeID, final);

    //        //�ϴ�һ��ģʽʧ��
    //        AnalyticsResult result = Analytics.CustomEvent("ModeLost" + modeID);
    //        Debug.Log("ModeLost-Upload:" + result + " ModeID:" + modeID);
    //    }
    //    else
    //    {
    //        int final = PlayerPrefs.GetInt("ModeWin" + modeID, 0) + count;
    //        if (final > 0 && final < 2)//��ģʽ��һ�λ�ʤ,�ϴ�����
    //        {
    //            var dic = new Dictionary<string, object>();
    //            int value = PlayerPrefs.GetInt("ModeLost" + modeID, 0);
    //            dic["ModeID=" + modeID] = value;
    //            AnalyticsResult result = Analytics.CustomEvent("ModeLostBeforeWin", dic);
    //            Debug.Log("ModeLostBeforeWin-Upload:" + result + " Value:" + value);
    //        }
    //        PlayerPrefs.SetInt("ModeWin" + modeID, final);

    //        //�ϴ�һ��ģʽʤ��
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
            if (modeData.BeforeLost != 999)//������״�ʤ�����ϴ�������
                dic["BeforeLost"] = modeData.BeforeLost;

            if (modeData.HighestTurretStrategy != null)
            {
                dic[modeData.HighestTurretStrategy.Attribute.Name] = modeData.HighestTurretStrategy.TotalDamage;
                //��һ�����������ü���

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

    //�ϳ��ض����ܵĴ��������ʹ��Ƶ��
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

    ////�ϳ�����ش���
    //public static void AddRefactorTurretCount(string turret)
    //{
    //    AnalyticsResult result = Analytics.CustomEvent(turret);
    //    Debug.Log("Turret-Upload:" + result + " Turret:" + turret);
    //}


}
