using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	//life span of this projectile in seconds
	private int maxLife = 10;

	// Use this for initialization
	void Start () {
		StartCoroutine("calculateDeath");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void destroySelf(){
		Destroy(gameObject);
	}

	IEnumerator calculateDeath() {
        yield return new WaitForSeconds(maxLife);
        destroySelf();
    }
}
