using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public Car Player;
    public bool Active = false;
    public float destoryTime;

    private void Awake() {
        Player = FindObjectOfType<Car>();
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            Active = true;
            Destroy(this.gameObject, destoryTime);
        }
    }
}
