using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject messagePanel = default;
    [SerializeField] Text messageTxt = default;
    [SerializeField] UILevelSelect levelPanel = default;
    Animator m_Anim;


    bool gameStart = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Anim = this.GetComponent<Animator>();
        Sound.Instance.PlayBg("menu");
        Game.Instance.Difficulty = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowMessage(GameMultiLang.GetTraduction("TEST1"));
            PlayerPrefs.DeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShowMessage(GameMultiLang.GetTraduction("TEST2"));
            PlayerPrefs.SetInt("MaxPassLevel", 3);
        }
    }

    public void StartGameBtnClick()
    {
        m_Anim.SetBool("OpenLevel", true);
        levelPanel.SetLevelInfo();
    }

    public void LevelBackClick()
    {
        m_Anim.SetBool("OpenLevel", false);
    }

    public void BluePrintBtnClick()
    {
        ShowMessage("暂未开放");
    }

    public void SettingBtnClick()
    {
        ShowMessage("暂未开放");
    }

    private void ShowMessage(string content)
    {
        StartCoroutine(MessageCor(content));
    }

    IEnumerator MessageCor(string content)
    {
        messagePanel.SetActive(true);
        messageTxt.text = content;
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);
        messageTxt.text = "";
    }

    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }

    //public void DifficultBtnClick(int value)
    //{
    //    Game.Instance.Difficulty += value;
    //    if (Game.Instance.Difficulty > Game.Instance.MaxDifficulty)
    //    {
    //        Game.Instance.Difficulty = 1;
    //    }
    //    else if (Game.Instance.Difficulty < 1)
    //    {
    //        Game.Instance.Difficulty = Game.Instance.MaxDifficulty;
    //    }
    //}


}
