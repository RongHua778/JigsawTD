using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public string Name;
    [TextArea(2, 3)]
    public string[] Words;
    public bool DontNeedClickEnd;
}
public class GuideUI : IUserInterface
{
    //���������Ҫ���Ƶ�UI
    private FuncUI m_FuncUI;
    private MainUI m_MainUI;
    private BluePrintShopUI m_ShopUI;
    //�������������ָ�����ӣ���ť
    //����������ʱ�����Ƹ�UI����ʾ����
    [SerializeField] Dialogue[] dialogues = default;
    [SerializeField] GameObject backBtn = default;
    private Dialogue currentDialogue;
    private bool typingSentence = false;

    private Queue<string> wordQueue;

    private int currentGuideIndex = 0;

    [SerializeField] Text newContent = default;
    [SerializeField] Text oldContent = default;


    public void Initialize(GameManager gameManager, FuncUI funcUI, MainUI mainUI,BluePrintShopUI shopUI)
    {
        Initialize(gameManager);
        m_FuncUI = funcUI;
        m_MainUI = mainUI;
        m_ShopUI = shopUI;
        wordQueue = new Queue<string>();
        if (Game.Instance.Difficulty == 1)
        {
            GuideTrigger(0);
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
        foreach (var word in currentDialogue.Words)
        {
            wordQueue.Enqueue(word);
        }

        switch (index)//�̳̿�ʼʱ��ʲô
        {
            case 3://�ڷ�ģ��̡̳������ǰ������水ť�ϵ�
                m_GameManager.ShowGuideVideo(1);
                m_GameManager.ShowGuideVideo(0);
                m_FuncUI.NextBtnObj.SetActive(true);
                break;
            case 4://Ѫ��
                GameManager.Instance.HideTips();
                m_MainUI.LifeObj.SetActive(true);
                m_MainUI.PlayAnim("ShowLife", true);
                break;
            case 5:
                m_MainUI.WaveObj.SetActive(true);
                m_MainUI.PlayAnim("ShowWave", true);
                break;
            case 6:
                m_FuncUI.LuckyObj.SetActive(true);
                break;
            case 7:
                m_FuncUI.LevelBtnObj.SetActive(true);
                m_MainUI.MoneyObj.SetActive(true);
                m_MainUI.PlayAnim("ShowMoney", true);
                break;
            case 8:
                m_ShopUI.ShopBtnObj.SetActive(true);
                break;
            case 9:
                m_GameManager.ShowGuideVideo(2);
                break;

        }


        DisplayNextSentence();
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


    private void EndDialogue()
    {
        backBtn.SetActive(false);
        switch (currentGuideIndex - 1)//����ÿ���̳���ʲô����������index-1
        {
            case 0://����ƶ��ӽǽ̳�
                ScaleAndMove.MoveTurorial = true;
                ScaleAndMove.CanControl = true;
                break;
            case 1://���Ŵ�̳�
                ScaleAndMove.SizeTutorial = true;
                ScaleAndMove.CanControl = true;
                m_FuncUI.Hide();//����������Ĭ�ϱ�prepareNextwave��SHOW��
                break;
            case 2://��ȡģ��̳�
                m_FuncUI.DrawBtnObj.SetActive(true);
                m_FuncUI.Show();
                break;
            case 4:
                GameManager.Instance.TriggerGuide(5);
                break;
            case 9://end
                m_MainUI.TopLeftArea.SetActive(true);
                //m_MainUI.SpeedBtnObj.SetActive(true);
                //m_MainUI.GuideVideoBtnObj.SetActive(true);
                m_MainUI.PlayAnim("ShowLife", false);
                m_MainUI.PlayAnim("ShowOther", true);
                break;
            case 10:
                Hide();
                Game.Instance.Tutorial = false;
                break;
        }

    }

    private IEnumerator TypeSentence(string word)
    {
        typingSentence = true;
        newContent.text = "<color=yellow>" + currentDialogue.Name + ":</color>";
        foreach (char letter in word.ToCharArray())
        {
            newContent.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        typingSentence = false;
        if (wordQueue.Count == 0)
        {
            if (currentDialogue.DontNeedClickEnd)
                EndDialogue();
        }
    }


    public void GuideTrigger(int index)
    {
        if (index == currentGuideIndex)
        {
            //currentGuideIndex++;
            StartCoroutine(GuideCor(index));
        }
            //StartCoroutine(GuideCor(index));
    }

    IEnumerator GuideCor(int index)
    {
        yield return new WaitForSeconds(0.1f);
        currentGuideIndex++;
        StartDialogue(index);
    }

    public void NextBtnClick()
    {
        if (!typingSentence)
            DisplayNextSentence();
    }



    public override void Update()
    {

    }

}
