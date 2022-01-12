using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class UnityAnalystics : MonoBehaviour
{
    private void Start()
    {
        Analytics.EnableEvent("Good", true);
    }
    public void TestBtnClick()
    {
        var dic = new Dictionary<string, object>();
        dic["Good"] = 1;
        AnalyticsResult result = Analytics.CustomEvent("Good", dic);
        Debug.Log("TestResult:" + result);
    }
}
