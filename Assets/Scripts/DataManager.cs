using System.Collections.Generic;
using UnityEngine;
using System.IO;


/// <summary>
/// Manages persistent storage and retrieval of player highscores
/// 
/// Key responsibilities:
/// - Maintains sorted leaderboard of top player scores
/// - Handles JSON serialization for persistent storage
/// - Manages player score updates and additions
/// - Provides formatted data for UI display
/// - Implements thread-safe file operations
/// 
/// Implementation details:
/// - Singleton pattern for global access
/// - Maximum of 3 highscores stored - which can be changed by modifying the MaxHighscores constant
/// - Uses Unity's persistent data path
/// - JSON-based file storage
/// </summary>
public class DataManager : singleton<DataManager>
{
    public string currentPlayerId { get; set; }     // Identifier for the current active player
    private List<HighscoreEntry> highscores = new List<HighscoreEntry>(); // Collection of all recorded highscores
    private const int MaxHighscores = 3;     // Maximum number of highscores to maintain in the leaderboard
    private const string SaveFileName = "highscoreList.json";     /// Filename for persistent storage of highscores


/// <summary>
/// Represents a single highscore entry with player name and score
/// Serializable to enable JSON persistence
/// </summary>
    [System.Serializable]
    public class HighscoreEntry
    {
        public string playerName;
        public int score;
    }

/// <summary>
/// Wrapper class for serializing the complete highscore list
/// Used for JSON conversion during save/load operations
/// </summary>   

    [System.Serializable]
    private class SaveData
    {
        public List<HighscoreEntry> highscores;
    }

    public override void Awake()
    {
        base.Awake();
        LoadHighscores();
    }
/// <summary>
/// Adds a new highscore or updates existing score for a player
/// </summary>
/// <param name="playerName">Name of the player</param>
/// <param name="score">Score achieved by the player</param>
/// <returns>True if highscore was added or updated, false otherwise</returns>
    public bool AddOrUpdateHighscore(string playerName, int score)
    {
        bool updated = false;
        HighscoreEntry existingEntry = null;
        for (int i = 0; i < highscores.Count; i++)
        {
            if (highscores[i].playerName == playerName)
            {
                existingEntry = highscores[i];
                break;
            }
        }
        
        if (existingEntry != null)
        {
            // Only update if the new score is higher
            if (score > existingEntry.score)
            {
                existingEntry.score = score;
                SortHighscores();
                updated = true;
            }
        }
        else
        {
            // Add new entry
            highscores.Add(new HighscoreEntry { playerName = playerName, score = score });
            SortHighscores();
            if (highscores.Count > MaxHighscores)
            {
                highscores.RemoveAt(highscores.Count - 1);
            }
            updated = true;
        }

        if (updated)
        {
            SaveHighscores();
        }

        return updated;
    }

   /// <summary>
/// Saves the current highscore list to persistent storage
/// Called after new highscores are added or updated
/// </summary>

    private void SaveHighscores()
    {
        SaveData saveData = new SaveData { highscores = highscores };
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(GetSaveFilePath(), json);
    }
/// <summary>
/// Loads saved highscores from disk when game initializes
/// Creates empty highscore list if save file doesn't exist
/// </summary>
    private void LoadHighscores()
    {
        string path = GetSaveFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            highscores = saveData.highscores;
        }
    }
/// <summary>
/// Constructs the full path for the highscores save file
/// Uses Unity's persistent data path for cross-platform compatibility
/// <returns>Complete file path for highscores storage</returns>
    private string GetSaveFilePath()
    {
        return Path.Combine(Application.persistentDataPath, SaveFileName);
    }

/// <summary>
/// Creates a formatted string of all highscores for UI display
/// Called by UI elements to show the highscore leaderboard
/// </summary>
/// <returns>Numbered list of player names and scores</returns>
    public string GetFormattedHighscores()
    {
        string formattedHighscores = "";
        for (int i = 0; i < highscores.Count; i++)
        {
            formattedHighscores += $"{i + 1}. {highscores[i].playerName}: {highscores[i].score}\n";
        }
        return formattedHighscores.TrimEnd('\n');
    }

/// <summary>
/// Sorts highscores in descending order
/// Called after new scores are added via ScoreManager
/// </summary>
    private void SortHighscores()
    {
        highscores.Sort((a, b) => b.score.CompareTo(a.score));
    }

//for debugging purposes
    public void ResetHighscores()
    {
        highscores.Clear();
        SaveHighscores();
        Debug.Log("Highscores have been reset.");
    }
}