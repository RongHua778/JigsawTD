using UnityEngine;
using UnityEngine.UI;

public class LangDropDown : MonoBehaviour
{
    [SerializeField] string[] myLangs;
    Dropdown drp;
    int index;

    void Start()
    {
        drp = this.GetComponent<Dropdown>();
        int v = 0;
        string lang = PlayerPrefs.GetString("_language");
        for (int i = 0; i < myLangs.Length; i++)
        {
            if (lang == myLangs[i])
            {
                v = i;
            }
        }

        drp.value = v;

        drp.onValueChanged.AddListener(delegate
        {
            index = drp.value;
            PlayerPrefs.SetString("_language", myLangs[index]);
            Debug.Log("language changed to " + myLangs[index]);
            //apply changes
            ApplyLanguageChanges();
        });
    }

    void ApplyLanguageChanges()
    {
        Game.Instance.ReloadScene();
    }

}
