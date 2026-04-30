using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject tutorialPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
        Time.timeScale = 1f;
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Quiting...");
        Application.Quit();
    }
}