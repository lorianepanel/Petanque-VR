using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class TerrainInfo : MonoBehaviour
{

    public List<GameObject> listOfProjectiles = new List<GameObject>();

    private Rigidbody rb;

    private XRGrabInteractable grab;
    
    void OnTriggerEnter(Collider collider){
        listOfProjectiles.Add(collider.gameObject);
        Debug.Log($"{collider.name} on the playable surface.");
        grab = collider.GetComponent<XRGrabInteractable>();
        grab.enabled = false; 
    }

    void OnTriggerExit(Collider collider){
        listOfProjectiles.Remove(collider.gameObject);
        Debug.Log($"{collider.name} not on the playable surface anymore");
        grab = collider.GetComponent<XRGrabInteractable>();
        grab.enabled = true; 
    }
    

    public bool IsIn(GameObject projectiles){
        return listOfProjectiles.Contains(projectiles);
    }



    // Faire un autre boolean pour savoir si les projectiles sont stabilisés ou pas. Va chercher les rb des projectiles lancés
    public bool IsStable(GameObject projectile){
        rb = projectile.GetComponent<Rigidbody>();
        if (rb != null) {
            Debug.Log($"{projectile.name} is moving.");
            return rb.velocity.magnitude < 0.001f;     
        }
        return false;
    }
    
}
