using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class TopCamera : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
        SnapShot();
    }


    public void SnapShot(){
        StartCoroutine(TakeSnapShot());
    }

    private IEnumerator TakeSnapShot(){
        cam.enabled = true;
        yield return null;
        cam.enabled = false;
    }
}
