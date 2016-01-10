using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Converable : MonoBehaviour {

    [HideInInspector]
    public bool locked = false;

    void Awake() {
        StartCoroutine(Unlock());
    }

    IEnumerator Unlock() {
        while (true) {
            locked = false;
            yield return new WaitForEndOfFrame();
        }
    }
}
