using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Dialogue
{
    public string GuideNote;
    public TutorialType TriggerType;
    public int TriggerID;//����ID
    public string Name;//˵��������
    public string[] Words;//�Ի�����
    public bool DontNeedClickEnd;//�Ƿ��Զ���������������
    public float WaitingTime;
}
public class GuideGirlUI : IUserInterface
{
    private Animator anim;
    private FuncUI m_FuncUI;
    private MainUI m_MainUI;
    private BluePrintShopUI m_ShopUI;

    //�������������ָ�����ӣ���ť
    //����������ʱ�����Ƹ�UI����ʾ����
    [SerializeField] Dialogue[] dialogues = default;
    [SerializeField] GameObject backBtn = default;
    [SerializeField] GuideIndicator guideIndicator = default;
    [SerializeField] GameObject dragGuide = default;
    [SerializeField] GameObject wheelGuide = default;
    [SerializeField] int startIndex = default;
    private Dialogue currentDialogue;
    private bool typingSentence = false;

    private Queue<string> wordQueue;

    [SerializeField] private int currentGuideIndex = 0;
    public int CurrentGuideIndex { get => currentGuideIndex; set => currentGuideIndex = value; }

    [SerializeField] TextMeshProUGUI dialogTxt = default;


    //GuideObjs
    [SerializeField] GameObject turretTips_ElementSkillObj = default;
    [SerializeField] GameObject turretTips_RefactorObj = default;

    [Header("С�����ʱ�Ի�")]
    private const float DialogueTime = 8f;
    [SerializeField] Dialogue[] StandardLost = default;
    [SerializeField] Dialogue[] StandardWin = default;
    [SerializeField] Dialogue[] EndLessEnd = default;
    [SerializeField] Dialogue[] Refactor = default;
    [SerializeField] Dialogue[] RefreshShop = default;


    public void Initialize(FuncUI funcUI, MainUI mainUI, BluePrintShopUI shopUI, ShapeSelectUI shapeUI)
    {
        anim = this.GetComponent<Animator>();
        m_FuncUI = funcUI;
        m_MainUI = mainUI;
        m_ShopUI = shopUI;
        wordQueue = new Queue<string>();
        GameEvents.Instance.onTempWord += DisplayTempDialogue;
        if (Game.Instance.Tutorial)
        {
            GameEvents.Instance.onTutorialTrigger += GuideTrigger;

            currentDialogue = dialogues[startIndex];
            m_FuncUI.Hide();//��ΪPreparenextwave�Զ�show��
            GuideTrigger(TutorialType.None);
        }
        else
        {
            ScaleAndMove.CanControl = true;
        }
    }
    public override void Release()
    {
        base.Release();
        GameEvents.Instance.onTutorialTrigger -= GuideTrigger;
        GameEvents.Instance.onTempWord -= DisplayTempDialogue;

    }

    #region ��ʱ�Ի�
    private void DisplayTempDialogue(TempWord wordType)
    {
        if (typingSentence||Game.Instance.Tutorial)
            return;
        switch (wordType.WordType)
        {
            case TempWordType.StandardLose:
                StartCoroutine(TempWordCor(StandardLost[wordType.ID]));//idΪ��׼ģʽ�Ѷ�
                break;
            case TempWordType.StandardWin:
                StartCoroutine(TempWordCor(StandardWin[wordType.ID]));//idΪ��׼ģʽ�Ѷ�
                break;
            case TempWordType.EndlessEnd:
                break;
            case TempWordType.RefreshShop:
                if (Random.value > 0.95f)//��5%���ʴ���
                    StartCoroutine(TempWordCor(RefreshShop[wordType.ID]));
                break;
            case TempWordType.Refactor:
                break;
        }
    }


    IEnumerator TempWordCor(Dialogue dialogue)
    {
        typingSentence = true;
        Show();
        backBtn.SetActive(false);
        dialogTxt.text = "";
        yield return new WaitForSeconds(0.5f);
        string word = GameMultiLang.GetTraduction(dialogue.Words[Random.Range(0, dialogue.Words.Length)]);
        dialogTxt.text = word;
        dialogTxt.maxVisibleCharacters = 0;
        dialogTxt.ForceMeshUpdate();
        var textInfo = dialogTxt.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            SetCharacterAlpha(i, 0);
        }

        // ��ʱ�������ʾ�ַ�
        var timer = 0f;
        var interval = 0.03f;
        while (dialogTxt.maxVisibleCharacters < textInfo.characterCount)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                dialogTxt.maxVisibleCharacters++;
            }

            yield return null;
        }
        yield return new WaitForSeconds(DialogueTime);
        Hide();
        typingSentence = false;
    }


    #endregion
    private void StartDialogue()
    {
        backBtn.SetActive(true);
        wordQueue.Clear();
        foreach (var key in currentDialogue.Words)
        {
            string word = GameMultiLang.GetTraduction(key);
            wordQueue.Enqueue(word);
        }
        GuideStartEvent();

        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        backBtn.SetActive(false);
        GuideEndEvent();
        if (CurrentGuideIndex < dialogues.Length - 1)//����������һ���̳̣���������һ���̳�
        {
            CurrentGuideIndex++;
            currentDialogue = dialogues[CurrentGuideIndex];
            if (currentDialogue.TriggerType == TutorialType.None)//�����NONE��ֱ�ӿ�ʼ��һ���̳�
                GuideTrigger(TutorialType.None);
        }
    }

    private void DisplayNextSentence()
    {
        if (wordQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        string word = wordQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(word));
    }

    private IEnumerator TypeSentence(string word)
    {
        typingSentence = true;
        //dialogTxt.text = "<color=yellow>" + GameMultiLang.GetTraduction(currentDialogue.Name) + ":</color>";
        dialogTxt.text = word;
        dialogTxt.maxVisibleCharacters = 0;
        dialogTxt.ForceMeshUpdate();

        var textInfo = dialogTxt.textInfo;
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            SetCharacterAlpha(i, 0);
        }

        // ��ʱ�������ʾ�ַ�
        var timer = 0f;
        var interval = 0.03f;
        while (dialogTxt.maxVisibleCharacters < textInfo.characterCount)
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                dialogTxt.maxVisibleCharacters++;
            }

            yield return null;
        }
        typingSentence = false;
        if (wordQueue.Count == 0)
        {
            if (currentDialogue.DontNeedClickEnd)
                EndDialogue();
        }
    }

    private void SetCharacterAlpha(int index, byte alpha)
    {
        var materialIndex = dialogTxt.textInfo.characterInfo[index].materialReferenceIndex;
        var vertexColors = dialogTxt.textInfo.meshInfo[materialIndex].colors32;
        var vertexIndex = dialogTxt.textInfo.characterInfo[index].vertexIndex;

        vertexColors[vertexIndex + 0].a = alpha;
        vertexColors[vertexIndex + 1].a = alpha;
        vertexColors[vertexIndex + 2].a = alpha;
        vertexColors[vertexIndex + 3].a = alpha;

    }


    public void GuideTrigger(TutorialType triggetType)
    {
        if (triggetType == currentDialogue.TriggerType)
        {
            StartCoroutine(GuideCor());
        }
    }

    IEnumerator GuideCor()
    {
        yield return new WaitForSeconds(currentDialogue.WaitingTime);
        StartDialogue();
    }

    public void NextBtnClick()
    {
        if (!typingSentence)
            DisplayNextSentence();
    }

    #region GuideCallBack
    private void GuideStartEvent()
    {
        switch (currentDialogue.TriggerID)
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
            case 5://��ʾ����ֵ
                m_MainUI.LifeObj.SetActive(true);
                m_MainUI.PlayAnim("ShowLife", true);
                break;
            case 6://��ʾ��һ����Ϣ
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
            case 24://����ع���ť��
            case 23://����̵갴ť
            case 22:
                guideIndicator.Show(false);
                break;
            case 12://���ºϳ���
                guideIndicator.Show(false);

                m_FuncUI.NextBtnObj.SetActive(true);
                break;
            case 13://���ĻغϿ�ʼ����ʾģ��ȼ�
                m_FuncUI.LevelBtnObj.SetActive(true);
                guideIndicator.Show(false);
                break;
            case 14:
                guideIndicator.Show(false);
                break;

        }
    }
    private void GuideEndEvent()
    {
        switch (currentDialogue.TriggerID)
        {
            case 0://��һ�ζԻ�����������ƶ�����
                   //����һ��ר���䷽
                List<int> elements = new List<int> { 0, 1, 2 };
                List<int> qualities = new List<int> { 1, 1, 1 };
                RefactorStrategy strategy = ConstructHelper.GetSpecificStrategy("CONSTRUCTOR", elements,qualities);

                m_ShopUI.AddBluePrint(strategy, true);
                m_ShopUI.RemoveGrid(BluePrintShopUI.ShopBluePrints[0]);//�Ƴ�1��

                ScaleAndMove.MoveTurorial = true;
                ScaleAndMove.CanControl = true;

                dragGuide.SetActive(true);
                turretTips_RefactorObj.SetActive(false);
                break;
            case 1://�ڶ��ζԻ�������������Ų���
                ScaleAndMove.SizeTutorial = true;
                ScaleAndMove.CanControl = true;

                wheelGuide.SetActive(true);
                break;
            case 2://��ʾ���
                m_MainUI.MoneyObj.SetActive(true);
                m_MainUI.PlayAnim("ShowMoney", true);
                m_FuncUI.Hide();

                break;
            case 3://��ʾ��ȡ��ť
                m_FuncUI.DrawBtnObj.SetActive(true);
                m_FuncUI.Show();

                guideIndicator.Show(true, m_FuncUI.DrawBtnObj.transform);

                ShapeInfo shapeInfo = new ShapeInfo(ShapeType.L, ElementType.WATER, 1, 3);
                GameRes.PreSetShape = shapeInfo;
                List<Vector2> poss = new List<Vector2> { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2), new Vector2(1, 2) };
                ForcePlace forcePlace = new ForcePlace(new Vector2(0, 2), Vector2.right, poss);
                GameRes.ForcePlace = forcePlace;

                break;

            case 6://��ʾ���ְ�ť
            case 201://ѡ��������鿴TIPS����ʼ��һ��
                m_FuncUI.NextBtnObj.SetActive(true);
                m_FuncUI.Show();
                guideIndicator.Show(true, m_FuncUI.NextBtnObj.transform);

                break;
            case 7:
                guideIndicator.Show(true, m_FuncUI.DrawBtnObj.transform);//�ڶ��γ�ȡָ��
                break;
            case 9:
                guideIndicator.Show(true, m_FuncUI.DrawBtnObj.transform);//�ڶ��γ�ȡָ��
                break;
            case 11:
                guideIndicator.Show(true, turretTips_ElementSkillObj.transform);//�ڶ��γ�ȡָ��
                break;
            case 12:
            case 13://����/�Ĵγ���
                guideIndicator.Show(true, m_FuncUI.NextBtnObj.transform);
                break;
            case 14://����
                Hide();
                Game.Instance.Tutorial = false;
                break;
            case 22://��ͣԪ�ؼ��ܺ󣬵���ع���ť
                turretTips_RefactorObj.SetActive(true);
                guideIndicator.Show(true, turretTips_RefactorObj.transform);
                break;

        }
    }


    #endregion


    public override void Show()
    {
        base.Show();
        Sound.Instance.PlayEffect("Sound_Guide");
        anim.SetBool("Show", true);
    }
    public override void Hide()
    {
        Sound.Instance.PlayEffect("Sound_Guide");
        anim.SetBool("Show", false);
    }
    public void HideRoot()
    {
        m_RootUI.gameObject.SetActive(false);
    }



}
