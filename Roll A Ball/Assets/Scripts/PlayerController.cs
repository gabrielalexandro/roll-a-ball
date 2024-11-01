using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Campos de texto asignados desde el inspector
    public TMP_Text countText;
    public TMP_Text winText;
    public TMP_Text lostText;
    public TMP_Text timerText;

    public float speed = 10.0f;
    public float jumpForce = 5.0f;

    private Rigidbody rb;
    private int count;

    // Variables del cronómetro
    private float timeRemaining = 30f;
    private bool timerIsRunning = true;

    // Variables para el movimiento
    private float movementX;
    private float movementY;
    private bool isGrounded = true;

    // Variables para mis sonidos
    public AudioClip backgroundSound;
    public AudioClip coinReceived;
    public AudioClip jump;
    public AudioClip ouch;
    public AudioClip gameOver;
    private AudioSource audioSource;

    void Start()
    {
        // Inicializa el puntaje, desactiva textos de victoria y derrota
        count = 0;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>(); // Obtengo el componente AudioSource

        winText.gameObject.SetActive(false);
        lostText.gameObject.SetActive(false);

        SetCountText();
        DisplayTime(timeRemaining); // Muestra el tiempo inicial

        // Reproduzco los audios
        audioSource.clip = backgroundSound;
        audioSource.loop = true;
        audioSource.Play();
        
    }

    void Update()
    {
        UpdateTimer();
    }

    // Maneja la física del movimiento del jugador
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    // Lee el movimiento del usuario
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Método para manejar el salto
    void OnJump()
    {
        if (isGrounded) // Solo salta si está en el suelo
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Evita que salte de nuevo hasta que toque el suelo
        }

        // Reproduce el sonido de salto
        audioSource.PlayOneShot(jump);
    }

    // Detecta si el jugador está en el suelo para permitir otro salto
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Si colisiona con el objeto enemigo con etiqueta "Enemy2"
        if (collision.gameObject.CompareTag("Enemy2"))
        {
            // Muestra el mensaje de pérdida y regresa al menú
            lostText.gameObject.SetActive(true); // Activa el texto de "You Lost!"

            audioSource.Stop(); // Detener el sonido de fondo
            audioSource.PlayOneShot(ouch);
            audioSource.PlayOneShot(gameOver); // Reproduzco sonido de la derrota

            StartCoroutine(ReturnToMainMenuAfterDelay());
        }
    }

    // Maneja las colisiones con objetos etiquetados como "PickUp"
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            audioSource.PlayOneShot(coinReceived); //Sonido por cada objeto obtenido
        }
    }

    // Actualiza el puntaje y verifica la condición de victoria
    void SetCountText()
    {
        countText.text = "Points: " + count.ToString();

        // Si el jugador recoge todos los objetos (por ejemplo, 12)
        if (count >= 12)
        {
            timerIsRunning = false; // Detiene el cronómetro
            winText.gameObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy2")); // Destruye el enemigo si gano
            StartCoroutine(LoadNextLevelAfterDelay()); // Espera y carga el próximo nivel
        }
    }

    // Corrutina para cargar el próximo nivel después de ganar
    IEnumerator LoadNextLevelAfterDelay()
    {
        yield return new WaitForSeconds(5); // Espera 5 segundos

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Verifica si hay otro nivel
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Carga la siguiente escena
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Si no hay otro nivel, regresa al menú principal
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Actualiza el cronómetro y verifica la condición de derrota
    void UpdateTimer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                TriggerLoss(); // Activa la lógica de derrota
            }
        }
    }

    // Muestra el tiempo restante en formato de minutos:segundos
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Muestra el mensaje de derrota y vuelve al menú principal después de 5 segundos
    void TriggerLoss()
    {
        lostText.gameObject.SetActive(true); // Activa el texto de "You Lost!"

        audioSource.Stop(); // Detener el sonido de fondo
        audioSource.PlayOneShot(gameOver); // Reproduzco sonido GameOver

        StartCoroutine(ReturnToMainMenuAfterDelay());
    }

    // Corrutina para regresar al menú principal
    IEnumerator ReturnToMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(5); // Espera 5 segundos
        SceneManager.LoadScene("MainMenu"); // Carga la escena del menú principal
    }
}
