using UnityEngine;
using System.Collections;

public class CreateSpidersOnKlick : MonoBehaviour {
    public Transform player;
	public GameObject ObjectToCreate;
	
    void Update() {
        if (Input.GetMouseButton(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
				Instantiate(ObjectToCreate, hit.point,Quaternion.identity);
            }
        }
    }
}