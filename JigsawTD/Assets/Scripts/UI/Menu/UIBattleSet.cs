using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBattleSet : IUserInterface
{
    [SerializeField] ElementSet m_ElementSet = default;
    [SerializeField] BluePrintPanel m_BlueprintPanel = default;

    public override void Show()
    {
        base.Show();
        m_ElementSet.InitializeSlots();
        m_BlueprintPanel.Initialize();
    }


    public void OnConfirmClick()
    {
        if (m_ElementSet.CheckSet()&&m_BlueprintPanel.CheckSets())
        {
            Hide();
        }
    }




}
