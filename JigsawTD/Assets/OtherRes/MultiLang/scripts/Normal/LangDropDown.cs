using UnityEngine;
using UnityEngine.UI;

public class LangDropDown : MonoBehaviour
{
	[SerializeField] string[] myLangs;
	Dropdown drp;
	int index;

	void Start ()
	{
		drp = this.GetComponent <Dropdown> ();
		int v = PlayerPrefs.GetInt ("_language_index", 0);
		drp.value = v;

		drp.onValueChanged.AddListener (delegate {
			index = drp.value;
			PlayerPrefs.SetInt ("_language_index", index);
			PlayerPrefs.SetString ("_language", myLangs [index]);
			Debug.Log ("language changed to " + myLangs [index]);
			//apply changes
			ApplyLanguageChanges ();
		});
	}

	void ApplyLanguageChanges ()
	{
		Game.Instance.ReloadScene();
	}

	void OnDestroy ()
	{
		drp.onValueChanged.RemoveAllListeners ();
	}
}
