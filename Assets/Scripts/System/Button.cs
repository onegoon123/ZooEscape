using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{

    public void LoadScene(int scene) {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }

    public void ToggleObject(GameObject gameObject) {
        gameObject.SetActive(!gameObject.active);
    }

    public void OffObejcet(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
