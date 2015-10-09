using UnityEngine;
using System.Collections;

public class TemperatureControl : MonoBehaviour {
    void Start() {
        GetComponent<TextMesh>().text = "98.3 ºC";
    }
}
