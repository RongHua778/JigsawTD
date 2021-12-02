using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Linq;
using System;

[CreateAssetMenu(menuName = "Dialogue/GuideDialogue", fileName = "NewGuideDialogue")]
[InlineEditor]
public class DialogueData:SerializedScriptableObject
{
    public TutorialType TriggerType;
    [TypeFilter("GetGuideConditionList")]
    public GuideCondition[] GuideConditions;
    //[InfoBox("$GuideNote")]
    string GuideNote => PreviewWords();
    [ValidateInput("AlwaysFalse", "$GuideNote", InfoMessageType.Info)]
    public string[] Words;//对话内容
    public bool DontNeedClickEnd;//是否自动结束并解锁操作
    public float WaitingTime;//教程的等待时间


    [TypeFilter("GetGuideEventList")]
    public GuideEvent[] GuideStartEvents;

    [TypeFilter("GetGuideEventList")]
    public GuideEvent[] GuideEndEvents;

    public IEnumerable<Type> GetGuideEventList()
    {
        var q = typeof(GuideEvent).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition)                             // Excludes C1<>
            .Where(x => typeof(GuideEvent).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass

        return q;
    }

    public IEnumerable<Type> GetGuideConditionList()
    {
        var q = typeof(GuideCondition).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition)                             // Excludes C1<>
            .Where(x => typeof(GuideCondition).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass

        return q;
    }

    public bool JudgeConditions(TutorialType tutorialType)
    {
        if (tutorialType != TriggerType)
            return false;
        foreach (var item in GuideConditions)
        {
            if (!item.Judge())
            {
                return false;
            }
        }
        return true;
    }

    public void TriggerGuideStartEvents()
    {
        foreach (var item in GuideStartEvents)
        {
            item.Trigger();
        }
    }

    public void TriggerGuideEndEvents()
    {
        foreach (var item in GuideEndEvents)
        {
            item.Trigger();
        }
    }

    private bool AlwaysFalse()
    {
        return false;
    }

    private string PreviewWords()
    {
        if (GameMultiLang.Fields != null)
        {
            string txt="";
            foreach(string key in Words)
            {
                txt += GameMultiLang.GetTraduction(key) + "\n";
            }
            return txt;
        }
        else
            return "";
    }


}
