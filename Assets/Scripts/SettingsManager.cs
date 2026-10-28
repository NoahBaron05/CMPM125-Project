using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public float Brightness { get; private set; } = 1f;

    Image brightnessOverlay;

    public bool ShowAimLine { get; private set; } = false;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Brightness = PlayerPrefs.GetFloat("Brightness", 1f);
        ShowAimLine = PlayerPrefs.GetInt("ShowAimLine", 1) == 1;
    }

    public void SetAimLine(bool value)
    {
        ShowAimLine = value;
        PlayerPrefs.SetInt("ShowAimLine", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetBrightnessOverlay(Image overlay)
    {
        brightnessOverlay = overlay;
        ApplyBrightness();
    }

    public void SetBrightness(float value)
    {
        Brightness = value;
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();

        ApplyBrightness();
    }

    void ApplyBrightness()
    {
        if (brightnessOverlay == null) return;

        Color color = brightnessOverlay.color;
        color.a = 1f - Brightness;
        brightnessOverlay.color = color;
    }
}
