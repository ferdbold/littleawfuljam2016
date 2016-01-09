using UnityEngine;

/// <summary>
/// Collider that can be used to activate interactable objects
/// </summary>
public class Interactor : MonoBehaviour {

	public void OnTriggerEnter(Collider other) {
		other.gameObject.SendMessage("Focus", SendMessageOptions.DontRequireReceiver);
	}

	public void OnTriggerExit(Collider other) {
		Debug.Log("trigger exit");
		other.gameObject.SendMessage("Blur", SendMessageOptions.DontRequireReceiver);
	}
}
