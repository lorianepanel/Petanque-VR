using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class TopCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    private Transform _goal;
    
    public Transform goal {
        get { 
            if (_goal == null) {
                _goal = GameObject.FindWithTag("Goal")?.transform;
                }
            return _goal;}
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        SnapShot();
    }


    public void SnapShot(){
        
        if(goal == null) return;

        // Obtenez la position actuelle de la caméra
        Vector3 cameraPosition = cam.transform.position;

        // Définissez la nouvelle position en X et Z
        cameraPosition.x = goal.position.x;
        cameraPosition.z = goal.position.z;

        // Appliquez la nouvelle position à la caméra sans changer la position en Y
        cam.transform.position = cameraPosition;
         
        StartCoroutine(TakeSnapShot());
    }

    private IEnumerator TakeSnapShot(){
        cam.enabled = true;
        yield return null;
        cam.enabled = false;
    }
}
