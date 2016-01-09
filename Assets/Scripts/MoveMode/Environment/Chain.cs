using UnityEngine;

/// <summary>
/// Interactable chain the character can grip
/// </summary>
public class Chain : Interactable {

	public override void Activate() {
		Debug.Log("Gripped!");
	}
}
