using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Manages debug logs by capturing, storing, and saving them to a file
/// Implements the Singleton pattern to ensure only one instance exists
/// </summary>
public class DebugLogManager : singleton<DebugLogManager>
{
    private StringBuilder logBuilder;
    private List<LogEntry> logEntries;
    private string logFilePath;
    private const int MAX_LOGS = 1000; // Prevent memory issues with too many logs

    [Serializable]
    private struct LogEntry
    {
        public string timestamp;
        public string type;
        public string message;
        public string stackTrace;
    }

    public override void Awake()
    {
        base.Awake();
        InitializeLogger();
    }

    private void InitializeLogger()
    {
        logBuilder = new StringBuilder();
        logEntries = new List<LogEntry>();
        
        // Get desktop path and create a timestamped filename
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        logFilePath = Path.Combine(desktopPath, $"unity_debug_logs_{timestamp}.txt");
        
        // Subscribe to log messages
        Application.logMessageReceived += HandleLog;
        
        Debug.Log($"DebugLogManager initialized. Logs will be saved to: {logFilePath}");
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Create new log entry
        LogEntry entry = new LogEntry
        {
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            type = type.ToString(),
            message = logString,
            stackTrace = stackTrace
        };

        // Add to our collection
        logEntries.Add(entry);

        // Maintain max log count
        if (logEntries.Count > MAX_LOGS)
        {
            logEntries.RemoveAt(0);
        }
    }

    /// <summary>
    /// Saves all collected logs to a file
    /// </summary>
    public void SaveLogs()
    {
        try
        {
            logBuilder.Clear();
            
            // Add session info at the top of the file
            logBuilder.AppendLine($"Unity Debug Logs - Session: {DateTime.Now}");
            logBuilder.AppendLine($"Project: {Application.productName}");
            logBuilder.AppendLine($"Version: {Application.version}");
            logBuilder.AppendLine("----------------------------------------");
            logBuilder.AppendLine();

            // Build the log content
            foreach (var entry in logEntries)
            {
                logBuilder.AppendLine($"[{entry.timestamp}] [{entry.type}] {entry.message}");
                if (!string.IsNullOrEmpty(entry.stackTrace))
                {
                    logBuilder.AppendLine("Stack Trace:");
                    logBuilder.AppendLine(entry.stackTrace);
                }
                logBuilder.AppendLine("----------------------------------------");
            }

            // Write to file
            File.WriteAllText(logFilePath, logBuilder.ToString());
            Debug.Log($"Logs saved successfully to: {logFilePath}");

            // Open the containing folder in explorer (Windows) or finder (macOS)
            #if UNITY_EDITOR_WIN
                System.Diagnostics.Process.Start("explorer.exe", "/select," + logFilePath);
            #elif UNITY_EDITOR_OSX
                System.Diagnostics.Process.Start("open", "-R " + logFilePath);
            #endif
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save logs: {e.Message}");
        }
    }

    /// <summary>
    /// Clears all stored logs
    /// </summary>
    public void ClearLogs()
    {
        logEntries.Clear();
        logBuilder.Clear();
        Debug.Log("Logs cleared successfully.");
    }

    private void OnDestroy()
    {
        // Unsubscribe from log messages
        Application.logMessageReceived -= HandleLog;
    }
}