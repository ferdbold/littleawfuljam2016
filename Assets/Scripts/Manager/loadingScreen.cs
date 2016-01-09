using UnityEngine;
using System.Collections;

public class loadingScreen : MonoBehaviour {

    [SerializeField]
    private GameObject icon;

    void Awake() {
        StartCoroutine(LoadingIcon());
    }

    IEnumerator LoadingIcon() {
        float currentRotation = 0;
        while (true) {
            icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, currentRotation -= 300 * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
    }
}
