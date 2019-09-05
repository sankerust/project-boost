using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
  [SerializeField] float rcsThrust = 120f;
  [SerializeField] float mainThrust = 20f;
  Rigidbody rigidBody;
  AudioSource thrustSound;
  AudioSource crashSound;
  AudioSource winSound;
  enum State { Alive, Dying, Transcending };
  State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        var rocketSounds = GetComponents<AudioSource>();
        thrustSound = rocketSounds[0];
        crashSound = rocketSounds[1];
        winSound = rocketSounds[2];
    }

    // Update is called once per frame
    void Update()
    {
      if (state == State.Alive)
      {
        Thrust();
        Rotate();
      }

    }

  void OnCollisionEnter(Collision collision) 
  {
    if (state != State.Alive) { return; } // ignore collisions if dead
    switch (collision.gameObject.tag)
    {
      case "Friendly" :
          break;
      case "Finish":
        state = State.Transcending;
        thrustSound.Stop();
        // reload active sc
        winSound.Play();
        // load next scene
        Invoke("LoadNextScene", 2.5f); //parametesie time
        break;
      default:
        state = State.Dying;
        crashSound.Play();
        thrustSound.Stop();
        // reload active scene
        Invoke("ReloadScene", 1f);
        break;
    }
  }

  private void ReloadScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  private void LoadNextScene()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  private void Thrust()
  {

    if (Input.GetKey(KeyCode.Space))
    {
      rigidBody.AddRelativeForce(Vector3.up * mainThrust);
      if (!thrustSound.isPlaying)
      {
        thrustSound.Play();
      }
    }
    else
    {
      thrustSound.Stop();
    }
  }

  private void Rotate()
  {
    
    float rotationThisFrame = rcsThrust * Time.deltaTime;

    rigidBody.freezeRotation = true; // take manual control of rotation

    if (Input.GetKey(KeyCode.A))
    {
      transform.Rotate(Vector3.forward * rotationThisFrame);
    }
    else if (Input.GetKey(KeyCode.D))
    {
      transform.Rotate(-Vector3.forward * rotationThisFrame);
    }

    rigidBody.freezeRotation = false; //resume physics control of rotation
  }


}
