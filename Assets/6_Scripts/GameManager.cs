using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    [Header("Environment Root")]
    public GameObject environmentRoot;
    // This is the parent GameObject for your whole scene (track, obstacles, world).
    // Drag your entire environment folder into this field in Unity.
    // We'll hide/show this on game start & quit.

    [Header("Gameplay")]
    //public PlayerController player;     // Your player script
    //public TrackSpawner trackSpawner;   // Your track generator script

    private Gamepad gamepad;
    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isPlaying = false;

    void Start()
    {
        // Show Main Menu UI at launch
        mainMenuPanel.SetActive(true);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Start with environment OFF until Start Game is pressed
        if (environmentRoot != null) environmentRoot.SetActive(false);

        // Disable player movement
        //player.enabled = false;
    }

    void Update()
    {
        gamepad = Gamepad.current;
        if (gamepad == null) return;

        // ---------------------------
        // MAIN MENU INPUT
        // ---------------------------
        if (mainMenuPanel.activeSelf)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame) StartGame();     // A
            if (gamepad.buttonNorth.wasPressedThisFrame) ShowAbout();    // Y
            if (gamepad.buttonEast.wasPressedThisFrame) QuitGame();      // B
            return;
        }

        // ---------------------------
        // ABOUT MENU (if you add one later)
        // ---------------------------
        // Example: If you decide to use it, handle it here

        // ---------------------------
        // PAUSE MENU INPUT
        // ---------------------------
        if (pausePanel.activeSelf)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame) ResumeGame();    // A
            if (gamepad.buttonNorth.wasPressedThisFrame) RestartGame();   // Y
            if (gamepad.buttonEast.wasPressedThisFrame) QuitToMenu();     // B
            return;
        }

        // ---------------------------
        // GAME OVER MENU INPUT
        // ---------------------------
        if (gameOverPanel.activeSelf)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame) RestartGame();   // A
            if (gamepad.buttonEast.wasPressedThisFrame) QuitToMenu();     // B
            return;
        }

        // ---------------------------
        // IN-GAME PAUSE
        // ---------------------------
        if (isPlaying && gamepad.startButton.wasPressedThisFrame)
        {
            PauseGame();
        }
    }

    // --------------------------
    // GAME STATE FUNCTIONS
    // --------------------------

    public void StartGame()
    {
        // Hide menu
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Activate the environment
        if (environmentRoot != null) environmentRoot.SetActive(true);

        // Flag the game as active
        isPlaying = true;
        isGameOver = false;

        // Resume normal time
        Time.timeScale = 1f;

        // Reset level (clear old chunks, spawn new ones)
        //trackSpawner.ResetSpawner();

        // Reset player position and states
        //player.ResetPlayer();
        //player.enabled = true;
    }

    public void PauseGame()
    {
        isPaused = true;

        pausePanel.SetActive(true);

        // Freeze gameplay
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        pausePanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void EndGame()
    {
        isGameOver = true;
        isPlaying = false;

        // Stop player movement
        //player.enabled = false;

        // Freeze world
        Time.timeScale = 0f;

        // Show Game Over panel on top of environment
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Hide UI
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

        isGameOver = false;

        // Restart time
        Time.timeScale = 1f;

        // Reset environment (track)
        //trackSpawner.ResetSpawner();

        // Reset player
        //player.ResetPlayer();
        //player.enabled = true;

        isPlaying = true;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Go back to main menu
        mainMenuPanel.SetActive(true);

        // Disable player movement
        isPlaying = false;
        //player.enabled = false;

        // Hide entire environment Root
        if (environmentRoot != null) environmentRoot.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ShowAbout()
    {
        // OPTIONAL: Add your About panel later if needed.
    }
}
