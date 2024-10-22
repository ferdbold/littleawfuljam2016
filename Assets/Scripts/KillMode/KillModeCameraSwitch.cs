﻿using UnityEngine;
using System.Collections;
using DG.Tweening;

public class KillModeCameraSwitch : MonoBehaviour {
	public delegate void KillModeCallback();

	[SerializeField] private float distanceFromTarget = 2f;

	// The target is the one that is being killed
	public void StartCameraAnim(KillModeCallback callback, Vector3 targetPosition, Vector3 frontVector) {
		StartCoroutine(CameraAnim(callback, targetPosition, targetPosition + frontVector * distanceFromTarget, 5f));
	}

	private IEnumerator CameraAnim(KillModeCallback callback, Vector3 targetPos, Vector3 position, float time) {
		transform.DOLookAt(targetPos, 1.5f);
		yield return new WaitForSeconds(1.5f);
		transform.DOMove(position, time);

		for (float i=0; i<5f; i+= Time.deltaTime) {
			transform.DOLookAt(targetPos, Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}

		callback();

        while (true) {
            transform.DOLookAt(targetPos, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
	}
}
