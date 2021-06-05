using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttTips : TileTips
{
    [SerializeField] Text RangeTypeValue = default;
    [SerializeField] Text AttackValue = default;
    [SerializeField] Text SpeedValue = default;
    [SerializeField] Text RangeValue = default;
    [SerializeField] Text CriticalValue = default;
    [SerializeField] Text SputteringValue = default;
    [SerializeField] Text SlowRateValue = default;
    [SerializeField] Text IntensifyValue = default;
    [SerializeField] GameObject BtnArea = default;//购买/合成按钮区
    [SerializeField] GameObject BuyBtn = default;//购买按钮
    [SerializeField] GameObject IntensifyArea = default;//元素塔加成效果区
    [SerializeField] TipsElementConstruct elementConstruct = default;//合成塔组成元素区

    public void ReadAttribute(TurretAttribute attribute)
    {

    }

    //public void ReadAttribute(BluePrintGrid bGrid)//通过配方查看
    //{
    //    m_Turret = null;
    //    m_BGrid = bGrid;
    //    TurretAttribute attribute = bGrid.BluePrint.CompositeTurretAttribute;
    //    Icon.sprite = attribute.TurretLevels[0].Icon;
    //    Name.text = attribute.TurretLevels[0].TurretName;

    //    SetRangeType(attribute);

    //    string damageIntensify = bGrid.BluePrint.CompositeAttackDamage <= 0 ? "" : "<color=#00ffffff> +" + (attribute.TurretLevels[0].AttackDamage * bGrid.BluePrint.CompositeAttackDamage).ToString() + "</color>";
    //    AttackValue.text = attribute.TurretLevels[0].AttackDamage.ToString() + damageIntensify;

    //    string speedIntensify = bGrid.BluePrint.CompositeAttackSpeed <= 0 ? "" : "<color=#00ffffff> +" + (attribute.TurretLevels[0].AttackSpeed * bGrid.BluePrint.CompositeAttackSpeed).ToString() + "</color>";
    //    SpeedValue.text = attribute.TurretLevels[0].AttackSpeed.ToString() + speedIntensify;

    //    RangeValue.text = attribute.TurretLevels[0].AttackRange.ToString();

    //    string criticalIntensify = bGrid.BluePrint.CompositeCriticalRate <= 0 ? "" : "<color=#00ffffff> +" + (bGrid.BluePrint.CompositeCriticalRate * 100).ToString() + "</color>";
    //    CriticalValue.text = (attribute.TurretLevels[0].CriticalRate * 100).ToString() + criticalIntensify + "%";

    //    string sputteringIntensify = bGrid.BluePrint.CompositeSputteringRange <= 0 ? "" : "<color=#00ffffff> +" + bGrid.BluePrint.CompositeSputteringRange.ToString() + "</color>";
    //    SputteringValue.text = attribute.TurretLevels[0].SputteringRange.ToString() + sputteringIntensify;

    //    string slowIntensify = bGrid.BluePrint.CompositeSlowRate <= 0 ? "" : "<color=#00ffffff> +" + bGrid.BluePrint.CompositeSlowRate.ToString() + "</color>";
    //    SlowRateValue.text = attribute.TurretLevels[0].SlowRate.ToString() + slowIntensify;

    //    if (attribute.TurretLevels[0].TurretEffects.Count > 0)
    //    {
    //        string finalDes = "";
    //        if (attribute.Description != "")
    //            finalDes += attribute.Description + "\n";
    //        foreach (TurretEffectInfo effect in attribute.TurretLevels[0].TurretEffects)
    //        {
    //            finalDes += effect.EffectDescription;
    //            finalDes += "\n";
    //        }
    //        Description.text = finalDes;
    //    }
    //    else
    //    {
    //        Description.text = attribute.Description;
    //    }
    //    elementConstruct.gameObject.SetActive(true);
    //    elementConstruct.SetElements(bGrid.BluePrint);
    //    IntensifyArea.SetActive(false);

    //    //关闭购买按钮，如果是在口袋里
    //    BuyBtn.SetActive(bGrid.InShop);
    //    BtnArea.SetActive(true);
    //    AnalysisArea.SetActive(false);
    //    UpgradeArea.SetActive(false);
    //}
    public void BuyBluePrintBtnClick()
    {
        //if (LevelUIManager.Instance.ConsumeMoney(StaticData.BuyBluePrintCost))
        //{
        //    LevelUIManager.Instance.LuckyPoints++;
        //    BuyBtn.SetActive(false);
        //    m_BGrid.MoveToPocket();
        //}
        //else
        //{
        //    GameEvents.Instance.Message("金币不足");
        //}
    }

    public void CompositeBtnClick()
    {
        //if (ShapeSelectUI.BuildingState != BuiidingState.Default)
        //{
        //    GameEvents.Instance.Message("需先放置抽取模块");
        //    return;
        //}
        //if (GameManager.Instance.OperationState.StateName != StateName.BuildingState)
        //{
        //    GameEvents.Instance.Message("战斗中不可合成");
        //    return;
        //}
        //if (!m_BGrid.BuildAble)
        //{
        //    GameEvents.Instance.Message("缺少必要素材");
        //    return;
        //}
        ////LevelUIManager.Instance.HideArea();
        //m_BGrid.Shop.CompositeBluePrint(m_BGrid);
    }

}
