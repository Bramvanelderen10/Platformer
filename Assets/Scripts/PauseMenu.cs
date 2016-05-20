using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public string leveSelect;

    public string currentLevel;

    public string mainMenu;

    public bool isPaused;

    public GameObject pauseMenuCanvas;
    
	// Update is called once per frame
	void Update () {
        if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        } else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(currentLevel);
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentLevel);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
