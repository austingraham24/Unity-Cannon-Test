﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//the projectile game object
	[SerializeField] private GameObject cannonBall;
	//the cannon game object NOTE: this is the entire canon object and not just the body
	[SerializeField] private GameObject cannon;
	//camera to be used to chase the projectile after launch
	[SerializeField] private GameObject chaseCam;
	//UI power element for visual progress
	[SerializeField] private Image powerBar;
	//UI text feedback on power progress
	[SerializeField] private Text powerProgress;
	//speed multiplier to move cannon side to side
	[SerializeField] private int moveSpeed;
	//speed multiplier to aim cannon
	[SerializeField] private int rotateSpeed;

	//the body of then cannon to be rotated and aimed; set on start
	private GameObject cannonBody;
    //number of frames fire button was held
    private int framesHeld = 0;
    //was missle fired?
    private bool hasFired = false;
    //max power amount
    private int maxFramePower = 350;
    // the initial & current rotation of the cannonBody
    private float currentRotation = 75.0f;

    public float maxAimAngle; //ex: 45
    public float minAimAngle; // ex: 90

	// Use this for initialization
	void Start () {
		cannonBody = cannon.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		handleUserInut();

        if (Input.GetKey(KeyCode.Space)) {
            //make sure missile has not been launched 
            if (!hasFired) {
                framesHeld++;
                float progressDecimal = ((float)framesHeld/(float)maxFramePower);
                int progressPercent = (int)(progressDecimal * 100f);
                powerBar.fillAmount = progressDecimal;
                powerProgress.text=(progressPercent.ToString()+"%");
                if(framesHeld>maxFramePower) {
                	framesHeld = maxFramePower;
                    hasFired = true;
                	FireUpAndOver(framesHeld);
                }
            }

        }
        else {
            if(framesHeld > 0 && !hasFired) {
                //FIRE ZE MISSILES!
                hasFired = true;
                FireUpAndOver(framesHeld);
            }
        }

    }

    private void handleUserInut(){
    	handleArrowInput();

    	if (Input.GetKey(KeyCode.Space)) {
    		handleSpaceInput();
    	}else{
    		if(framesHeld > 0 && !hasFired) {
                //FIRE ZE MISSILES!
                hasFired = true;
                FireUpAndOver(framesHeld);
            }
    	}
    }

    private void handleArrowInput(){
    	if(Input.GetAxis ("Vertical")!=0){
    		currentRotation += -Input.GetAxis ("Vertical") * 2;
			currentRotation = Mathf.Clamp(currentRotation, minAimAngle, maxAimAngle);
    		cannonBody.transform.eulerAngles = new Vector3(currentRotation,cannonBody.transform.eulerAngles.y, cannonBody.transform.eulerAngles.z);
    	}
    	if(Input.GetAxis ("Horizontal")!=0){
    		cannon.transform.position -= new Vector3(-Input.GetAxis ("Horizontal") * moveSpeed * Time.deltaTime,0.0f,0.0f);
    		if(!hasFired){
    			cannonBall.transform.position -= new Vector3(-Input.GetAxis ("Horizontal") * moveSpeed * Time.deltaTime,0.0f,0.0f);
    		}
    	}
    }

    private void handleSpaceInput(){
    	if (!hasFired) {
                framesHeld++;
                float progressDecimal = ((float)framesHeld/(float)maxFramePower);
                int progressPercent = (int)(progressDecimal * 100f);
                powerBar.fillAmount = progressDecimal;
                powerProgress.text=(progressPercent.ToString()+"%");
                if(framesHeld>maxFramePower) {
                	framesHeld = maxFramePower;
                    hasFired = true;
                	FireUpAndOver(framesHeld);
                }
            }
    }

    private void reloadCannon(){
    	powerBar.fillAmount = 0;
        powerProgress.text=("0%");
        cannon.transform.position = Vector3.MoveTowards(cannon.transform.position,new Vector3(0f,0f,0f), 2 * Time.deltaTime);
    }

    private void FireUpAndOver(int power) {
    	print(power);
        cannonBall.GetComponent<Rigidbody>().AddForce(cannonBody.transform.up * power * 5);
        reloadCannon();
    }
}