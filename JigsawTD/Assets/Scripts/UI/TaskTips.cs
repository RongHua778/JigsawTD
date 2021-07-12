using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTips : TileTips
{
    public void SetInfo(Task task)
    {
        Description.text = "�����غ���: "+task.Periods.ToString();
        Name.text = task.Difficulty.ToString();
        Icon.sprite = task.Icon;
    }
}
