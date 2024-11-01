using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Método que será llamado al hacer clic en el botón Start
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // Método que será llamado al hacer clic en el botón Exit
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting"); // Esto solo se verá en el editor
    }
}
