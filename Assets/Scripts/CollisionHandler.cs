using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("in seconds")] [SerializeField] float levelLoadDelay = 1f;
    [Tooltip("Fx prefab on player")] [SerializeField] GameObject deathFx;
    /*private void OnCollisionEnter(Collision collision)
    {
        print("Collision");
    }*/

    private void OnTriggerEnter(Collider other)
    {
        StartDeathSequence();
        deathFx.SetActive(true);
        Invoke("ReloadScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        SendMessage("OnPlayerDeath");

    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
