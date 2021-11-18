using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretItem : MonoBehaviour
{
    [SerializeField] Image icon = default;
    [SerializeField] Text nameTxt = default;
    [SerializeField] Text damageValue = default;
    public int TotalDamage;
    private TurretContent m_Turret;
    [SerializeField] Sprite[] medalSprites = default;
    [SerializeField] Image medalImg = default;
    [SerializeField] Text rankTxt = default;
    public void SetItemData(TurretContent turret)
    {
        m_Turret = turret;
        switch (m_Turret.Strategy.Attribute.StrategyType)
        {
            case StrategyType.Element:
                string element = StaticData.FormElementName(m_Turret.Strategy.Attribute.element, m_Turret.Strategy.Quality);
                nameTxt.text = element + " " + GameMultiLang.GetTraduction(m_Turret.Strategy.Attribute.Name);
                break;
            case StrategyType.Composite:
                nameTxt.text = GameMultiLang.GetTraduction(m_Turret.Strategy.Attribute.Name);
                break;
        }
        icon.sprite = turret.Strategy.Attribute.TurretLevels[m_Turret.Strategy.Quality-1].TurretIcon;
        damageValue.text = turret.Strategy.TotalDamage.ToString();
        TotalDamage = turret.Strategy.TotalDamage;
    }

    public void SetRank(int rank)
    {
        if (rank <= 2)
        {
            medalImg.gameObject.SetActive(true);
            medalImg.sprite = medalSprites[rank];
            rankTxt.gameObject.SetActive(false);
        }
        else
        {
            rankTxt.text = (rank + 1).ToString();
            rankTxt.gameObject.SetActive(true);
            medalImg.gameObject.SetActive(false);
        }
    }

    public void LocateTurret()
    {
        BoardSystem.SelectingTile = m_Turret.m_GameTile;
        GameManager.Instance.LocateCamPos(m_Turret.transform.position);
    }

}
