using UnityEngine;
using System.Collections;

public class Camera_MakeTransparent : MonoBehaviour {

    /// <summary>
    /// Make the object's mesh renderer fade to target Transparency
    /// </summary>

    private Material oldMaterial;
    public Material newMaterial;

    private float timeToBecomeVisible = 1f;
    private float curTransparentTime = 0;
    private float startAlpha = 0.2f;

    private bool isTransparent = false;
    private bool isFadingIn = false;
    [HideInInspector] public Camera_HideBlockingObjects HidingScript;
    public MeshRenderer myRenderer;
  

    public void Awake() {
        newMaterial = (Material) Resources.Load("Material/TransparentMaterial");
    }

    public void MakeTransparent() {

        curTransparentTime = timeToBecomeVisible;

        if (!isTransparent) {
            // reset the transparency;
            oldMaterial = myRenderer.material;
            myRenderer.material = newMaterial;
            isTransparent = true;
            StartCoroutine(LerpAlphaIn(1));
        } else if (!isFadingIn) {
            StopCoroutine("LerpAlphaOut");
            myRenderer.material.color = new Color(myRenderer.material.color.r,
                                                       myRenderer.material.color.g,
                                                       myRenderer.material.color.b,
                                                       startAlpha);
        }
    }

    public void MakeNotTransparent() {
        if (isTransparent) {
            // reset the transparency;
            myRenderer.material = oldMaterial;
            isTransparent = false;
            oldMaterial = null;
            HidingScript = null;
        }

    }

    void Update() {

        if (curTransparentTime > 0f) {
            if(!isFadingIn) curTransparentTime -= Time.deltaTime;
        } else if (isTransparent) StartCoroutine("LerpAlphaOut");                                                                                                                                                                                                                                                                                                  
    }

    IEnumerator LerpAlphaOut() {
        Debug.Log("Start Out alpha  " + isTransparent);
        for (float i = startAlpha; i < 1; i += Time.deltaTime / 0.8f) {
            myRenderer.material.color = new Color(  myRenderer.material.color.r,
                                                    myRenderer.material.color.g,
                                                    myRenderer.material.color.b,
                                                    i);
            yield return null;
        }
        MakeNotTransparent();
    }

    IEnumerator LerpAlphaIn(float startAlpha) {
        Debug.Log("Start In alpha  " + isTransparent);
        isFadingIn = true;
        for (float i = startAlpha; i >= startAlpha; i -= Time.deltaTime / 0.8f) {
            myRenderer.material.color = new Color(myRenderer.material.color.r,
                                                    myRenderer.material.color.g,
                                                    myRenderer.material.color.b,
                                                    i);
            yield return null;
        }
        isFadingIn = false;
    }

}
