using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public Transform target;
    Transform cameraTrack;
    public float trackSpeed;

    public Vector3 cameraPos;
    public Vector3 camerarotation;

    Car car;

    private void Awake() {
        cameraTrack = target.GetChild(2).transform;
    }
    void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, cameraTrack.position, trackSpeed);
        transform.position += cameraPos;
        transform.LookAt(target);
        transform.Rotate(camerarotation);
        if (car.gyroUse)
            transform.Rotate(new Vector3(0, 0, -car.handle));
    }
    private void Start() {
        car = target.GetComponent<Car>();
    }

    private void Update() {
        
    }
}
