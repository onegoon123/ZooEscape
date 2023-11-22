using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    [System.Serializable]
    public struct Stat
    {
        public float acceleration;  // 가속도
        public float topSpeed;      // 최고속도
        public float handling;      // 핸들링
        public float brake;         // 브레이크

        public void StatAdd(Stat stat)
        {
            acceleration += stat.acceleration;
            topSpeed += stat.topSpeed;
            handling += stat.handling;
            brake += stat.brake;
        }
        public void StatAdd(params int[] stat)
        {
            acceleration += stat[0];
            topSpeed += stat[1];
            handling += stat[2];
            brake += stat[3];
        }
        public void Clear()
        {
            acceleration = 0;
            topSpeed = 0;
            handling = 0;
            brake = 0;
        }
    }
    public Stat myCar;
    public Stat bonus;
    public Stat[] animalStat;
    public Stat[] carStat;

    public GameObject[] Animal;
    public GameObject[] Car;
    Text money;
    Slider Accel;
    Slider TopSpeed;
    Slider Handling;
    Slider Brake;
    Slider bonusAccel;
    Slider bonusTopSpeed;
    Slider bonusHandling;
    Slider bonusBrake;
    public int[] animalPrice;
    public int[] carPrice;

    private void Awake()
    {
        for (int i = 0; i < Animal.Length; i++) {
            Animal[i] = transform.GetChild(0).GetChild(0).GetChild(i).gameObject;
            if (GameManager.instance.MyAnimal[i]) {
                Animal[i].transform.GetChild(0).GetComponent<Image>().color = Color.black;
                Destroy(Animal[i].transform.GetChild(0).GetChild(0).gameObject);
            }
            if (i == GameManager.instance.animalNum) Animal[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }
        for (int i = 0; i < Car.Length; i++)
        {
            Car[i] = transform.GetChild(1).GetChild(0).GetChild(i).gameObject;
            if (GameManager.instance.MyCar[i]) {
                Car[i].transform.GetChild(0).GetComponent<Image>().color = Color.black;
                Destroy(Car[i].transform.GetChild(0).GetChild(0).gameObject);
            }
            if (i == GameManager.instance.carNum) Car[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }
        Accel = transform.GetChild(2).GetChild(0).GetComponent<Slider>();
        TopSpeed = transform.GetChild(2).GetChild(1).GetComponent<Slider>();
        Handling = transform.GetChild(2).GetChild(2).GetComponent<Slider>();
        Brake = transform.GetChild(2).GetChild(3).GetComponent<Slider>();

        bonusAccel = Accel.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        bonusTopSpeed = TopSpeed.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        bonusHandling = Handling.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        bonusBrake = Brake.transform.GetChild(1).GetChild(0).GetComponent<Slider>();

        SetStat();
        money = transform.GetChild(4).GetComponent<Text>();
        money.text = GameManager.instance.Money.ToString();
    }

    public void AnimalButton(int i)
    {
        if (GameManager.instance.MyAnimal[i]) // 이미 있는경우
        {
            SetAnimal(i);
        }
        else    //없는경우
        {
            if (animalPrice[i] <= GameManager.instance.Money)
            {
                GameManager.instance.Money -= animalPrice[i];
                GameManager.instance.MyAnimal[i] = true;
                Destroy(Animal[i].transform.GetChild(0).GetChild(0).gameObject);
                SetAnimal(i);
            }
        }

        money.text = GameManager.instance.Money.ToString();
        GameManager.instance.Save();
    }

    void SetAnimal(int num)
    {
        GameManager.instance.animalNum = num;

        for (int i = 0; i < Animal.Length; i++)
        {
            if (GameManager.instance.MyAnimal[i]) Animal[i].transform.GetChild(0).GetComponent<Image>().color = Color.black;
            if (i == GameManager.instance.animalNum) Animal[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }

        FindObjectOfType<Preview>().Previewreset();
        SetStat();
    }

    public void CarButton(int i)
    {
        if (GameManager.instance.MyCar[i]) // 이미 있는경우
        {
            SetCar(i);
        }
        else    //없는경우
        {
            if (carPrice[i] <= GameManager.instance.Money)
            {
                GameManager.instance.Money -= carPrice[i];
                GameManager.instance.MyCar[i] = true;
                Destroy(Car[i].transform.GetChild(0).GetChild(0).gameObject);
                SetCar(i);
            }
        }
        money.text = GameManager.instance.Money.ToString();
        GameManager.instance.Save();
    }

    void SetCar(int num)
    {
        GameManager.instance.carNum = num;

        for (int i = 0; i < Car.Length; i++)
        {
            if (GameManager.instance.MyCar[i]) Car[i].transform.GetChild(0).GetComponent<Image>().color = Color.black;
            if (i == GameManager.instance.carNum) Car[i].transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }

        FindObjectOfType<Preview>().Previewreset();
        SetStat();
    }

    void SetStat()
    {
        myCar.Clear();
        bonus.Clear();
        myCar.StatAdd(animalStat[GameManager.instance.animalNum]);
        myCar.StatAdd(carStat[GameManager.instance.carNum]);
        if (GameManager.instance.animalNum == 0 && GameManager.instance.carNum == 0) bonus.StatAdd(1, 1, 2, 1); // 원숭이바나나
        if (GameManager.instance.animalNum == 1 && GameManager.instance.carNum == 1) bonus.StatAdd(1, 2, 1, 1); // 판다대나무
        if (GameManager.instance.animalNum == 2 && GameManager.instance.carNum == 2) bonus.StatAdd(1, 1, 2, 1); // 팽귄생선
        if (GameManager.instance.animalNum == 3 && GameManager.instance.carNum == 2) bonus.StatAdd(1, 1, 2, 1); // 북극곰생선
        if (GameManager.instance.animalNum == 3 && GameManager.instance.carNum == 3) bonus.StatAdd(2, 2, 2, -2);// 북극곰콜라
        Accel.value = myCar.acceleration;
        TopSpeed.value = myCar.topSpeed;
        Handling.value = myCar.handling;
        Brake.value = myCar.brake;

        bonusAccel.value = myCar.acceleration + bonus.acceleration;
        bonusTopSpeed.value = myCar.topSpeed + bonus.topSpeed;
        bonusHandling.value = myCar.handling + bonus.handling;
        bonusBrake.value = myCar.brake + bonus.brake;
    }
}
