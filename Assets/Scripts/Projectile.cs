using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	//where does the object spanw and respawn at
	private Vector3 spawnPoint;

	// Use this for initialization
	void Start () {
		spawnPoint = transform.position;
		print(spawnPoint);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void respawn(){
		transform.position = spawnPoint;
	}
}
