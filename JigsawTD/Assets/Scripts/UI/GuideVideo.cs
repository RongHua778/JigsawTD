using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideVideo :IUserInterface
{
    [SerializeField] Toggle[] tabs = default;

    public void ShowPage(int index)
    {
        tabs[index].gameObject.SetActive(true);
        tabs[index].isOn = true;
    }

}
