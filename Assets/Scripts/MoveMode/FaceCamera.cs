using UnityEngine;

/// <summary>
/// Turns the object so that it always faces the main game camera
/// </summary>
public class FaceCamera : MonoBehaviour {

	private Transform _gameCamera;

	public void Awake() {
		FindElements();
	}

	/// <summary>
	/// Register all child elements
	/// </summary>
	private void FindElements() {
		_gameCamera = GameObject.FindWithTag("MainCamera").transform;
	}

	public void Update () {
		transform.LookAt(_gameCamera.position);
	}
}
