using UnityEditor;
using UnityEngine;

public enum Quality
{
    level1, Level2, level3, Level4, level5
}

public class TestWindow : EditorWindow
{
    string moneyGet = "10000";
    //string element = "0";
    Quality quality = Quality.level1;
    Element element = Element.Gold;

    string turretName = "F1";
    Element e1 = Element.Gold;
    Element e2 = Element.Gold;
    Element e3 = Element.Gold;

    string trapName = "BLINKTRAP";
    [MenuItem("Window/TestWindow")]
    public static void ShowWindow()
    {
        GetWindow<TestWindow>("TestWindow");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("金钱");
        moneyGet = EditorGUILayout.TextField("", moneyGet, GUILayout.Width(80));
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            GameManager.Instance.GainMoney(int.Parse(moneyGet));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("元素");
        element = (Element)EditorGUILayout.EnumPopup("", element, GUILayout.Width(60));
        GUILayout.Label("品质");
        quality = (Quality)EditorGUILayout.EnumPopup("", quality, GUILayout.Width(60));
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            ConstructHelper.GetElementTurretByQualityAndElement(element, (int)quality + 1);
        }
        GUILayout.EndHorizontal();

        turretName = EditorGUILayout.TextField("合成塔", turretName);
        GUILayout.BeginHorizontal();
        GUILayout.Label("元素1");
        e1 = (Element)EditorGUILayout.EnumPopup("", e1, GUILayout.Width(60));
        GUILayout.Label("元素2");
        e2 = (Element)EditorGUILayout.EnumPopup("", e2, GUILayout.Width(60)); 
        GUILayout.Label("元素3");
        e3 = (Element)EditorGUILayout.EnumPopup("", e3, GUILayout.Width(60));
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            ConstructHelper.GetCompositeTurretByNameAndElement(turretName, (int)e1, (int)e2, (int)e3);

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        trapName= EditorGUILayout.TextField("陷阱", trapName);
        if (GUILayout.Button("获取", GUILayout.Width(120)))
        {
            ConstructHelper.GetTrapByName(trapName);
        }
        GUILayout.EndHorizontal();

    }
}
