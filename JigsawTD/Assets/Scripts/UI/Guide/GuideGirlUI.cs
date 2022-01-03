using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class GuideGirlUI : SerializedMonoBehaviour
{
    [SerializeField] int startIndex = default;
    [ReadOnly]
    [ShowInInspector]
    public int CurrentGuideIndex { get; set; }

    private const float DialogueTime = 8f;

    private Transform m_RootUI;
    [SerializeField] private RectTransform m_GirlTr = default;
    private Animator anim;

    private GameObject backBtn;
    private DialogueData currentDialogue;
    private bool typingSentence = false;
    private TextMeshProUGUI dialogTxt;
    private Queue<string> wordQueue;

    //[Title("�̳�����")]
    //[ListDrawerSettings(CustomAddFunction = "AddDialogue")]
    //[SerializeField] DialogueData[] GuideDialogues = default;
    private DialogueData[] m_Dialogues;

    [Title("С�����ʱ�Ի�")]
    [SerializeField] DialogueData RefreshShop = default;


    [Title("��ѧ����")]
    [SerializeField] GameObject[] GuideObjects = default;
    Dictionary<string, GameObject> GuideDIC = new Dictionary<string, GameObject>();
    [SerializeField] GameObject ClickTip = default;

    public void Initialize()
    {
        m_RootUI = transform.Find("Root");
        backBtn = m_RootUI.Find("BackBtn").gameObject;
        dialogTxt = m_RootUI.GetComponentInChildren<TextMeshProUGUI>();
        anim = this.GetComponent<Animator>();
        wordQueue = new Queue<string>();
        m_Dialogues = LevelManager.Instance.CurrentLevel.GuideDialogues;
        Game.Instance.Tutorial = m_Dialogues.Length > 0;//����н�ѧ�԰׾ͱ�ʾ��ʼ��ѧ
        GameEvents.Instance.onTempWord += DisplayTempDialogue;
    }

    private void InitializeGuideDIC()
    {
        foreach (var item in GuideObjects)
        {
            GuideDIC.Add(item.name, item);
        }
    }

    public GameObject GetGuideObj(string name)
    {
        if (GuideDIC.ContainsKey(name))
            return GuideDIC[name];
        else
        {
            Debug.LogWarning("û�п��Ըý�ѧ����:" + name);
            return null;
        }
    }

    public void PrepareTutorial()
    {
        InitializeGuideDIC();
        if (m_Dialogues.Length > 0)
            StarTutorial();
    }
    private void StarTutorial()
    {
        GameEvents.Instance.onTutorialTrigger += GuideTrigger;
        CurrentGuideIndex = startIndex;
        currentDialogue = m_Dialogues[CurrentGuideIndex];
        GuideTrigger();
    }



    public void Release()
    {
        GameEvents.Instance.onTutorialTrigger -= GuideTrigger;
        GameEvents.Instance.onTempWord -= DisplayTempDialogue;
    }

    #region ��ʱ�Ի�
    private void DisplayTempDialogue(TempWord wordType)
    {
        if (typingSentence || Game.Instance.Tutorial)//��ѧ�ڼ䲻������ʱ�Ի�
            return;
        switch (wordType.WordType)
        {
            case TempWordType.StandardLose:
                StartCoroutine(TempWordCor(LevelManager.Instance.CurrentLevel.LostDialogue));
                break;
            case TempWordType.StandardWin:
                StartCoroutine(TempWordCor(LevelManager.Instance.CurrentLevel.WinDialogue));
                break;
            case TempWordType.EndlessEnd://idΪͨ������,30=0,40=1,50=2
                StartCoroutine(TempWordCor(LevelManager.Instance.CurrentLevel.WinDialogue, wordType.ID));
                break;
            case TempWordType.RefreshShop:
                if (Random.value > 0.95f)//��5%���ʴ���
                    StartCoroutine(TempWordCor(RefreshShop,Random.Range(0,RefreshShop.Words.Length-1)));
                break;
        }
    }


    IEnumerator TempWordCor(DialogueData dialogue, int id = 0)
    {
        typingSentence = true;
        SetGirlPos(1);
        ClickTip.SetActive(false);
        dialogue.TriggerGuideStartEvents();
        Show();
        backBtn.SetActive(false);
        dialogTxt.text = "";
        yield return new WaitForSeconds(0.5f);
        string word = GameMultiLang.GetTraduction(dialogue.Words[id]);
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
        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        backBtn.SetActive(false);
        currentDialogue.TriggerGuideEndEvents();
        if (CurrentGuideIndex < m_Dialogues.Length - 1)//����������һ���̳̣���������һ���̳�
        {
            CurrentGuideIndex++;
            currentDialogue = m_Dialogues[CurrentGuideIndex];
            GuideTrigger(TutorialType.None);
        }
        else
        {
            Hide();
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
        ClickTip.SetActive(true);

        typingSentence = true;
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


    public void GuideTrigger(TutorialType triggetType = TutorialType.None)
    {
        if (!Game.Instance.Tutorial)
            return;
        if (currentDialogue.JudgeConditions(triggetType))
        {
            currentDialogue.TriggerGuideStartEvents();
            Invoke(nameof(StartDialogue), currentDialogue.WaitingTime);
        }
    }


    public void NextBtnClick()
    {
        if (!typingSentence)
            DisplayNextSentence();
    }

    public void Show()
    {
        m_RootUI.gameObject.SetActive(true);
        Sound.Instance.PlayEffect("Sound_Guide");
        anim.SetBool("Show", true);
    }
    public void Hide()
    {
        anim.SetBool("Show", false);
    }
    public void HideRoot()
    {
        m_RootUI.gameObject.SetActive(false);
    }

    public void SetGirlPos(int posID)
    {
        switch (posID)
        {
            case 0:
                m_GirlTr.anchorMin = new Vector2(0.5f, 0);
                m_GirlTr.anchorMax = new Vector2(0.5f, 0);
                m_GirlTr.anchoredPosition = new Vector2(0, 250);
                break;
            case 1:
                m_GirlTr.anchorMin = new Vector2(0f, 0);
                m_GirlTr.anchorMax = new Vector2(0f, 0);
                m_GirlTr.anchoredPosition = new Vector2(380, 100);
                break;
        }
    }


}
