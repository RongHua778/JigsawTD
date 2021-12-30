using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UITujian : IUserInterface
{
    private Animator m_Anim;
    [SerializeField] TipsElementConstruct[] elementConstructs = default;
    List<int> skillPreviewElements;
    [SerializeField] GameLevelHolder gameLevelPrefab = default;
    private List<ItemSlot> items;
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
        skillPreviewElements = new List<int> { 0, 1, 2, 3, 4 };
        SetElementSkills();
        //items = GetComponentsInChildren<ItemSlot>().ToList();
        //gameLevelPrefab.SetData();
    }

    public void SetElementSkills()
    {
        List<List<int>> skills = StaticData.GetAllCC2(skillPreviewElements);
        for (int i = 0; i < skills.Count; i++)
        {
            ElementSkill skill = TurretEffectFactory.GetElementSkill(skills[i]);
            TurretAttribute attribute = StaticData.Instance.ContentFactory.GetElementAttribute(ElementType.GOLD);
            skill.strategy = new StrategyBase(attribute, 1);
            skill.strategy.BaseGoldCount++;
            skill.strategy.BaseWoodCount++;
            skill.strategy.BaseWaterCount++;
            skill.strategy.BaseFireCount++;
            skill.strategy.BaseDustCount++;

            elementConstructs[i].SetElements(skill);
        }
    }

    public override void Show()
    {
        if (!Game.Instance.TestMode)
        {
            MenuUIManager.Instance.ShowMessage(GameMultiLang.GetTraduction("TEST3"));
            return;
        }
        base.Show();
        m_Anim.SetBool("OpenLevel", true);
        gameLevelPrefab.SetData();
        if (items == null)
            items = GetComponentsInChildren<ItemSlot>().ToList();
        foreach (var item in items)
        {
            item.SetContent();
        }
    }

    public override void Hide()
    {
        base.Hide();
        MenuUIManager.Instance.HideTips();
    }

    public void ClosePanel()
    {
        m_Anim.SetBool("OpenLevel", false);
    }
}
