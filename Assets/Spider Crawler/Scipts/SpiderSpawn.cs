using UnityEngine;
using System.Collections;

public class SpiderSpawn : MonoBehaviour {
	private int SpiderAmount = 0;
	public GameObject Spider;
	public GameObject SpawnLocation;
	public int MaxSpiderAmount = 200;
	public int SpawnSpeed = 20;
    void Update() {
	int SpawnTime =	Random.Range(1,SpawnSpeed);
		if(SpawnTime == 1){
			if(SpiderAmount < MaxSpiderAmount){
				Instantiate(Spider, SpawnLocation.transform.position,Quaternion.identity);
				SpiderAmount += 1;
			}
		}
    }
}