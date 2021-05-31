using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] Game m_Game = default;
    [SerializeField] Text difficultyTxt = default;
    // Start is called before the first frame update
    void Start()
    {
        Sound.Instance.PlayBg("menu");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGameBtnClick()
    {
        Game.Instance.LoadScene(1);
    }

    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }

    public void DifficultBtnClick(int value)
    {
        m_Game.Difficulty += value;
        if (m_Game.Difficulty > 3)
        {
            m_Game.Difficulty = 1;
        }
        else if (m_Game.Difficulty < 1)
        {
            m_Game.Difficulty = 3;
        }
        difficultyTxt.text = "дя╤х" + m_Game.Difficulty;
    }
}
