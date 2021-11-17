using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RHTest : MonoBehaviour
{
    [SerializeField] GameObject panel = default;
    [SerializeField] InputField moneyInputField = default;
    [SerializeField] InputField lifeInputField = default;

    [SerializeField] InputField drawInputField = default;

    [SerializeField] InputField compositeInputField = default;
    [SerializeField] InputField e1 = default;
    [SerializeField] InputField e2 = default;
    [SerializeField] InputField e3 = default;


    [SerializeField] InputField qualityInputField = default;
    [SerializeField] InputField elementInputField = default;

    [SerializeField] InputField trapInputField = default;

   // [SerializeField] InputField waveStateField = default;
    [SerializeField] WaveSystem waveSystem = default;


    private void Start()
    {
        compositeInputField.text = "F1";
        e1.text = "0";
        e2.text = "0";
        e3.text = "0";
    }
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
        ConstructHelper.GetCompositeTurretByNameAndElement(compositeInputField.text, int.Parse(e1.text), int.Parse(e2.text), int.Parse(e3.text));
    }

    public void GetElementClick()
    {
        ConstructHelper.GetElementTurretByQualityAndElement((ElementType)int.Parse(elementInputField.text), int.Parse(qualityInputField.text));
    }

    public void GetTrapClick()
    {
        ConstructHelper.GetTrapShapeByName(trapInputField.text);
    }

    public void SetWaveBtnClick()
    {
        //waveSystem.waveStage = float.Parse(waveStateField.text);
        // waveSystem.waveCoolDown = float.Parse(waveCoolDownField.text);
        waveSystem.LevelInitialize();
        GameManager.Instance.PrepareNextWave();
    }

    public void GetLifeBtnClick()
    {
        GameRes.Life += int.Parse(lifeInputField.text);
    }

}
