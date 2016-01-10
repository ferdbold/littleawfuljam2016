using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Converable : MonoBehaviour {

    [HideInInspector]
    public bool locked = false;
}
