using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canus : MonoBehaviour {

    public Vector3[] wayPoints;
    public float speed;
    int i = 0;

    void Start() {
        transform.LookAt(wayPoints[0]);
    }

    void FixedUpdate() {
        if (i < wayPoints.Length) {
            transform.position = Vector3.MoveTowards(transform.position, wayPoints[i], speed * Time.deltaTime);
            if (Vector3.Distance(wayPoints[i], transform.position) == 0f) {
                i++;
                if (i < wayPoints.Length) {
                    transform.LookAt(wayPoints[i]);
                }
            }
        }
        else {
            i = 0;
            transform.LookAt(wayPoints[i]);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameObject.FindWithTag("UI").transform.GetChild(0).GetComponent<Poo>().Active();
        }
    }

}
