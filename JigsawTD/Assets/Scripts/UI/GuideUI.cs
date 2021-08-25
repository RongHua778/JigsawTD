using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string GuideNote;
    public int TriggerID;//����ID
    public string Name;//˵��������
    public string[] Words;//�Ի�����
    public bool DontNeedClickEnd;//�Ƿ��Զ���������������
}
public class GuideUI : IUserInterface
{
    //���������Ҫ���Ƶ�UI
    private FuncUI m_FuncUI;
    private MainUI m_MainUI;
    private BluePrintShopUI m_ShopUI;
    private ShapeSelectUI m_ShapeUI;
    //�������������ָ�����ӣ���ť
    //����������ʱ�����Ƹ�UI����ʾ����
    [SerializeField] Dialogue[] dialogues = default;
    [SerializeField] GameObject backBtn = default;
    private Dialogue currentDialogue;
    private bool typingSentence = false;

    private Queue<string> wordQueue;

    [SerializeField]private int currentGuideIndex = 0;
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
            m_FuncUI.Hide();//��ΪPreparenextwave�Զ�show��
        }
        else
        {
            ScaleAndMove.CanControl = true;
        }
    }


    private void StartDialogue(int index)
    {
        backBtn.SetActive(true);
        currentDialogue = dialogues[index];
        wordQueue.Clear();
        foreach (var key in currentDialogue.Words)
        {
            string word = GameMultiLang.GetTraduction(key);
            wordQueue.Enqueue(word);
        }
        GuideStartEvent(CurrentGuideIndex);

        //Invoke(currentDialogue.GuideStartEvent, 0f);

        //switch (index)//�̳̿�ʼʱ��ʲô
        //{
        //    case 3://�ڷ�ģ��̡̳������ǰ������水ť�ϵ�
        //        GameManager.Instance.ShowGuideVideo(1);
        //        GameManager.Instance.ShowGuideVideo(0);
        //        m_FuncUI.NextBtnObj.SetActive(true);
        //        break;
        //    case 4://Ѫ��
        //        GameManager.Instance.HideTips();
        //        m_MainUI.LifeObj.SetActive(true);
        //        m_MainUI.PlayAnim("ShowLife", true);
        //        break;
        //    case 5:
        //        m_MainUI.WaveObj.SetActive(true);
        //        m_MainUI.PlayAnim("ShowWave", true);
        //        break;
        //    case 6:
        //        m_FuncUI.LuckyObj.SetActive(true);
        //        break;
        //    case 7:
        //        m_FuncUI.LevelBtnObj.SetActive(true);
        //        m_MainUI.MoneyObj.SetActive(true);
        //        m_MainUI.PlayAnim("ShowMoney", true);
        //        break;
        //    case 8:
        //        m_ShopUI.ShopBtnObj.SetActive(true);
        //        break;
        //    case 9:
        //        GameManager.Instance.ShowGuideVideo(2);
        //        break;

        //}


        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        backBtn.SetActive(false);
        CurrentGuideIndex++;
        GuideEndEvent(currentGuideIndex);
        
        //Invoke(currentDialogue.GuideEndEvent, 0f);
        //switch (currentGuideIndex - 1)//����ÿ���̳���ʲô����������index-1
        //{
        //    case 0://����ƶ��ӽǽ̳�
        //        ScaleAndMove.MoveTurorial = true;
        //        ScaleAndMove.CanControl = true;
        //        break;
        //    case 1://���Ŵ�̳�
        //        ScaleAndMove.SizeTutorial = true;
        //        ScaleAndMove.CanControl = true;
        //        m_FuncUI.Hide();//����������Ĭ�ϱ�prepareNextwave��SHOW��
        //        break;
        //    case 2://��ȡģ��̳�
        //        m_FuncUI.DrawBtnObj.SetActive(true);
        //        m_FuncUI.Show();
        //        break;
        //    case 4:
        //        GameManager.Instance.TriggerGuide(5);
        //        break;
        //    case 9://end
        //        m_MainUI.TopLeftArea.SetActive(true);
        //        //m_MainUI.SpeedBtnObj.SetActive(true);
        //        //m_MainUI.GuideVideoBtnObj.SetActive(true);
        //        m_MainUI.PlayAnim("ShowLife", false);
        //        m_MainUI.PlayAnim("ShowOther", true);
        //        break;
        //    case 10:
        //        Hide();
        //        Game.Instance.Tutorial = false;
        //        break;
        //}

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


    public int GuideTrigger(int index)
    {
        if (index == CurrentGuideIndex)
        {
            StartCoroutine(GuideCor(index));
            return index;
        }
        return -1;
    }

    IEnumerator GuideCor(int index)
    {
        yield return new WaitForSeconds(0.1f);
        StartDialogue(index);
    }

    public void NextBtnClick()
    {
        if (!typingSentence)
            DisplayNextSentence();
    }

    #region GuideCallBack
    //private void AllowMouseControl()
    //{
    //    ScaleAndMove.MoveTurorial = true;
    //    ScaleAndMove.CanControl = true;
    //}
    //private void AllowSizeControl()
    //{
    //    ScaleAndMove.SizeTutorial = true;
    //    ScaleAndMove.CanControl = true;
    //}

    //private void ShowMoney()
    //{
    //    m_MainUI.MoneyObj.SetActive(true);
    //    m_MainUI.PlayAnim("ShowMoney", true);
    //    m_FuncUI.Hide();
    //    GameManager.Instance.TriggerGuide(3);
    //}

    //private void FirstTutorialDraw()
    //{
    //    m_FuncUI.DrawBtnObj.SetActive(true);
    //    m_FuncUI.Show();
    //    m_ShapeUI.TutorialID = 1;
    //}
    private void GuideStartEvent(int id)
    {
        switch (id)
        {
            case 4://��ʾ����ֵ
                m_MainUI.LifeObj.SetActive(true);
                m_MainUI.PlayAnim("ShowLife", true);
                break;
            case 5://��ʾ��һ����Ϣ
                m_MainUI.WaveObj.SetActive(true);
                m_MainUI.PlayAnim("ShowWave", true);
                break;
            case 6:
                m_ShapeUI.TutorialID = 2;
                m_FuncUI.NextBtnObj.SetActive(false);
                break;
            case 7:
                m_FuncUI.NextBtnObj.SetActive(true);
                break;
            case 8:
                m_ShapeUI.TutorialID = 3;
                m_FuncUI.NextBtnObj.SetActive(false);
                break;
            case 9://���µ�����ģ��󣬳��ֺϳɰ�ť
                m_ShopUI.ShopBtnObj.SetActive(true);
                break;

        }
    }
    private void GuideEndEvent(int id)
    {
        switch (id)
        {
            case 1://��һ�ζԻ�����������ƶ�����
                ScaleAndMove.MoveTurorial = true;
                ScaleAndMove.CanControl = true;
                break;
            case 2://�ڶ��ζԻ�������������Ų���
                ScaleAndMove.SizeTutorial = true;
                ScaleAndMove.CanControl = true;
                break;
            case 3://��ʾ���
                m_MainUI.MoneyObj.SetActive(true);
                m_MainUI.PlayAnim("ShowMoney", true);
                m_FuncUI.Hide();
                GameManager.Instance.TriggerGuide(3);
                break;
            case 4://��ʾ��ȡ��ť
                m_FuncUI.DrawBtnObj.SetActive(true);
                m_FuncUI.Show();
                m_ShapeUI.TutorialID = 1;
                break;
            case 5:
                GameManager.Instance.TriggerGuide(5);
                break;
            case 6://��ʾ���ְ�ť
                m_FuncUI.NextBtnObj.SetActive(true);
                break;

        }
    }


    #endregion

}
