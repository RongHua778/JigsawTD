using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ElementSet : SetHolder
{

    [SerializeField] TipsElementConstruct[] elementConstructs = default;

    private List<int> skillPreviewElements = new List<int>();


    public override int SelectedCount
    {
        get => base.SelectedCount;
        set
        {
            base.SelectedCount = value;
            if (SelectedCount < MaxSelectCount)
            {
                HideAllSkills();
            }
            else
            {
                SetSkillPreview();
            }
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


    public void InitializeSlots()
    {
        SelectedCount = MaxSelectCount;
       // SaveList = Game.Instance.SaveData.SaveSelectedElement;

        for (int i = 0; i < SaveList.Count; i++)
        {
            turretSlots[i].Initialize(this, SaveList[i]);
        }

        SetSkillPreview();
    }

    private void SetSkillPreview()
    {
        skillPreviewElements.Clear();
        for (int i = 0; i < turretSlots.Length; i++)
        {
            if (turretSlots[i].IsSelect)
                skillPreviewElements.Add(i);
        }

        List<List<int>> skills = StaticData.GetAllCC2(skillPreviewElements);
        for (int i = 0; i < skills.Count; i++)
        {
            ElementSkill skill = TurretEffectFactory.GetElementSkill(skills[i]);
            elementConstructs[i].SetElements(skill);
        }
        StartCoroutine(ShowSkills(true));
    }



}
