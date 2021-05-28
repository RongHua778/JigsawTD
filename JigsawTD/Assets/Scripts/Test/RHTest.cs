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

    public void GetCompositeClick()
    {
        foreach (TurretAttribute attribute in StaticData.Instance.CompositionAttributes)
        {
            if (attribute.Name == compositeInputField.text)
            {
                GameManager.Instance.GetTestShape(attribute);
            }
        }
    }

    public void GetElementClick()
    {
        GameManager.Instance.GetTestElement(int.Parse(qualityInputField.text), int.Parse(elementInputField.text));
    }
}
