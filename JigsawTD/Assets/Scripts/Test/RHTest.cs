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

    [SerializeField] InputField waveStateField = default;
    [SerializeField] InputField waveCoolDownField = default;
    [SerializeField] WaveSystem waveSystem = default;

    public void MenuBtnClick()
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void GetMoneyClick()
    {
        GameManager.Instance.GainMoney(int.Parse(moneyInputField.text));
    }

    public void GetDrawClick()
    {
        GameManager.Instance.GainDraw(int.Parse(drawInputField.text));
    }

    public void GetCompositeClick()
    {
        ConstructHelper.GetCompositeTurretByName(compositeInputField.text);
    }

    public void GetElementClick()
    {
        ConstructHelper.GetElementTurretByQualityAndElement((Element)int.Parse(elementInputField.text), int.Parse(qualityInputField.text));
    }

    public void GetTrapClick()
    {
        ConstructHelper.GetTrapByName(trapInputField.text);
    }

    public void SetWaveBtnClick()
    {
        waveSystem.waveStage = float.Parse(waveStateField.text);
       // waveSystem.waveCoolDown = float.Parse(waveCoolDownField.text);
        waveSystem.LevelInitialize();
        GameManager.Instance.PrepareNextWave();
    }

}
