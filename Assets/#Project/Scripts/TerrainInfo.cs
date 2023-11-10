using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TerrainInfo : MonoBehaviour
{

    public List<GameObject> listOfProjectiles = new List<GameObject>();
    // Start is called before the first frame update
    
    void OnTriggerEnter(Collider collider){
        listOfProjectiles.Add(collider.gameObject);
        Debug.Log($"{collider.name} is on the terrain");
    }

    void OnTriggerExit(Collider collider){
        listOfProjectiles.Remove(collider.gameObject);
        Debug.Log($"{collider.name} is not on the terrain anymore");
    }
    

    public bool IsIn(GameObject projectiles){
        return listOfProjectiles.Contains(projectiles);
    }


    // TU EN ETAIS LA ! Faire un boolean pour savoir si les projectiles sont stabilisés ou pas. Va chercher les rb des projectiles lancés
    // public bool IsStable(GameObject projectiles){
    //     return projectiles.Rigidbody;
    // }
}
