using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

    public GameObject player;

	void Update () {
        Vector3 lookAtPos = player.transform.position - transform.position + new Vector3(0,0.2f,0);
        Quaternion newRot = Quaternion.LookRotation(lookAtPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot , 0.1f);
	}
}
