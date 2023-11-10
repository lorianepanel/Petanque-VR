using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TerrainInfo : MonoBehaviour
{

    List<GameObject> items = new List<GameObject>();
    // Start is called before the first frame update
    
    void OnTriggerEnter(Collider collider){
        Debug.Log($"^{collider.name} is on the terrain");
        items.Add(collider.gameObject);
    }

    void OnTriggerExit(Collider collider){
        items.Remove(collider.gameObject);
    }
    

    public bool IsIn(GameObject go){
        return items.Contains(go);
        Debug.Log($"^{collider.name} is not on the terrain anymore");
    }
}
