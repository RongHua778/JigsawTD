using System;
using UnityEngine;
using System.Collections.Generic;
using Lanuguage;


// Created by: Hamza Herbou        (mobile games developer)
// email     : hamza95herbou@gmail.com


public class GameMultiLang : MonoBehaviour
{
	public static GameMultiLang Instance;

	public static Dictionary<String, String> Fields;

	[SerializeField] string defaultLang = default;
	[SerializeField] LanguageManager languageData = default;


	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
			//DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
		LoadLanguage();

	}


	public void LoadLanguage ()
	{
		if (Fields == null)
			Fields = new Dictionary<string, string> ();
		
		Fields.Clear ();
		string lang = PlayerPrefs.GetString ("_language", defaultLang);
		PlayerPrefs.SetString("_language", lang);
		switch (lang)
		{
			case "en":
				PlayerPrefs.SetInt("_language_index", 0);
				break;
			case "ch":
				PlayerPrefs.SetInt("_language_index", 1);
				break;
		}



		//string allTexts = (Resources.Load (@"Languages/" + lang) as TextAsset).text; //without (.txt)

		//string[] lines = allTexts.Split (new string[] { "\r\n", "\n" }, StringSplitOptions.None);
		//string key, value;

		//for (int i = 0; i < lines.Length; i++) {
		//	if (lines [i].IndexOf ("=") >= 0 && !lines [i].StartsWith ("#")) {
		//		key = lines [i].Substring (0, lines [i].IndexOf ("="));
		//		value = lines [i].Substring (lines [i].IndexOf ("=") + 1,
		//			lines [i].Length - lines [i].IndexOf ("=") - 1).Replace ("\\n", Environment.NewLine);
		//		Fields.Add (key, value);
		//	}
		//}

		LoadLanguageData();
	}


    private void LoadLanguageData()
    {
        switch (PlayerPrefs.GetInt("_language_index", 0))
        {
            case 0:
				for (int i = 0; i < languageData.dataArray.Length; i++)
				{
					if (Fields.ContainsKey(languageData.dataArray[i].Key))
					{
						Debug.Log(languageData.dataArray[i].Key);
					}
					else
						Fields.Add(languageData.dataArray[i].Key, languageData.dataArray[i].English);
				}
				break;
            case 1:
				for (int i = 0; i < languageData.dataArray.Length; i++)
				{
                    if (Fields.ContainsKey(languageData.dataArray[i].Key))
                    {
						Debug.Log(languageData.dataArray[i].Key);
                    }else
					Fields.Add(languageData.dataArray[i].Key, languageData.dataArray[i].Chinese);
				}
				break;
        }


    }

    public static string GetTraduction (string key)
	{
		if (!Fields.ContainsKey (key)) {
			Debug.LogError ("There is no key with name: [" + key + "] in your text files");
			return null;
		}

		return Fields [key];
	}


}
