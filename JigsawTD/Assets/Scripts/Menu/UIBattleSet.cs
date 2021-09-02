using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBattleSet : IUserInterface
{
    [SerializeField] TipsElementConstruct[] elementConstructs = default;
    [SerializeField] Text m_ElementCountTxt = default;
    [SerializeField] TurretAttribute[] ElementTurrets = default;
    [SerializeField] TurretAttribute[] CompositeTurrets = default;

    private int selectedElement;
    public int SelectedElement
    {
        get => selectedElement;
        set
        {
            selectedElement = Mathf.Clamp(value, 0, MaxSelectElement);
            if (selectedElement < MaxSelectElement)
            {
                HideAllSkills();
            }
            else
            {
                SetData();
                SetSkillPreview();
            }
            m_ElementCountTxt.text = GameMultiLang.GetTraduction("SELECTELEMENT") + selectedElement + "/" + MaxSelectElement;
        }
    }

    private void HideAllSkills()
    {
        foreach (var slot in elementConstructs)
        {
            slot.gameObject.SetActive(false);
            slot.transform.localScale = Vector2.one * 0.5f;
        }
    }

    private IEnumerator ShowSkills(bool value)
    {
        foreach (var slot in elementConstructs)
        {
            slot.gameObject.SetActive(value);
            slot.transform.DOScale(1f, 0.05f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private int maxSelectElement = 4;
    public int MaxSelectElement => maxSelectElement;

    [SerializeField] ElementSlot[] elementSlots = default;

    public override void Show()
    {
        base.Show();
        InitializeSlots();

    }
    private void InitializeSlots()
    {
        SelectedElement = 0;
        List<int> eElements = Game.Instance.SaveData.SaveSelectedElement;

        for (int i = 0; i < elementSlots.Length; i++)
        {
            elementSlots[i].Initialize(this, ElementTurrets[i]);
        }

        for (int i = 0; i < eElements.Count; i++)
        {
            elementSlots[i].IsSelect = true;//等于I就代表这个元素已选择
        }

        SetSkillPreview();
    }

    private void SetData()
    {
        Game.Instance.SaveData.SaveSelectedElement.Clear();

        for (int i = 0; i < elementSlots.Length; i++)
        {
            if (elementSlots[i].IsSelect)
            {
                Game.Instance.SaveData.SaveSelectedElement.Add(i);
            }
        }
    }

    public void OnConfirmClick()
    {
        if (SelectedElement < MaxSelectElement)
        {
            MenuUIManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOENOUGHELEMENT"));
            return;
        }
        else
        {
            SetData();
            Hide();
        }
    }

    private void SetSkillPreview()
    {
        List<List<int>> skills = StaticData.GetAllCC2(Game.Instance.SaveData.SaveSelectedElement);
        for (int i = 0; i < skills.Count; i++)
        {
            ElementSkill skill = TurretEffectFactory.GetElementSkill(skills[i]);
            elementConstructs[i].SetElements(skill);
        }
        StartCoroutine(ShowSkills(true));
    }



}
