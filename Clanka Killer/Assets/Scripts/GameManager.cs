using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject deathScreen;   // Assign your UI panel
    private bool isDead = false;

    public void PlayerDied()
    {
        if (isDead) return;
        isDead = true;

        // Show UI
        deathScreen.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;
        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    
}

    public void Restart()
    {
        Debug.Log("Button Clicked");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
