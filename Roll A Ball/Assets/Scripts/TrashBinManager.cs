using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrashBinManager : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Referencia al texto en la UI
    public int binID; // ID único para cada bote
    private static int ballsInserted = 0; // Contador global de pelotas introducidas

    //Variable para los sonidos
    public AudioClip backgroundSound;
    public AudioClip coinReceived;
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.clip = backgroundSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es una pelota
        if (other.CompareTag("Ball"))
        {
            // Obtén el componente Ball para verificar si la pelota tiene el ID correcto
            Ball ball = other.GetComponent<Ball>();
            if (ball != null && ball.targetBinID == binID) // Compara IDs
            {
                ballsInserted++; // Incrementa el contador
                Destroy(other.gameObject); // Elimina la pelota de la escena
                gameObject.SetActive(false); // Desactiva este bote

                // Cambia el mensaje dependiendo de cuántas pelotas se han introducido
                if (ballsInserted == 1)
                {
                    audioSource.PlayOneShot(coinReceived);
                    messageText.text = "Has introducido una bola.";
                    Invoke("ClearMessage", 5f);
                }
                else if (ballsInserted == 2)
                {
                    audioSource.PlayOneShot(coinReceived);
                    messageText.text = "Has introducido la última bola.\n¡Felicidades! Has terminado esta simulación.";
                    Invoke("ClearMessage", 5f);

                    // Cerrar la aplicación después de 3 segundos
                    Invoke("ReturnToMainMenu", 5f);
                }
            }
        }
    }

    private void ClearMessage()
    {
        messageText.text = ""; // Limpia el texto
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Regresa al menú principal
    }
}
