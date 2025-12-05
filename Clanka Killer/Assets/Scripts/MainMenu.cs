using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level_1"); // replace with your scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game quit");
    }
}
