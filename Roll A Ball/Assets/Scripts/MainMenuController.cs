using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // M�todo que ser� llamado al hacer clic en el bot�n Start
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    // M�todo que ser� llamado al hacer clic en el bot�n Exit
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting"); // Esto solo se ver� en el editor
    }
}
