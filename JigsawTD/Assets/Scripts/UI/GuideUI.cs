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

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        GameEvents.Instance.onGuideTrigger += GuideControl;
        wordQueue = new Queue<string>();
        if (Game.Instance.Difficulty == 1)
        {
            GuideControl(0);
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
        oldContent.text += "\n" + newContent.text;
        string word = wordQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(word));
    }


    private void EndDialogue()
    {
        backBtn.SetActive(false);
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


    private void GuideControl(int index)
    {
        StartCoroutine(GuideCor(index));
    }

    IEnumerator GuideCor(int index)
    {
        yield return new WaitForSeconds(1f);
        if (index == currentGuideIndex)
        {
            switch (index)//具体每个教程做什么
            {
                case 0:
                    Show();
                    StartDialogue(0);
                    ScaleAndMove.MoveTurorial = true;
                    ScaleAndMove.CanControl = true;
                    break;
                case 1:
                    Show();
                    StartDialogue(1);
                    ScaleAndMove.SizeTutorial = true;
                    ScaleAndMove.CanControl = true;
                    break;
                case 2:
                    StartDialogue(2);
                    break;
            }
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
