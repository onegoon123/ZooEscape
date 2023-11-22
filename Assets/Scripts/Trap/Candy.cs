using UnityEngine;

public class Candy : Trap {


    // Update is called once per frame
    void Update() {
        if (Active) {
            Player.speedUp();
            Destroy(this.gameObject);
        }
    }

}
