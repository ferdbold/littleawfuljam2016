using UnityEngine;

/**
 * Turns the object so that it always faces the main game camera
 */
public class FaceCamera : MonoBehaviour {

	private Transform _gameCamera;

	public void Awake() {
		FindElements();
	}

	/**
	 * Register all child elements
	 */
	private void FindElements() {
		_gameCamera = GameObject.FindWithTag("MainCamera").transform;
	}

	public void Update () {
		transform.LookAt(_gameCamera.position);
	}
}
