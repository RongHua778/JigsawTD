using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BillboardItem : MonoBehaviour
{
    [SerializeField] Text rank_Txt = default;
    [SerializeField] Text name_Txt = default;
    [SerializeField] Text score_Txt = default;
    [SerializeField] Image medalImg = default;
    [SerializeField] Sprite[] medalSprites = default;

    public void SetContent(int rank, string name, int score)
    {
        if (rank <= 3)
        {
            medalImg.gameObject.SetActive(true);
            medalImg.sprite = medalSprites[rank - 1];
            rank_Txt.gameObject.SetActive(false);
        }
        else
        {
            rank_Txt.text = (rank + 1).ToString();
            rank_Txt.gameObject.SetActive(true);
            medalImg.gameObject.SetActive(false);
        }

        name_Txt.text = name;
        score_Txt.text = score.ToString();
    }

}
