using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : ReusableObject
{
    [SerializeField] Image healthProgress = default;
    [SerializeField] Image frostProgess = default;

    [SerializeField] Vector2 enemyOffset = default;
    [SerializeField] Vector2 iconStartPos = default;
    [SerializeField] Vector2 iconOffset = default;

    private List<GameObject> iconShowList = new List<GameObject>();

    [SerializeField] GameObject[] Icons = default;//1=SLOW,2=DAMAGEMARK,3=SLOWINTENSIFY,4=GOLD
    public Transform followTrans;

    float fillAmount;
    public float FillAmount
    {
        get => fillAmount;
        set
        {
            fillAmount = value;
            healthProgress.fillAmount = value;
        }
    }

    float frostAmount;
    public float FrostAmount
    {
        get => frostAmount;
        set
        {
            frostAmount = value;
            frostProgess.fillAmount = value;
        }
    }

    public override void OnUnSpawn()
    {
        FillAmount = 1;
        FrostAmount = 0;
        iconShowList.Clear();
    }

    public void ShowIcon(int id, bool value)
    {
        Icons[id].SetActive(value);
        if (value)
        {
            if (!iconShowList.Contains(Icons[id]))
                iconShowList.Add(Icons[id]);
        }
        else
            iconShowList.Remove(Icons[id]);
        SortIcons();
    }

    private void SortIcons()
    {
        Vector2 offset = iconOffset;
        for (int i = 0; i < iconShowList.Count; i++)
        {
            iconShowList[i].transform.localPosition = iconStartPos + offset;
            offset += iconOffset;
        }

    }

    private void LateUpdate()
    {
        //if (followTrans != null)
        //    transform.position = (Vector2)followTrans.position + enemyOffset;
        transform.position = (Vector2)transform.parent.position + enemyOffset;
        transform.rotation = Quaternion.identity;

    }
}
