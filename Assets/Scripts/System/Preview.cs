using UnityEngine;

public class Preview : MonoBehaviour {

    bool left;
    bool right;


    private void Awake()
    {
        Previewreset();
    }

    public void Previewreset ()
    {
        for (int i = 0; i < 8; i++)
        {
            transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).GetChild(GameManager.instance.animalNum).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(GameManager.instance.carNum + GameManager.instance.MyAnimal.Length).gameObject.SetActive(true);
    }
    public void leftTurn() {
        left = true;
        right = false;
    }

    public void leftEnd() {
        left = false;
    }

    public void rightTurn() {
        left = false;
        right = true;
    }
    public void rightEnd() {
        right = false;
    }

    private void Update() {
        if (left)
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        if (right)
            transform.Rotate(Vector3.up * -100 * Time.deltaTime);
    }
}
