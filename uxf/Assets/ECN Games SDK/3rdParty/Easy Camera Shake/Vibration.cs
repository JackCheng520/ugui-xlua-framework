﻿/// <summary>
/// Vibration
/// Author: MutantGopher
/// This script vibrates a GameObject in unity.  The intended use is for camera
/// shake effects, but you can use it to shake all kinds of GameObjects.  Simply
/// attach the script to the GameObject that you want to shake and adjust the
/// settings to fit your needs.  You can call the StartShaking(), StartShakingRandom(),
/// and StopShaking() functions from other scripts.  This is useful for explosions
/// or other vibrating effects.  
/// </summary>

using UnityEngine;
using System.Collections;

// The Preset class is used to hold preset values for the user
public class Preset
{
	public bool vibrateOnAwake = true;
	public Vector3 startingShakeDistance;
	public Quaternion startingRotationAmount;
	public float shakeSpeed;
	public float decreaseMultiplier;
	public int numberOfShakes;
	public bool shakeContinuous = false;
}

public class Vibration : MonoBehaviour
{
	public Preset[] presets;							// An array of presets
	public int presetToUse = 0;							// The selected preset
	
	public bool vibrateOnAwake = false;					// Should this GameObject vibrate on Awake()?
	public Vector3 startingShakeDistance = new Vector3(0,-0.25f,0.5f);				// The distance the GameObject will shake
	public Quaternion startingRotationAmount = Quaternion.identity;			// The amount the GameObject will rotate by
	public float shakeSpeed = 140.0f;					// How fast the GameObject will shake
	public float decreaseMultiplier = 0f;				// How fast the shake distance will diminish
	public int numberOfShakes = 3;						// The number of times this object will shake
	public bool shakeContinuous = false;				// Will this GameObject shake continously, instead of just once?

    //
	private Vector3 actualStartingShakeDistance;		// The shake distance actaully used.  This value may change
	private Quaternion actualStartingRotationAmount;	// The rotation amount actually used.  this value may change.
	private float actualShakeSpeed;						// The shake speed actually used. This value may change
	private float actualDecreaseMultiplier;				// The decrease multiplier actually used.  This value may change
	private int actualNumberOfShakes;					// The number of shakes actually used.  This value may change

    //记录震动前的初始位置
	private Vector3 originalPosition;					// Keep track of the position this GameObject was at before shaking (used for resetting when the vibration is over)
	private Quaternion originalRotation;				// Keep track of the rotation this GameObject was at before shaking (used for resetting when the vibration is over)

	void Awake()
	{
		// Initialize the original position to wherever the GameObject is at on Awake
        vibrateOnAwake = false;
		originalPosition = this.transform.localPosition;
		originalRotation = this.transform.localRotation;

		if (vibrateOnAwake)
		{
			StartShaking();
		}
	}

    /// <summary>
    /// 刷新位置
    /// </summary>
    void Update()
    {
        originalPosition = this.transform.localPosition;
        originalRotation = this.transform.localRotation;
    }



	// This function will cause the GameObject to start shaking with its own default values
	public void StartShaking()
	{
        originalPosition = this.transform.localPosition;
        originalRotation = this.transform.localRotation;

        //设置参数
		actualStartingShakeDistance = startingShakeDistance;
		actualStartingRotationAmount = startingRotationAmount;
		actualShakeSpeed = shakeSpeed;
		actualDecreaseMultiplier = decreaseMultiplier;
		actualNumberOfShakes = numberOfShakes;

		StopShaking();
		StartCoroutine("Shake");
	}

	// This function will cause the GameObject to start shaking with the values passed to it
	public void StartShaking(Vector3 shakeDistance, Quaternion rotationAmount, float speed, float diminish, int numOfShakes)
	{

        originalPosition = this.transform.localPosition;
        originalRotation = this.transform.localRotation;

		actualStartingShakeDistance = shakeDistance;
		actualStartingRotationAmount = rotationAmount;
		actualShakeSpeed = speed;
		actualDecreaseMultiplier = diminish;
		actualNumberOfShakes = numOfShakes;
		StopShaking();
		StartCoroutine("Shake");
	}

	// This function will cause the GameObject to start shaking with random values
	public void StartShakingRandom(float minDistance, float maxDistance, float minRotationAmount, float maxRotationAmount)
	{
		actualStartingShakeDistance = new Vector3(Random.Range(minDistance, maxDistance), Random.Range(minDistance, maxDistance), Random.Range(minDistance, maxDistance));
		actualStartingRotationAmount = new Quaternion(Random.Range(minRotationAmount, maxRotationAmount), Random.Range(minRotationAmount, maxRotationAmount), Random.Range(minRotationAmount, maxRotationAmount), 1);
		actualShakeSpeed = shakeSpeed * Random.Range(0.8f, 1.2f);
		actualDecreaseMultiplier = decreaseMultiplier * Random.Range(0.8f, 1.2f);
		actualNumberOfShakes = numberOfShakes + Random.Range(-2, 2);
		StopShaking();
		StartCoroutine("Shake");
	}

	public void StopShaking()
	{
		// Stop the shake coroutine if its running
		StopCoroutine("Shake");

		// Reset the position of the GameObject to its original position
		transform.localPosition = originalPosition;
		transform.localRotation = originalRotation;

        //
        

	}

	private IEnumerator Shake()
	{
		originalPosition = transform.localPosition;
		originalRotation = transform.localRotation;
        //originalPosition = transform.position;
        //originalRotation = transform.rotation;

		float hitTime = Time.time;
		float shake = actualNumberOfShakes;

		float shakeDistanceX = actualStartingShakeDistance.x;
		float shakeDistanceY = actualStartingShakeDistance.y;
		float shakeDistanceZ = actualStartingShakeDistance.z;

		float shakeRotationX = actualStartingRotationAmount.x;
		float shakeRotationY = actualStartingRotationAmount.y;
		float shakeRotationZ = actualStartingRotationAmount.z;

		// Shake the number of times specified in actualNumberOfShakes
		while (shake > 0 || shakeContinuous)
		{
			float timer = (Time.time - hitTime) * actualShakeSpeed;
			float x = originalPosition.x + Mathf.Sin(timer) * shakeDistanceX;
			float y = originalPosition.y + Mathf.Sin(timer) * shakeDistanceY;
			float z = originalPosition.z + Mathf.Sin(timer) * shakeDistanceZ;

			float xr = originalRotation.x + Mathf.Sin(timer) * shakeRotationX;
			float yr = originalRotation.y + Mathf.Sin(timer) * shakeRotationY;
			float zr = originalRotation.z + Mathf.Sin(timer) * shakeRotationZ;

			transform.localPosition = new Vector3(x, y, z);
			transform.localRotation = new Quaternion(xr, yr, zr, transform.localRotation.w);

			if (timer > Mathf.PI * 2)
			{
				hitTime = Time.time;
				shakeDistanceX *= actualDecreaseMultiplier;
				shakeDistanceY *= actualDecreaseMultiplier;
				shakeDistanceZ *= actualDecreaseMultiplier;

				shakeRotationX *= actualDecreaseMultiplier;
				shakeRotationY *= actualDecreaseMultiplier;
				shakeRotationZ *= actualDecreaseMultiplier;

				shake--;
			}
			yield return true;
		}
        
        
        // Reset the position of the GameObject to its original position
		transform.localPosition = originalPosition;
		transform.localRotation = originalRotation;
	}
}



