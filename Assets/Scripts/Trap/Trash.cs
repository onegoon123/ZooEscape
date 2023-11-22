using UnityEngine;

public class Trash : MonoBehaviour{

    public Car Player;
    bool Active = false;
    public float DownSpeed;
    public float DownTime;
    private void Awake() {
        Player = FindObjectOfType<Car>();
    }
    void Update() {
        if (Active) {
            Player.Brake(DownSpeed, DownTime);
            Destroy(this.gameObject, 5);
            Destroy(this);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.transform.CompareTag("Player")) {
            Active = true;
        }
    }
}
