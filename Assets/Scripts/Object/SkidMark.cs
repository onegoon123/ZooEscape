using UnityEngine;

public class SkidMark : MonoBehaviour {

    public bool StopSkid;
    public float currentFrictionValue;
    public TrailRenderer trail;
    public GameObject SkidSound;
    private float waitTime = 0.1f;
    private WheelCollider wheel;

    private void Awake() {
        wheel = GetComponent<WheelCollider>();
    }

    void Update() {
        if (StopSkid) return;
        WheelHit hit;
        wheel.GetGroundHit(out hit);
        currentFrictionValue = hit.sidewaysSlip;
        currentFrictionValue = Mathf.Abs(currentFrictionValue);
        if (currentFrictionValue >= 0.3f && waitTime >= 0.7f) {
            Destroy((GameObject)Instantiate(SkidSound, hit.point, Quaternion.identity), 1);
            waitTime = 0f;
        }
        else {
            waitTime += Time.deltaTime;
        }
        if (currentFrictionValue >= 0.3f) {
            trail.emitting = true;
        }
        else {
            trail.emitting = false;
        }
    }
}
