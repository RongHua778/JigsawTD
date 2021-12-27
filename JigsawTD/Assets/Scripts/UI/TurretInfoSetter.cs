using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretInfoSetter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite[] rareSprite = default;
    [SerializeField] Image rareIcon = default;
    [SerializeField] GameObject[] levelIcons = default;
    [SerializeField] InfoBtn rareInfo = default;

    private void Start()
    {
        //rareInfo.SetContent(GameMultiLang.GetTraduction("RARE"));
    }
    public void SetRare(int quality)
    {
        rareIcon.sprite = rareSprite[quality - 1];
    }

    public void SetLevel(int level)
    {
        foreach (var obj in levelIcons)
        {
            obj.SetActive(false);
        }
        for (int i = 0; i < level; i++)
        {
            levelIcons[i].SetActive(true);
        }
    }
}
