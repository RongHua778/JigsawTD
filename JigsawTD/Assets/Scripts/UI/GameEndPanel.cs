using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPanel : MonoBehaviour
{
    [SerializeField] Text title = default;
    [SerializeField] Text totalCompositeTxt = default;
    [SerializeField] Text totalDamageTxt = default;

    private static int totalComposite = 0;
    private static int totalDamage = 0;

    public static int TotalComposite { get => totalComposite; set => totalComposite = value; }
    public static int TotalDamage { get => totalDamage; set => totalDamage = value; }

    public void ShowGameEndPanel(bool win)
    {
        if (win)
        {
            title.text = "ͨ���Ѷ�" + GameManager.Instance.Difficulty;
        }
        else
        {
            title.text = "����";
        }
        totalCompositeTxt.text = TotalComposite.ToString();
        totalDamageTxt.text = TotalDamage.ToString();

    }

    public void ReturnToMenu()
    {
        if (Game.Instance != null)
        {
            Game.Instance.LoadScene(0);
        }
        Debug.LogWarning("GAME������");
    }

    

}