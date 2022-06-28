using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Selections selections;


    public TextMeshProUGUI changingText;


    private void Awake()
    {
        InputUIEvents.GetInstance().Up += UpCallback;
        InputUIEvents.GetInstance().Down += DownCallback;
        InputUIEvents.GetInstance().Enter += SelectCallback;
        InputUIEvents.GetInstance().Back += BackCallback;
    }

    private void OnDestroy()
    {
        InputUIEvents.GetInstance().Up -= UpCallback;
        InputUIEvents.GetInstance().Down -= DownCallback;
        InputUIEvents.GetInstance().Enter -= SelectCallback;
        InputUIEvents.GetInstance().Back -= BackCallback;
    }

    public void changeHintText(string hint)
    {
        changingText.text = hint;
    }

    public void onClickContinue()
    {
        StartCoroutine(LoadGame());
    }

    public void onClickNewGame()
    {
        SaveManager.GetInstance().New();

        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        while (load.isDone == false)
        {
            yield return null;
            Debug.Log($"{name} Loaded");
        }
    }


    private IEnumerator LoadGame()
    {


        yield return StartCoroutine(LoadScene("Loading", LoadSceneMode.Additive));



        foreach (var scene in SaveManager.GetInstance().Current.scenes)
        {
            yield return StartCoroutine(LoadScene(scene, LoadSceneMode.Additive));
        }

        yield return StartCoroutine(LoadScene("Players", LoadSceneMode.Additive));

        SceneManager.UnloadSceneAsync("Loading");


        SceneManager.UnloadSceneAsync("Main Menu UI");
    }




    public void onClickExit()
    {
        Application.Quit();

    }



    public void UpCallback()
    {
        selections.Up();
    }

    public void DownCallback()
    {
            selections.Down();
    }

    public void SelectCallback()
    {
        selections.Select();

    }

    private void BackCallback()
    {
        SceneManager.LoadScene("Splash");
    }
}
