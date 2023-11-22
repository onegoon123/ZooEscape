using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour
{
    bool stop;
    public GameObject stopMenu;

    public void Stop() {
        stop = !stop;
        Time.timeScale = stop ? 0 : 1;
        stopMenu.SetActive(stop);
    }
}
