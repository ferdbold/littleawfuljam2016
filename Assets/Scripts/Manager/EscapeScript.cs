using UnityEngine;
using System.Collections;

public class EscapeScript : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
