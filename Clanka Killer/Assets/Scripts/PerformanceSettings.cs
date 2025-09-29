using UnityEngine;

public class PerformanceSettings : MonoBehaviour
{
    void Start()
    {
        // Disable V-Sync
        QualitySettings.vSyncCount = 0;

        // Optional: Set a target framerate (e.g., 144 FPS)
        //Application.targetFrameRate = 144;
    }
}
