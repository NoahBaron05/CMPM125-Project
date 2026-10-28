using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("Brightness")]
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Image brightnessOverlay;

    [Header("Aim Line")]
    [SerializeField] Toggle aimLineToggle;

    void Start()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogError("SettingsManager was not found!");
            return;
        }

        // Set up brightness slider
        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = 0f;
            brightnessSlider.maxValue = 1f;
            brightnessSlider.value =
                SettingsManager.Instance.Brightness;

            brightnessSlider.onValueChanged.AddListener(
                SettingsManager.Instance.SetBrightness);
        }

        // Set up aim line toggle
        if (aimLineToggle != null)
        {
            aimLineToggle.isOn =
                SettingsManager.Instance.ShowAimLine;

            aimLineToggle.onValueChanged.AddListener(
                SettingsManager.Instance.SetAimLine);
        }

        // Apply brightness to the menu overlay
        if (brightnessSlider != null)
        {
            brightnessSlider.minValue = 0f;
            brightnessSlider.maxValue = 1f;
            brightnessSlider.value = SettingsManager.Instance.Brightness;
            brightnessSlider.onValueChanged.AddListener(SettingsManager.Instance.SetBrightness);
        }

        if (brightnessOverlay != null)
        {
            SettingsManager.Instance.SetBrightnessOverlay(brightnessOverlay);
        }
    }

    void OnDestroy()
    {
        if (SettingsManager.Instance == null)
            return;

        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.RemoveListener(SettingsManager.Instance.SetBrightness);
        }

        if (aimLineToggle != null)
        {
            aimLineToggle.onValueChanged.RemoveListener(SettingsManager.Instance.SetAimLine);
        }
    }
}
