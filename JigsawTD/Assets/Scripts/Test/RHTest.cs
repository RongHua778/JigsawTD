using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RHTest : MonoBehaviour
{
    [SerializeField] GameObject panel = default;
    [SerializeField] InputField moneyInputField = default;

    [SerializeField] InputField drawInputField = default;

    [SerializeField] InputField compositeInputField = default;

    [SerializeField] InputField qualityInputField = default;
    [SerializeField] InputField elementInputField = default;

    [SerializeField] InputField trapInputField = default;

    public void MenuBtnClick()
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void GetMoneyClick()
    {
        LevelUIManager.Instance.PlayerCoin += int.Parse(moneyInputField.text);
    }

    public void GetDrawClick()
    {
        LevelUIManager.Instance.LotteryDraw += int.Parse(drawInputField.text);
    }

    //public void GetCompositeClick()
    //{
    //    GameManager.Instance.GetCompositeAttributeByName(compositeInputField.text);
    //}

    //public void GetElementClick()
    //{
    //    GameManager.Instance.GetTestElement(int.Parse(qualityInputField.text), int.Parse(elementInputField.text));
    //}

    //public void GetTrapClick()
    //{
    //    GameManager.Instance.GetTrapByName(trapInputField.text);

    //}
}
