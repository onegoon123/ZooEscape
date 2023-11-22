using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public Text text;
    public bool run = false;
    public float time = 0;
    public GameObject[] otherUI;
    public GameObject endMenu;
    public GameObject newBest;
    public Text clearTime;
    public Text bestTime;
    public int mapNum;

    IEnumerator Start() {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<Car>().enabled = true;
        FindObjectOfType<Car>().transform.GetChild(3).gameObject.SetActive(true);
        run = true;
        yield return new WaitForSeconds(5f);
        FindObjectOfType<ZooKeeper>().enabled = true;
    }

    void Update() {
        if (run) time += Time.deltaTime;
        int m = (int)(time / 60);
        int s = (int)(time - m*60);
        int ms = (int)((time - m * 60 - s) * 100);
        text.text = m.ToString("00") + ":" + s.ToString("00") + ":" + ms.ToString("00");
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // 클리어 메뉴 표시, 마무리
            GameManager.instance.Money += 100;
            run = false;
            FindObjectOfType<ZooKeeper>().enabled = false;
            foreach (GameObject ui in otherUI) {
                ui.SetActive(false);
            }
            endMenu.SetActive(true);

            // 클리어타임 표시
            int m = (int)(time / 60);
            int s = (int)(time - m * 60);
            int ms = (int)((time - m * 60 - s) * 100);
            clearTime.text = m.ToString("00") + ":" + s.ToString("00") + ":" + ms.ToString("00");

            // 최고기록 갱신
            if (time < GameManager.instance.bestTime[mapNum]) { // 신기록
                GameManager.instance.bestTime[mapNum] = time;
                bestTime.text = m.ToString("00") + ":" + s.ToString("00") + ":" + ms.ToString("00");
                newBest.SetActive(true);

                if (mapNum == 1) {
                    if (!GameManager.instance.MyAnimal[2]) {
                        // 팽귄, 생선
                        GameManager.instance.MyAnimal[2] = true;
                        GameManager.instance.MyCar[2] = true;
                    }
                    if (!GameManager.instance.MyAnimal[3]) {
                        if (time < 60) {
                            GameManager.instance.MyAnimal[3] = true;
                            GameManager.instance.MyCar[3] = true;
                        }
                    }
                }
            }
            else {  // 신기록X
                m = (int)(GameManager.instance.bestTime[mapNum] / 60);
                s = (int)(GameManager.instance.bestTime[mapNum] - m * 60);
                ms = (int)((GameManager.instance.bestTime[mapNum] - m * 60 - s) * 100);
                bestTime.text = m.ToString("00") + ":" + s.ToString("00") + ":" + ms.ToString("00");
            }

            GameManager.instance.Save();
        }
    }
}
