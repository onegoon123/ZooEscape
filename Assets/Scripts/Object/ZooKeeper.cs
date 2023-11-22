using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZooKeeper : MonoBehaviour {

    Animator animator;
    NavMeshAgent nvAgent;
    public Transform targetTransform;

    void Awake() {
        animator = GetComponent<Animator>();
        nvAgent = GetComponent<NavMeshAgent>();
    }
    
    void Update() {
        nvAgent.destination = targetTransform.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(Attack());
            animator.SetTrigger("Attack");
        }
    }

    IEnumerator Attack() {
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime;
            transform.LookAt(targetTransform);
            yield return null;
        }
    }

}
