using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    void Awake() {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
        Load();
    }

    public int animalNum;

    public int carNum;

    public bool[] MyAnimal;
    public bool[] MyCar;

    public bool gyroUse = false;
    public int quality;
    public float volume = 0;
    public AudioMixer audioMixer;

    public int Money;

    public float[] bestTime = { 5999.99f, 5999.99f };

    public void Save() {
        PlayerPrefs.SetInt("animalNum", animalNum);
        PlayerPrefs.SetInt("carNum", carNum);
        for (int i = 0; i < MyAnimal.Length; i++) {
            PlayerPrefs.SetString("MyAnimal" + i, MyAnimal[i].ToString());
        }
        for (int i = 0; i < MyCar.Length; i++) {
            PlayerPrefs.SetString("MyCar" + i, MyCar[i].ToString());
        }
        PlayerPrefs.SetString("gyroUse", gyroUse.ToString());
        PlayerPrefs.SetInt("quality", quality);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetFloat("bestTime0", bestTime[0]);
        PlayerPrefs.SetFloat("bestTime1", bestTime[1]);
        
    }

    public void Load() {
        animalNum = PlayerPrefs.GetInt("animalNum", 0);
        carNum = PlayerPrefs.GetInt("carNum", 0);
        for (int i = 0; i < MyAnimal.Length; i++) {
            MyAnimal[i] = System.Convert.ToBoolean(PlayerPrefs.GetString("MyAnimal" + i, "false"));
        }
        MyAnimal[0] = true;
        for (int i = 0; i < MyCar.Length; i++) {
            MyCar[i] = System.Convert.ToBoolean(PlayerPrefs.GetString("MyCar" + i, "false"));
        }
        MyCar[0] = true;
        gyroUse = System.Convert.ToBoolean(PlayerPrefs.GetString("gyroUse", "false"));
        quality = PlayerPrefs.GetInt("quality", 0);
        volume = PlayerPrefs.GetFloat("volume", 0);
        QualitySettings.SetQualityLevel(quality);
        audioMixer.SetFloat("volume", volume);

        Money = PlayerPrefs.GetInt("Money", 0);
        bestTime[0] = PlayerPrefs.GetFloat("bestTime0", 5999.99f);
        bestTime[1] = PlayerPrefs.GetFloat("bestTime1", 5999.99f);
    }
}
