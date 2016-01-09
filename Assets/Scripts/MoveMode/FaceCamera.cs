using UnityEngine;

/**
 * Turns the object so that it always faces the main game camera
 */
public class FaceCamera : MonoBehaviour {

	private Transform gameCamera;

	public void Awake() {
		this.FindElements();
	}

	/**
	 * Register all child elements
	 */
	private void FindElements() {
		this.gameCamera = GameObject.FindWithTag("MainCamera").transform;
	}

	public void Update () {
		transform.LookAt(this.gameCamera.position);
	}
}
