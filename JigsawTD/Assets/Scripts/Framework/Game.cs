using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Sound))]
public class Game : Singleton<Game>
{
    public int Difficulty = 1;
    public Animator transition;
    public float transitionTime = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);

    }
    public void LoadScene(int index)
    {
        StartCoroutine(Transition(index));
    }

    public void LoadNextLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    IEnumerator Transition(int index)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
        transition.SetTrigger("End");

    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
