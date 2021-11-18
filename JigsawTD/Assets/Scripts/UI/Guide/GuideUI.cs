using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideUI : IUserInterface
{
    //存放所有需要控制的UI
    private FuncUI m_FuncUI;
    private MainUI m_MainUI;
    private BluePrintShopUI m_ShopUI;
    private ShapeSelectUI m_ShapeUI;
    //触发条件：点击指定格子，按钮
    //当满足条件时，控制各UI的显示动画
    [SerializeField] Dialogue[] dialogues = default;
    [SerializeField] GameObject backBtn = default;
    [SerializeField] GuideIndicator guideIndicator = default;
    [SerializeField] GameObject dragGuide = default;
    [SerializeField] GameObject wheelGuide = default;
    private Dialogue currentDialogue;
    private bool typingSentence = false;

    private Queue<string> wordQueue;

    [SerializeField] private int currentGuideIndex = 0;
    public int CurrentGuideIndex { get => currentGuideIndex; set => currentGuideIndex = value; }


    [SerializeField] Text newContent = default;
    [SerializeField] Text oldContent = default;


    public void Initialize(FuncUI funcUI, MainUI mainUI, BluePrintShopUI shopUI, ShapeSelectUI shapeUI)
    {
        m_FuncUI = funcUI;
        m_MainUI = mainUI;
        m_ShopUI = shopUI;
        m_ShapeUI = shapeUI;
        wordQueue = new Queue<string>();
        if (Game.Instance.Tutorial)
        {
            GuideTrigger(0);
            m_FuncUI.Hide();//因为Preparenextwave自动show了
        }
        else
        {
            ScaleAndMove.CanControl = true;
        }
    }


    private void StartDialogue()
    {
        backBtn.SetActive(true);
        wordQueue.Clear();
        foreach (var key in currentDialogue.Words)
        {
            string word = GameMultiLang.GetTraduction(key);
            wordQueue.Enqueue(word);
        }
        GuideStartEvent(CurrentGuideIndex);

        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        backBtn.SetActive(false);
        GuideEndEvent(currentGuideIndex);
        if (CurrentGuideIndex < dialogues.Length)//如果不是最后一个教程，就设置下一个教程
        {
            CurrentGuideIndex++;
            currentDialogue = dialogues[CurrentGuideIndex];
        }
    }

    private void DisplayNextSentence()
    {
        if (wordQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        oldContent.text += "\n" + newContent.text;
        string word = wordQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(word));
    }

    private IEnumerator TypeSentence(string word)
    {
        typingSentence = true;
        newContent.text = "<color=yellow>" + GameMultiLang.GetTraduction(currentDialogue.Name) + ":</color>";
        foreach (char letter in word.ToCharArray())
        {
            newContent.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
        typingSentence = false;
        if (wordQueue.Count == 0)
        {
            if (currentDialogue.DontNeedClickEnd)
                EndDialogue();
        }
    }


    public void GuideTrigger(TutorialType triggetType)
    {

        if (currentDialogue.TriggerType == triggetType)
        {
            StartCoroutine(GuideCor());
        }
    }

    IEnumerator GuideCor()
    {
        yield return new WaitForSeconds(0.1f);
        StartDialogue();
    }

    public void NextBtnClick()
    {
        if (!typingSentence)
            DisplayNextSentence();
    }

    #region GuideCallBack
    private void GuideStartEvent(int id)
    {
        switch (id)
        {
            case 0:
                m_MainUI.TopLeftArea.SetActive(true);
                break;
            case 1:
                dragGuide.SetActive(false);
                break;
            case 2:
                wheelGuide.SetActive(false);
                break;
            case 4:
                guideIndicator.Show(false);
                break;
            case 5://显示生命值
                m_MainUI.LifeObj.SetActive(true);
                m_MainUI.PlayAnim("ShowLife", true);
                break;
            case 6://显示下一波信息
                m_MainUI.WaveObj.SetActive(true);
                m_MainUI.PlayAnim("ShowWave", true);
                m_MainUI.PlayAnim("ShowMoney", false);
                break;
            case 7:
                ShapeInfo shapeInfo = new ShapeInfo(ShapeType.L, ElementType.GOLD, 1, 1);
                GameRes.PreSetShape = shapeInfo;
                List<Vector2> poss = new List<Vector2> { new Vector2(0, 1), new Vector2(0, 2), new Vector2(-1, 2), new Vector2(-2, 2) };
                ForcePlace forcePlace = new ForcePlace(new Vector2(0, 2), Vector2.down, poss);
                GameRes.ForcePlace = forcePlace;

                m_FuncUI.NextBtnObj.SetActive(false);
                guideIndicator.Show(false);

                break;
            case 8:
                m_FuncUI.Hide();

                break;
            case 9:
                ShapeInfo shapeInfo2 = new ShapeInfo(ShapeType.T, ElementType.WOOD, 1, 3);
                GameRes.PreSetShape = shapeInfo2;
                List<Vector2> poss2 = new List<Vector2> { new Vector2(-2, 0), new Vector2(-2, 1), new Vector2(-2, -1), new Vector2(-3, 0) };
                ForcePlace forcePlace2 = new ForcePlace(new Vector2(-2, 0), Vector2.left, poss2);
                GameRes.ForcePlace = forcePlace2;

                m_FuncUI.NextBtnObj.SetActive(false);
                guideIndicator.Show(false);
                break;
            case 10:
                m_ShopUI.ShopBtnObj.SetActive(true);
                List<Vector2> poss3 = new List<Vector2> { new Vector2(-1, 1) };
                ForcePlace forcePlace3 = new ForcePlace(new Vector2(-1, 1), Vector2.zero, poss3);
                GameRes.ForcePlace = forcePlace3;
                guideIndicator.Show(true, m_ShopUI.ShopBtnObj.transform);

                break;
            case 11:
                m_FuncUI.NextBtnObj.SetActive(false);
                guideIndicator.Show(false);

                break;
            case 12://放下合成塔
                m_FuncUI.NextBtnObj.SetActive(true);
                break;
            case 13://第四回合开始，显示模块等级
                m_FuncUI.LevelBtnObj.SetActive(true);
                guideIndicator.Show(false);
                break;
            case 14:
                guideIndicator.Show(false);
                break;

        }
    }
    private void GuideEndEvent(int id)//比正常值+1
    {
        switch (id)
        {
            case 1://第一段对话结束，鼠标移动操作
                //生成一个专属配方
                //Blueprint blueprint = ConstructHelper.GetSpecificBlueprint("CONSTRUCTOR", 0, 1, 2);
                //RefactorStrategy strategy=ConstructHelper
                //m_ShopUI.AddBluePrint(blueprint, true);
                m_ShopUI.RemoveGrid(m_ShopUI.ShopBluePrints[0]);//移除1个

                ScaleAndMove.MoveTurorial = true;
                ScaleAndMove.CanControl = true;

                dragGuide.SetActive(true);
                break;
            case 2://第二段对话结束，鼠标缩放操作
                ScaleAndMove.SizeTutorial = true;
                ScaleAndMove.CanControl = true;

                wheelGuide.SetActive(true);
                break;
            case 3://显示金币
                m_MainUI.MoneyObj.SetActive(true);
                m_MainUI.PlayAnim("ShowMoney", true);
                m_FuncUI.Hide();
                GuideTrigger(TutorialType.None);
                break;
            case 4://显示抽取按钮
                m_FuncUI.DrawBtnObj.SetActive(true);
                m_FuncUI.Show();

                guideIndicator.Show(true, m_FuncUI.DrawBtnObj.transform);

                ShapeInfo shapeInfo = new ShapeInfo(ShapeType.L, ElementType.WATER, 1, 3);
                GameRes.PreSetShape = shapeInfo;
                List<Vector2> poss = new List<Vector2> { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 2) };
                ForcePlace forcePlace = new ForcePlace(new Vector2(0, 2), Vector2.right, poss);
                GameRes.ForcePlace = forcePlace;

                break;
            case 6:
                //GameManager.Instance.TriggerGuide(6);
                GuideTrigger(TutorialType.None);

                break;

            case 7://显示出怪按钮
            case 9:
                m_FuncUI.NextBtnObj.SetActive(true);
                m_FuncUI.Show();
                guideIndicator.Show(true, m_FuncUI.NextBtnObj.transform);

                break;
            case 8:
                guideIndicator.Show(true, m_FuncUI.DrawBtnObj.transform);//第二次抽取指引
                break;
            case 10:
                guideIndicator.Show(true, m_FuncUI.DrawBtnObj.transform);//第二次抽取指引
                break;
            case 13:
            case 14://第三/四次出兵
                guideIndicator.Show(true, m_FuncUI.NextBtnObj.transform);
                break;
            case 15://结束
                Hide();
                Game.Instance.Tutorial = false;
                break;

        }
    }


    #endregion

}
