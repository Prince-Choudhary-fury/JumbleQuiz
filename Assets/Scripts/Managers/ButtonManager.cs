using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public AudioSource tap;


    public void PlayButton()
    {
        StartCoroutine(Button(1, 0));
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void Retry()
    {
        QuizController.currentLevel -= 1;
        StartCoroutine(Button(1, QuizController.currentLevel));
    }

    public void Next()
    {
        StartCoroutine(Button(1, QuizController.currentLevel));
    }

    public void Menu()
    {
        StartCoroutine(Button(0, 0));
    }

    //functionality of all the buttons
    IEnumerator Button(int sceneIndex, int levelIndex)
    {
        tap.Play();
        yield return new WaitForSeconds(0.6f);
        Debug.Log(levelIndex);
        QuizController.isLevel[levelIndex] = true;
        SceneManager.LoadScene(sceneIndex);
    }

}
