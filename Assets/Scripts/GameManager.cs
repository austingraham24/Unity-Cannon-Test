using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//the projectile game object
	[SerializeField] private GameObject cannonBallPrefab;
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
	//the spawn point to create cannonBalls at is a child so it follows the aiming of the cannon body
	private GameObject projectileSpawnPoint;
	//the active cannon ball
	private GameObject cannonBall;
    //number of frames fire button was held
    private int framesHeld = 0;
    //current power value
    private float power;
    //current power prercentage
    private int powerPercentage;
    //was missle fired?
    private bool hasFired = false;
    //max power amount
    private int maxFramePower = 350;
    // the initial & current rotation of the cannonBody
    private float currentRotation = 75.0f;
    //make sure you get these in the correct order! Otherwise you will have unexpected behavior (or none at all).
    //also make sure your current rotation is within this range so it does not snap to the min or max angle upon user input
    private float minAimAngle = 45; // ex: 45
    private float maxAimAngle = 90; //ex: 90
    //# of shots allowed
    private int numShots = 3;

	// Use this for initialization
	void Start () {
		cannonBody = cannon.transform.GetChild(0).gameObject;
		projectileSpawnPoint = cannonBody.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		handleUserInut();

    }

    private void handleUserInut(){
    	handleArrowInput();

    	if(numShots != 0){
    		if (Input.GetKey(KeyCode.Space)) {
    		handleSpaceInput();
	    	}else{ //meaning I have let go of spacebar
	    		if(framesHeld > 0 && !hasFired) {
	                //FIRE ZE MISSILES!
	                hasFired = true;
	                FireUpAndOver(framesHeld);
	            }
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
    		//using forces which moves the cannon and the cannon ball through Physics!
    		cannon.GetComponent<Rigidbody>().AddForce(cannon.transform.right * Input.GetAxis ("Horizontal") * 10);

    		//old method manually changing transform.position of both the cannon and the cannon ball since this ignores
    		//physics calculations
    		/*cannon.transform.position -= new Vector3(-Input.GetAxis ("Horizontal") * moveSpeed * Time.deltaTime,0.0f,0.0f);
    		if(!hasFired){
    			cannonBall.transform.position -= new Vector3(-Input.GetAxis ("Horizontal") * moveSpeed * Time.deltaTime,0.0f,0.0f);
    		}*/
    	}
    }

    private void handleSpaceInput(){
    	if (!hasFired) {
                framesHeld++;
                power = ((float)framesHeld/(float)maxFramePower);
                powerPercentage = (int)(power * 100f);
                powerBar.fillAmount = power;
                powerProgress.text=(powerPercentage.ToString()+"%");
                if(framesHeld>maxFramePower) {
                	framesHeld = maxFramePower;
                    hasFired = true;
                	FireUpAndOver(framesHeld);
                }
            }
    }

    private void reloadCannon(){
        //cannonBall.GetComponent<Projectile>().respawn();
        cannonBall = null;
        hasFired = false;
        framesHeld = 0;
    }

    private void FireUpAndOver(int power) {
    	cannonBall = Instantiate(cannonBallPrefab, projectileSpawnPoint.transform.position, Quaternion.identity);
        cannonBall.GetComponent<Rigidbody>().AddForce(cannonBody.transform.up * power * 5);
        cannonBall.GetComponent<Rigidbody>().useGravity = true;
        numShots -= 1;
        StartCoroutine(powerDown());
    }

    IEnumerator powerDown() {
    	float subtractPower = 1f;
    	while (power > 0){
    		power = Mathf.Lerp(0,power,subtractPower);
    		if(power > .01f){
    			subtractPower -= .5f * Time.deltaTime;
    		}else{
    			subtractPower -= 1;
    		}
    		powerPercentage = (int)(power * 100f);
	        powerBar.fillAmount = power;
	        powerProgress.text=(powerPercentage.ToString()+"%");
	        yield return new WaitForSeconds(.025f);	
    	}
    	reloadCannon();
    }

}
