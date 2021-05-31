using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject messagePanel = default;
    [SerializeField] Text messageTxt = default;
    //[SerializeField] Game m_Game = default;
    [SerializeField] Text difficultyTxt = default;
    bool gameStart = false;
    // Start is called before the first frame update
    void Start()
    {
        Sound.Instance.PlayBg("menu");
        Game.Instance.Difficulty = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowMessage("测试：已清除了存档");
            Debug.Log("删除了通关记录");
            PlayerPrefs.DeleteAll();
        }
    }

    public void StartGameBtnClick()
    {
        if (PlayerPrefs.GetInt("MaxPassLevel", 1) < Game.Instance.Difficulty)
        {
            ShowMessage("需先通关上一级难度");
            return;
        }
        if (!gameStart)
        {
            Game.Instance.LoadScene(1);
            gameStart = true;
        }

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

    public void DifficultBtnClick(int value)
    {
        Game.Instance.Difficulty += value;
        if (Game.Instance.Difficulty > 3)
        {
            Game.Instance.Difficulty = 1;
        }
        else if (Game.Instance.Difficulty < 1)
        {
            Game.Instance.Difficulty = 3;
        }
        difficultyTxt.text = "难度" + Game.Instance.Difficulty;
    }
}
