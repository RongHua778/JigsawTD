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
}
public class GuideUI : IUserInterface
{
    //���������Ҫ���Ƶ�UI
    private FuncUI m_FuncUI;
    private MainUI m_MainUI;
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


    public void Initialize(GameManager gameManager,FuncUI funcUI,MainUI mainUI)
    {
        Initialize(gameManager);
        m_FuncUI = funcUI;
        m_MainUI = mainUI;
        GameEvents.Instance.onGuideTrigger += GuideTrigger;
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
        switch (currentGuideIndex)//����ÿ���̳���ʲô
        {
            case 0://����ƶ��ӽǽ̳�
                ScaleAndMove.MoveTurorial = true;
                ScaleAndMove.CanControl = true;
                break;
            case 1://���Ŵ�̳�
                ScaleAndMove.SizeTutorial = true;
                ScaleAndMove.CanControl = true;
                break;
            case 2://��ȡģ��̳�
                m_FuncUI.DrawBtnObj.SetActive(true);
                m_FuncUI.Show();
                break;
            case 3://�ڷ�ģ��̳�
                m_GameManager.ShowGuideVideo(0);
                break;
            case 4://��ʾ�Ϸ�UI,���κ�Ѫ��
                m_MainUI.WaveObj.SetActive(true);
                m_MainUI.LifeObj.SetActive(true);
                m_MainUI.Show();
                GuideTrigger(5);
                break;
            case 5://��ʾ��һ����ť
                m_FuncUI.NextBtnObj.SetActive(true);
                break;
        }
        currentGuideIndex++;
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
            EndDialogue();
        }
    }


    public void GuideTrigger(int index)
    {
        StartCoroutine(GuideCor(index));
    }

    IEnumerator GuideCor(int index)
    {
        yield return new WaitForSeconds(1f);
        if (index == currentGuideIndex)
        {
            Show();
            StartDialogue(index);
        }
        else
        {
            Debug.LogWarning("����Ľ̴̳���ʱ��:"+index);
        }
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
