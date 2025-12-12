using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;    
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Level_1"); // replace with your scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game quit");
    }
    public void MainMenu1()
    {
        SceneManager.LoadScene("MainMenu");
    }

    
}
