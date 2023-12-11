using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class TerrainInfo : MonoBehaviour
{

    public List<GameObject> listOfProjectiles = new List<GameObject>();

    private ScoreManager scoreManager;

    private Rigidbody rb;

    private XRGrabInteractable grab;
    

    private void Start(){
        scoreManager = FindObjectOfType<ScoreManager>();
        if(scoreManager == null){
            Debug.LogError("TerrainInfo needs a score manager");
        }
    }

    void OnTriggerEnter(Collider collider){

        listOfProjectiles.Add(collider.gameObject);
        // Debug.Log($"{collider.name} is on the terrain.");

        if(collider.CompareTag("Ball")){
            scoreManager.balls.Add(collider.gameObject);
            // Debug.Log($"{collider.name} is in {nameof(scoreManager.balls)}");
        }
        
        grab = collider.GetComponent<XRGrabInteractable>();
        grab.enabled = false; 
    }

    void OnTriggerExit(Collider collider){
        listOfProjectiles.Remove(collider.gameObject);
        // Debug.Log($"{collider.name} is not on the terrain anymore.");

        if(collider.CompareTag("Ball")){
            scoreManager.balls.Remove(collider.gameObject);
        }
        
        grab = collider.GetComponent<XRGrabInteractable>();
        grab.enabled = true; 
    }
    

    public bool IsIn(GameObject projectiles){
        return listOfProjectiles.Contains(projectiles);
    }


    public bool IsStable(GameObject projectile){
        rb = projectile.GetComponent<Rigidbody>();
        if (rb != null) {
            return rb.velocity.magnitude < 0.1f;     
        }
        return false;
    }

    
}
