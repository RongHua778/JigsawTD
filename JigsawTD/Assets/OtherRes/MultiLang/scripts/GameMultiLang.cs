using System;
using UnityEngine;
using System.Collections.Generic;
using Lanuguage;
using Sirenix.OdinInspector;


public class GameMultiLang : MonoBehaviour
{
	public static GameMultiLang Instance;

	public static Dictionary<String, String> Fields;
	public static Dictionary<string, Sprite> SpriteFields;

	[SerializeField] string defaultLang = default;
	[SerializeField] LanguageManager languageData = default;

	[SerializeField] List<SpriteData> SpriteDatas = default;
	public static List<LanguageData> LanguageData { get; set; }



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

	[Button]
	private void SetLanguage()
    {
		PlayerPrefs.SetString("_language", defaultLang);
	}

	[Button]
	public void LoadLanguage ()
	{
		if (Fields == null)
			Fields = new Dictionary<string, string> ();

		if (SpriteFields == null)
			SpriteFields = new Dictionary<string, Sprite>();
		
		Fields.Clear ();
		SpriteFields.Clear();

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

		//LanguageData = ExcelTool.CreateLanguageArrayWithExcel(ExcelConfig.excelsFolderPath + "LanguageExcel.xlsx");
		LanguageData = languageData.dataArray;
		switch (PlayerPrefs.GetInt("_language_index", 0))
        {
            case 0:
				for (int i = 0; i < LanguageData.Count; i++)
				{
					if (Fields.ContainsKey(LanguageData[i].Key))
					{
						Debug.Log(LanguageData[i].Key);
					}
					else
						Fields.Add(LanguageData[i].Key, LanguageData[i].English);
				}
				//图片
				for (int i = 0; i < SpriteDatas.Count; i++)
				{
					SpriteFields.Add(SpriteDatas[i].Key, SpriteDatas[i].English);
				}
				break;
            case 1:
				for (int i = 0; i < LanguageData.Count; i++)
				{
                    if (Fields.ContainsKey(LanguageData[i].Key))
                    {
						Debug.Log(LanguageData[i].Key);
                    }else
					Fields.Add(LanguageData[i].Key, LanguageData[i].Chinese);
				}
				//图片
				for (int i = 0; i < SpriteDatas.Count; i++)
				{
					SpriteFields.Add(SpriteDatas[i].Key, SpriteDatas[i].Chinese);
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

	public static Sprite GetSprite(string key)
	{
		if (!SpriteFields.ContainsKey(key))
		{
			Debug.LogError("There is no key with name: [" + key + "] in your sprite files");
			return null;
		}

		return SpriteFields[key];
	}



}
