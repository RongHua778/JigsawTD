using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBook : MonoBehaviour
{

    [SerializeField] GameObject[] pages = default;
    private int currentIndex = 0;

    public void ShowBook()
    {
        currentIndex = 0;
        ShowPage(currentIndex);
    }

    public void CloseBook()
    {
        this.gameObject.SetActive(false);
    }
    private void ShowPage(int index)
    {
        foreach (var page in pages)
        {
            page.SetActive(false);
        }
        pages[index].SetActive(true);
    }

    public void SideBtnClick(int value)
    {
        currentIndex += value;
        if (currentIndex < 0)
        {
            currentIndex = pages.Length - 1;
        }
        else if (currentIndex > pages.Length - 1)
        {
            currentIndex = 0;
        }
        ShowPage(currentIndex);
    }

}
