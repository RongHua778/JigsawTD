using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UITujian : IUserInterface
{
    private Animator m_Anim;
    [SerializeField] GameLevelHolder gameLevelPrefab = default;
    [SerializeField] UITujian_ListHolder[] listHolders = default;

    [SerializeField] TipsElementConstruct[] elementConstructs = default;
    List<int> skillPreviewElements;
    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();

        foreach (var item in listHolders)
        {
            item.SetContent();
        }

        skillPreviewElements = new List<int> { 0, 1, 2, 3, 4 };
        SetElementSkills();
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
        //if (!Game.Instance.TestMode)
        //{
        //    MenuManager.Instance.ShowMessage(GameMultiLang.GetTraduction("TEST3"));
        //    return;
        //}
        base.Show();
        m_Anim.SetBool("OpenLevel", true);
        gameLevelPrefab.SetData();
    }

    public override void Hide()
    {
        base.Hide();
        MenuManager.Instance.HideTips();
    }

    public override void ClosePanel()
    {
        m_Anim.SetBool("OpenLevel", false);
        MenuManager.Instance.ShowMenu();
    }
}
