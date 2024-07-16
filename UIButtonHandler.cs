/**
 * UIButtonHandler for Unity
 * 
 * This script manages interactions between Unity UI buttons and the FirebaseManager. It handles user
 * input through button clicks to save scores, load a single score, and load the top N scores from the
 * Firebase Realtime Database. Additionally, it updates the UI TextMeshPro elements with the retrieved
 * score data.
 * 
 * Key functionalities provided by this script include:
 * - Handling button click events to trigger Firebase operations.
 * - Updating the UI with the latest score data fetched from the Firebase database.
 * 
 * Note:
 * - Ensure that the UI buttons and TextMeshPro elements are correctly assigned in the Unity Inspector.
 * - Ensure that the FirebaseManager script is correctly integrated and operational.
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages interactions between Unity UI buttons and the FirebaseManager. Handles button clicks
/// to save scores, load a single score, and load the top N scores from the Firebase Realtime Database.
/// Updates the UI TextMeshPro elements with the retrieved score data.
/// </summary>
public class UIButtonHandler : MonoBehaviour
{
    // Singleton instance of UIButtonHandler
    public static UIButtonHandler Instance { get; private set; }

    // UI elements for button interactions and displaying scores
    public Button saveScoreButton;
    public Button loadScoreButton;
    public Button loadTopScoresButton;
    public TextMeshProUGUI scoresText;
    public TextMeshProUGUI topScoresText;

    /// <summary>
    /// Initializes the singleton instance and ensures there is only one instance of this script.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Sets up button click listeners and subscribes to FirebaseManager events.
    /// </summary>
    void Start()
    {
        // Add listeners for button clicks
        saveScoreButton.onClick.AddListener(OnSaveScoreButtonClicked);
        loadScoreButton.onClick.AddListener(OnLoadScoreButtonClicked);
        loadTopScoresButton.onClick.AddListener(OnLoadTopScoresButtonClicked);

        // Subscribe to the OnTopScoresFetched event from FirebaseManager
        FirebaseManager.Instance.OnTopScoresFetched += UpdateTopScoresText;
    }

    /// <summary>
    /// Unsubscribes from FirebaseManager events when the script is destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (FirebaseManager.Instance != null)
        {
            // Unsubscribe from the OnTopScoresFetched event
            FirebaseManager.Instance.OnTopScoresFetched -= UpdateTopScoresText;
        }
    }

    /// <summary>
    /// Handles the save score button click event.
    /// </summary>
    void OnSaveScoreButtonClicked()
    {
        // Save a test score with a player name "TestPlayer" and a score of 1234
        FirebaseManager.Instance.SaveScore("TestPlayer", 1234);
        Debug.Log("Test score saved.");
    }

    /// <summary>
    /// Handles the load single score button click event.
    /// </summary>
    void OnLoadScoreButtonClicked()
    {
        Debug.Log("LoadScoreButton clicked");
        FirebaseManager.Instance.LoadSingleEntry();
    }

    /// <summary>
    /// Handles the load top scores button click event.
    /// </summary>
    void OnLoadTopScoresButtonClicked()
    {
        Debug.Log("LoadTopScoresButton clicked");
        FirebaseManager.Instance.LoadTopEntries(5); // Change this number to load top N scores
    }

    /// <summary>
    /// Updates the UI TextMeshPro element with a single player's score.
    /// </summary>
    /// <param name="playerName">The name of the player.</param>
    /// <param name="score">The score of the player.</param>
    public void UpdateScoreText(string playerName, int score)
    {
        scoresText.text = $"Player: {playerName}, Score: {score}";
    }

    /// <summary>
    /// Updates the UI TextMeshPro element with the top players' scores.
    /// </summary>
    /// <param name="scores">A list of Score objects.</param>
    public void UpdateTopScoresText(List<Score> scores)
    {
        topScoresText.text = string.Join("\n", scores.Select(s => $"Player: {s.name}, Score: {s.score}"));
    }
}
