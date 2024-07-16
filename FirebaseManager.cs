/**
 * FirebaseManager for Unity
 * 
 * This script manages interactions between Unity and Firebase Realtime Database, enabling functionalities
 * such as adding, updating, and fetching leaderboard entries. It facilitates the communication between
 * Unity and Firebase, ensuring that the game can store and retrieve data efficiently.
 * 
 * Key functionalities provided by this script include:
 * - Checking if Firebase is initialised and ready to use.
 * - Adding new score entries to the Firebase database.
 * - Fetching a single score entry from the database.
 * - Fetching the top N score entries from the database.
 * - Updating the Unity UI with the retrieved score data.
 * 
 * This script implements a singleton pattern to ensure a single instance manages all Firebase operations.
 * It uses external JavaScript methods (defined in a .jslib file) to perform Firebase operations, enabling
 * seamless integration with the web environment in which the Unity WebGL build runs.
 * 
 * Note:
 * - Ensure that Firebase is correctly initialised in the hosting HTML page
 *           This will be the WebGLTemplates\CustomTemplate\index.html file
 *           And Ultimately the index.html file for each build.
 * - Ensure that the Unity GameObject and method names match the ones used in this script.
 */

using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Manages interactions with Firebase, including initialisation check, saving scores,
/// and loading single or multiple entries from the database.
/// </summary>
 
public class FirebaseManager : MonoBehaviour
{
    // Singleton instance of FirebaseManager
    public static FirebaseManager Instance { get; private set; }

    // Indicates if Firebase is initialised and ready
    public bool IsFirebaseReady { get; private set; } = false;
    
    // Event triggered when a single top score is fetched
    public event Action<string, int> OnTopScoreFetched;

    // Event triggered when multiple top scores are fetched
    public event Action<List<Score>> OnTopScoresFetched;

    // External methods implemented in JavaScript for Firebase operations
    // See firebase.jslib file in Plugins folder
    [DllImport("__Internal")]
    private static extern int CheckFirebaseInitialisation();

    [DllImport("__Internal")]
    private static extern void AddEntryToDatabase(string jsonEntry);

    [DllImport("__Internal")]
    private static extern void FetchSingleEntry();

    [DllImport("__Internal")]
    private static extern void FetchTopEntries(int count);

    /// <summary>
    /// Initialises the singleton instance and ensures it persists across scenes.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
            InitialiseFirebase(); // Check Firebase initialisation on startup
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    /// <summary>
    /// Checks if Firebase has been initialised and sets the IsFirebaseReady flag.
    /// </summary>
    void InitialiseFirebase()
    {
        int initialisationResult = CheckFirebaseInitialisation();
        IsFirebaseReady = initialisationResult == 1; // Firebase is ready if result is 1
        if (!IsFirebaseReady)
        {
            Debug.LogError("Firebase initialisation failed.");
        }
    }

    /// <summary>
    /// Saves a player's score to the database.
    /// </summary>
    /// <param name="playerName">The name of the player.</param>
    /// <param name="score">The score of the player.</param>
    public void SaveScore(string playerName, int score)
    {
        if (IsFirebaseReady)
        {
            var entry = new Score { name = playerName, score = score };
            var jsonEntry = JsonUtility.ToJson(entry); // Convert the score entry to JSON
            AddEntryToDatabase(jsonEntry); // Save to database
        }
        else
        {
            Debug.LogError("Firebase is not ready. Cannot save score.");
        }
    }

    /// <summary>
    /// Loads a single entry from the database.
    /// </summary>
    public void LoadSingleEntry()
    {
        if (IsFirebaseReady)
        {
            FetchSingleEntry(); // Fetch a single entry if Firebase is ready
        }
        else
        {
            Debug.LogError("Firebase is not ready. Cannot load single entry.");
        }
    }

    /// <summary>
    /// Loads the top entries from the database. The number of entries to load is 
    /// dictated by the count value which is set in UIButtonHandler.cs
    /// OnLoadTopScoresButtonClicked()
    /// </summary>
    /// <param name="count">The number of top entries to load.</param>
    public void LoadTopEntries(int count)
    {
        if (IsFirebaseReady)
        {
            FetchTopEntries(count); // Fetch top entries if Firebase is ready
        }
        else
        {
            Debug.LogError("Firebase is not ready. Cannot load top entries.");
        }
    }

    /// <summary>
    /// Called by JavaScript when a single entry is loaded.
    /// </summary>
    /// <param name="jsonEntry">The JSON string of the entry.</param>
    public void OnSingleEntryLoaded(string jsonEntry)
    {
        ProcessEntry(jsonEntry); // Process the loaded entry
    }

    /// <summary>
    /// Called by JavaScript when multiple entries are loaded.
    /// </summary>
    /// <param name="jsonEntries">The JSON string of the entries.</param>
    public void OnTopEntriesLoaded(string jsonEntries)
    {
        ProcessEntries(jsonEntries); // Process the loaded entries
    }

    /// <summary>
    /// Processes a single entry JSON string and updates the UI.
    /// </summary>
    /// <param name="jsonEntry">The JSON string of the entry.</param>
    private void ProcessEntry(string jsonEntry)
    {
        try
        {
            var entry = JsonUtility.FromJson<Score>(jsonEntry); // Convert JSON to Score object
            if (entry != null)
            {
                UIButtonHandler.Instance.UpdateScoreText(entry.name, entry.score); // Update UI
                OnTopScoreFetched?.Invoke(entry.name, entry.score); // Trigger event
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing JSON: {ex.Message}"); // Log any errors
        }
    }

    /// <summary>
    /// Processes multiple entries JSON string and updates the UI.
    /// </summary>
    /// <param name="jsonEntries">The JSON string of the entries.</param>
    private void ProcessEntries(string jsonEntries)
    {
        try
        {
            var scores = JsonHelper.FromJson<Score>(jsonEntries); // Convert JSON to array of Score objects
            if (scores != null && scores.Length > 0)
            {
                UIButtonHandler.Instance.UpdateTopScoresText(scores.ToList()); // Update UI
                OnTopScoresFetched?.Invoke(scores.ToList()); // Trigger event
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing JSON: {ex.Message}"); // Log any errors
        }
    }
}

/// <summary>
/// Represents a score entry with a player's name and score.
/// </summary>
[Serializable]
public class Score
{
    public string name;
    public int score;
}
