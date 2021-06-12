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
    //存放所有需要控制的UI
    private FuncUI m_FuncUI;
    private MainUI m_MainUI;
    //触发条件：点击指定格子，按钮
    //当满足条件时，控制各UI的显示动画
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
        switch (currentGuideIndex)//具体每个教程做什么
        {
            case 0://鼠标移动视角教程
                ScaleAndMove.MoveTurorial = true;
                ScaleAndMove.CanControl = true;
                break;
            case 1://鼠标放大教程
                ScaleAndMove.SizeTutorial = true;
                ScaleAndMove.CanControl = true;
                break;
            case 2://抽取模块教程
                m_FuncUI.DrawBtnObj.SetActive(true);
                m_FuncUI.Show();
                break;
            case 3://摆放模块教程
                m_GameManager.ShowGuideVideo(0);
                break;
            case 4://显示上方UI,波次和血量
                m_MainUI.WaveObj.SetActive(true);
                m_MainUI.LifeObj.SetActive(true);
                m_MainUI.Show();
                GuideTrigger(5);
                break;
            case 5://显示下一波按钮
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
            Debug.LogWarning("错误的教程触发时机:"+index);
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
