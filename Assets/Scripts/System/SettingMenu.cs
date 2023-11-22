using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    private void Awake() {
        transform.GetChild(0).GetChild(1).GetComponent<Slider>().value = GameManager.instance.volume;
        transform.GetChild(0).GetChild(0).GetComponent<Dropdown>().value = GameManager.instance.quality;
        SetGyroPlay(GameManager.instance.gyroUse);
    }

    public void SetVolume(float volume) {
        audioMixer.SetFloat("volume", volume);
        GameManager.instance.volume = volume;
        GameManager.instance.Save();
    }

    public void SetQuality (int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
        GameManager.instance.quality = qualityIndex;
        GameManager.instance.Save();
    }

    public void SetGyroPlay(bool gyro) {
        GameManager.instance.gyroUse = gyro;
        transform.GetChild(0).GetChild(2).GetComponent<Image>().color = gyro ? Color.white : Color.gray;
        transform.GetChild(0).GetChild(3).GetComponent<Image>().color = gyro ? Color.gray : Color.white;

        GameManager.instance.Save();
    }
}
