using UnityEngine;
using System.Collections;

public class Camera_HideBlockingObjects : MonoBehaviour {

    /// <summary>
    /// Fade objects inbetween the camera and the player by attaching a Camera_MakeTransparent to them.
    /// </summary>
   
    public float DistanceToPlayer = 5f;
    public float DistanceBehindCamera = 125f;


    void Update() { 
        RaycastHit[] hits; // you can also use CapsuleCastAll() 

        //Debug.DrawRay(transform.position - (transform.forward * DistanceBehindCamera), transform.forward * (DistanceToPlayer + DistanceBehindCamera));
        hits = Physics.RaycastAll(transform.position - (transform.forward * DistanceBehindCamera), transform.forward, DistanceToPlayer + DistanceBehindCamera); 
        
        for(int i =0 ; i < hits.Length; i++) {
            MeshRenderer renderer = hits[i].collider.GetComponent<MeshRenderer>();
            if (renderer == null) continue; // If no renderer, skip this


            if (renderer.tag == "CanBeTransparent") { //Only fade objects of tag "obstacle"
                Camera_MakeTransparent transparencyScript = renderer.gameObject.GetComponent<Camera_MakeTransparent>();
                if(transparencyScript == null) {
                    transparencyScript = renderer.gameObject.AddComponent<Camera_MakeTransparent>();
                    transparencyScript.myRenderer = renderer;
                    transparencyScript.MakeTransparent();
                    transparencyScript.HidingScript = this;

                } else {
                    transparencyScript.MakeTransparent();
                }
            }
         }
     }

} 
