using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poo : MonoBehaviour {

    Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public void Active() {
        StartCoroutine(pooAnim());
    }

    IEnumerator pooAnim() {
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime * 4;
            image.color = new Color(255, 255, 255, t);
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        t = 1;
        while (0 < t) {
            t -= Time.deltaTime * 0.75f;
            image.color = new Color(255, 255, 255, t);
            yield return null;
        }
        image.color = new Color(255, 255, 255, 0);
    }
}
