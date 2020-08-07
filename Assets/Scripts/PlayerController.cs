using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    [Header("General")]
    [Tooltip("in m.s^-1")][SerializeField] float xSpeed = 20f;
    [Tooltip("in m.s^-1")] [SerializeField] float ySpeed = 20f;
    [SerializeField] GameObject[] guns;

    [Header("Position")]
    [SerializeField] float positionPitchFactor = -2.5f;
    [SerializeField] float positionYawFactor = 2.5f;


    [Header("Control Throw")]
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = -20f;

    [SerializeField] float clampX = 12f;
    [SerializeField] float clampUpY = 7.5f;
    [SerializeField] float clampDownY = -5f;

    float xThrow, yThrow;

    bool isControlEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnPlayerDeath()
    {
        isControlEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
        }
        
    }

    private void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            SetGunsActive(true);
            
        }
        else
        {
            SetGunsActive(false);
        }
    }

    private void SetGunsActive(bool isActive)
    {
        foreach (GameObject gun in guns)
        {
            var particleEmission = gun.GetComponent<ParticleSystem>().emission;
            particleEmission.enabled = isActive;
        }
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * xSpeed * Time.deltaTime;
        float yOffset = yThrow * ySpeed * Time.deltaTime;

        float rawNewXPos = transform.localPosition.x + xOffset;
        float rawNewYPos = transform.localPosition.y + yOffset;

        float clampedXPos = Mathf.Clamp(rawNewXPos, -clampX, clampX);
        float clampedYPos = Mathf.Clamp(rawNewYPos, clampDownY, clampUpY);

        transform.localPosition = new Vector3(clampedXPos, /*transform.localPosition.y*/ clampedYPos, transform.localPosition.z);
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow ;

        float yaw = transform.localPosition.x * positionYawFactor;
        float roll =xThrow * controlRollFactor;
        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
}
