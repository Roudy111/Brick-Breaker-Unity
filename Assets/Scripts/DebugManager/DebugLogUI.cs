using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages UI interactions for the Debug Log Manager
/// </summary>
public class DebugLogUI : MonoBehaviour
{
    [SerializeField]
    private Button saveLogsButton;

    void Start()
    {
        if (saveLogsButton != null)
        {
            saveLogsButton.onClick.AddListener(OnSaveLogsClicked);
        }
    }

    private void OnSaveLogsClicked()
    {
        DebugLogManager.Instance.SaveLogs();
    }

    void OnDestroy()
    {
        if (saveLogsButton != null)
        {
            saveLogsButton.onClick.RemoveListener(OnSaveLogsClicked);
        }
    }
}