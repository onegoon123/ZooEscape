using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class Car : MonoBehaviour {

    // 스텟은 1 ~ 10 수치 까지 계획
    [System.Serializable]
    public struct Stat {
        public float acceleration;  // 가속도
        public float topSpeed;      // 최고속도
        public float handling;      // 핸들링
        public float brake;         // 브레이크

        public void StatAdd(Stat stat) {
            acceleration += stat.acceleration;
            topSpeed += stat.topSpeed;
            handling += stat.handling;
            brake += stat.brake;
        }
        public void StatAdd(params int[] stat) {
            acceleration += stat[0];
            topSpeed += stat[1];
            handling += stat[2];
            brake += stat[3];
        }
    }
    public Stat myCar;
    public Stat[] animalStat;
    public Stat[] carStat;

    public WheelCollider[] wheels; //휠 콜라이더를 받아온다.
    public Transform[] tires; //바퀴가 돌아가는 걸 표현하기위한 메쉬를 받아온다.

    public float acceleration;  // 가속도
    private float RevAccel;      // 후진 가속도
    private float brakePower;
    public float maxSpeed;      // 전진 최고속도
    private float maxRevSpeed;   // 후진 최고속도
    private float rot;           // 회전 각도
    private float handling;      // 핸들링
    public float currentSpeed;  // 현재 속도 

    public GameObject touchUI;  // 터치 플레이 UI
    public GameObject gyroUI;

    public float handle = 0;            // 핸들
    public bool gyroUse;                // 자이로조작
    private bool breaking = false;      // 브레이크
    private bool drift = false;         // 드리프트
    private bool Ldrift = false;
    private bool Rdrift = false;
    public float friction;              // 마찰력
    private float brakeTime;
    private bool gameOver;
    Rigidbody rb;
    public SkidMark[] skids;


    void Start() {
        gyroUse = GameManager.instance.gyroUse;
        Input.gyro.enabled = gyroUse;  // 자이로 온
        touchUI.SetActive(!gyroUse);     // 터치 ui
        gyroUI.SetActive(gyroUse);

        rb = GetComponent<Rigidbody>(); //리지드바디를 받아온다.

        for (int i = 0; i < 4; i++) {
            wheels[i].steerAngle = 0; //만약 바퀴와 휠콜라이더의 방향이 교차한다면 90으로 설정해주자.
            wheels[i].ConfigureVehicleSubsteps(5, 12, 13);
        }
        rb.centerOfMass = new Vector3(0, 0, 0); //무게중심을 가운데로 맞춰서 안정적으로 주행하도록 한다.

        transform.GetChild(0).GetChild(GameManager.instance.animalNum).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(GameManager.instance.carNum + GameManager.instance.MyAnimal.Length).gameObject.SetActive(true);
        // ----- 스텟 조정 -----
        myCar.StatAdd(animalStat[GameManager.instance.animalNum]);
        myCar.StatAdd(carStat[GameManager.instance.carNum]);
        if (GameManager.instance.animalNum == 0 && GameManager.instance.carNum == 0) myCar.StatAdd(1, 1, 2, 1); // 원숭이바나나
        if (GameManager.instance.animalNum == 1 && GameManager.instance.carNum == 1) myCar.StatAdd(1, 2, 1, 1); // 판다대나무
        if (GameManager.instance.animalNum == 2 && GameManager.instance.carNum == 2) myCar.StatAdd(1, 1, 2, 1); // 팽귄생선
        if (GameManager.instance.animalNum == 3 && GameManager.instance.carNum == 2) myCar.StatAdd(1, 1, 2, 1); // 북극곰생선
        if (GameManager.instance.animalNum == 3 && GameManager.instance.carNum == 3) myCar.StatAdd(2, 2, 2, -2);// 북극곰콜라

        acceleration = 500 + myCar.acceleration * 100;
        maxSpeed = 80 + myCar.topSpeed * 5;
        handling = 0.04f + myCar.handling * 0.01f;
        rot = 38 + myCar.handling * 1.5f;
        friction = 0.3f - myCar.handling * 0.02f;
        RevAccel = -(1500 + myCar.brake * 200);
        maxRevSpeed = -(5 + myCar.brake * 8);
        brakePower = 1000 + 800 * myCar.brake;

        this.enabled = false;
    }

    private void Update() {
        if (gameOver) return;
        Operation();    // 주행 조작
        UpdateMeshesPostion(); //바퀴가 돌아가는게 보이도록 함
    }

    void FixedUpdate() {
        if (gameOver) return;
        WheelControl();
        Drift();
    }

    void Operation() {
        bool leftTouch = false;
        bool rightTouch = false;

        if (Input.touchCount > 0) { // 터치가 있다면
            for (int i = 0; i < Input.touchCount; i++) {    // 터치 처음부터 끝까지
                Touch touch = Input.GetTouch(i);
                float touchPos = Camera.main.ScreenToViewportPoint(touch.position).x;
                if (touchPos < 0.5) {
                    leftTouch = true;
                }
                else if (0.5 < touchPos) {
                    rightTouch = true;
                }
            }
        }

        
        leftTouch = Input.GetKey(KeyCode.LeftArrow);
        rightTouch = Input.GetKey(KeyCode.RightArrow);
        drift = Input.GetKey(KeyCode.Space);
        

        if (gyroUse) GyroControl(leftTouch, rightTouch);    // 자이로 사용
        else TouchControl(leftTouch, rightTouch);           // 터치 사용
    }

    public void LeftDriftSet(bool drift) {
        Ldrift = drift;
        this.drift = Rdrift || Ldrift;
    }

    public void RightDriftSet(bool drift) {
        Rdrift = drift;
        this.drift = Rdrift || Ldrift;
    }

    public void GyroReset() {
        handle = 0;
    }

    void GyroControl(bool leftTouch, bool rightTouch) {    // 자이로
        handle -= Input.gyro.rotationRate.z;    // 자이로 핸들링

        if (leftTouch) breaking = true; // 왼쪽터치 (브레이크)
        else breaking = false;

        if (rightTouch && !leftTouch) drift = true;
        else drift = false;
    }

    void TouchControl(bool leftTouch, bool rightTouch) {   // 터치

        if (!leftTouch && !rightTouch) { // 터치 없음
            handle = Mathf.Lerp(handle, 0, handling);
            breaking = false;
            return;
        }
        else if (leftTouch && rightTouch) {    // 양쪽터치 브레이크
            handle = Mathf.Lerp(handle, 0, handling);
            if (!drift)
                breaking = true;
            return;
        }
        else {
            breaking = false;
        }

        if (leftTouch)
            handle = Mathf.Lerp(handle, -rot, handling);
        else if (rightTouch)
            handle = Mathf.Lerp(handle, rot, handling);


    }

    void WheelControl() {
        // 가속 or 감속
        if (breaking) { // 브레이크
            foreach (WheelCollider wheel in wheels) {
                wheel.motorTorque = RevAccel;

                if (brakeTime > 0.8) {
                    wheel.brakeTorque = 0;
                }
                else if (brakeTime > 0) {
                    wheel.brakeTorque = brakePower;
                }
                brakeTime += Time.deltaTime;
            }
        }
        else {  // 가속
            brakeTime = 0;

            foreach (WheelCollider wheel in wheels) {
                wheel.motorTorque = acceleration;
                wheel.brakeTorque = 0;

                if (currentSpeed < 0) {
                    wheel.motorTorque += 10000;
                }
            }
        }
        // 속도제한
        currentSpeed = 2 * 3.14f * wheels[3].radius * wheels[3].rpm * 60 / 1000;
        currentSpeed = Mathf.Round(currentSpeed);

        if (currentSpeed > 5 && currentSpeed > maxSpeed) {//전진속도 제한
            foreach (WheelCollider wheel in wheels) {
                wheel.motorTorque = -1000;
            }
        }
        if (currentSpeed < -5 && currentSpeed < maxRevSpeed) {//후진속도 제한
            foreach (WheelCollider wheel in wheels) {
                wheel.motorTorque = 100;
            }
        }

        float wheelDir = Mathf.Clamp(handle, -rot, rot);
        for (int i = 0; i < 2; i++) {
            wheels[i].steerAngle = wheelDir;
        }
    }

    void Drift() {
        if (drift) {
            WheelFrictionCurve wfc = new WheelFrictionCurve();

            wfc.asymptoteSlip = wheels[2].sidewaysFriction.asymptoteSlip;
            wfc.asymptoteValue = wheels[2].sidewaysFriction.asymptoteValue;
            wfc.extremumSlip = wheels[2].sidewaysFriction.extremumSlip;
            wfc.extremumValue = wheels[2].sidewaysFriction.extremumValue;
            wfc.stiffness = friction;

            wheels[2].sidewaysFriction = wfc;
            wheels[3].sidewaysFriction = wfc;
        }
        else {
            WheelFrictionCurve wfc = new WheelFrictionCurve();
            wfc.asymptoteSlip = wheels[2].sidewaysFriction.asymptoteSlip;
            wfc.asymptoteValue = wheels[2].sidewaysFriction.asymptoteValue;
            wfc.extremumSlip = wheels[2].sidewaysFriction.extremumSlip;
            wfc.extremumValue = wheels[2].sidewaysFriction.extremumValue;
            wfc.stiffness = 1f;

            wheels[2].sidewaysFriction = wfc;
            wheels[3].sidewaysFriction = wfc;
        }
    }

    void UpdateMeshesPostion() {
        for (int i = 0; i < 4; i++) {

            Quaternion quat;
            Vector3 pos;
            wheels[i].GetWorldPose(out pos, out quat);
            tires[i].position = pos;
            tires[i].rotation = quat;

        }
        if (drift) {
            tires[2].localRotation = Quaternion.identity;
            tires[3].localRotation = Quaternion.identity;
        }
    }

    public void speedUp() {
        StartCoroutine(_SpeedUp());
    }
    public void Brake(float brake, float time) {
        StartCoroutine(_SpeedDown(brake, time));
    }

    private IEnumerator _SpeedUp() {
        acceleration += 9999;
        maxSpeed += 100;
        yield return new WaitForSeconds(1f);
        acceleration -= 9999;
        maxSpeed -= 100;
    }
    private IEnumerator _SpeedDown(float brake, float time) {
        foreach (WheelCollider wheel in wheels) {
            maxSpeed -= brake;
        }
        yield return new WaitForSeconds(time);
        foreach (WheelCollider wheel in wheels) {
            maxSpeed += brake;
        }
        yield return null;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.CompareTag("Wall")) {
            foreach (SkidMark skid in skids) {
                skid.StopSkid = true;
            }
            StartCoroutine(WaitSkid());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EnemyAttack")) {
            GameManager.instance.Money += 15;
            GameManager.instance.Save();
            gameOver = true;
            foreach (WheelCollider wheel in wheels) {
                wheel.brakeTorque = 10000;
            }
            GameObject.FindWithTag("Enemy").GetComponent<Animator>().SetTrigger("End");
            StartCoroutine(EndGame());
        }
    }

    IEnumerator WaitSkid() {
        yield return new WaitForSeconds(1);
        foreach (SkidMark skid in skids) {
            skid.StopSkid = false;
        }
    }

    IEnumerator EndGame() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}