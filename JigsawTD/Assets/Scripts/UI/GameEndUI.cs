using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : IUserInterface
{
    [SerializeField] Text title = default;
    [SerializeField] Text totalCompositeTxt = default;
    [SerializeField] Text totalDamageTxt = default;
    [SerializeField] Text levelHighScore = default;

    private static int totalComposite = 0;
    private static int totalDamage = 0;

    public static int TotalComposite { get => totalComposite; set => totalComposite = value; }
    public static int TotalDamage { get => totalDamage; set => totalDamage = value; }

    public void SetGameResult(int turn)
    {
        title.text = "ͨ��" + turn + "��";
        int maxLevel = LevelManager.Instance.LevelMaxTurn;
        if (turn > maxLevel)
        {
            LevelManager.Instance.LevelMaxTurn = turn;
        }
        levelHighScore.text = LevelManager.Instance.LevelMaxTurn.ToString();
        totalCompositeTxt.text = TotalComposite.ToString();
        totalDamageTxt.text = TotalDamage.ToString();
    }

    public void ReturnToMenu()
    {
        if (Game.Instance != null)
        {
            Game.Instance.LoadScene(0);
        }
    }



}
