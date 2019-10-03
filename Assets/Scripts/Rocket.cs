using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
  [SerializeField] float rcsThrust = 120f;
  [SerializeField] float mainThrust = 20f;
  [SerializeField] float levelLoadDelay = 2f;
  [SerializeField] AudioClip mainEngine;
  [SerializeField] AudioClip death;
  [SerializeField] AudioClip success;

  [SerializeField] ParticleSystem mainEngineParticles;
  [SerializeField] ParticleSystem successParticles;
  [SerializeField] ParticleSystem deathParticles;
  

  Rigidbody rigidBody;
  AudioSource audioSource;
  enum State { Alive, Dying, Transcending };
  State state = State.Alive;
  Boolean collisionsEnabled = true;
  int currentSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
      if (state == State.Alive)
      {
        RespondToThrustInput();
        RespondToRotateInput();
        // todo only if debug mode is on
        if (Debug.isDebugBuild)
        {
          MonitorDebugKeys();
        }
      }

    }

  private void MonitorDebugKeys()
  {
    if (Input.GetKeyDown(KeyCode.L))
    {
      LoadNextScene();
    }
    else if (Input.GetKeyDown(KeyCode.K))
    {
      SceneManager.LoadScene(currentSceneIndex - 1);
    }
    else if (Input.GetKeyDown(KeyCode.C))
    {
      collisionsEnabled = !collisionsEnabled;
    }
  }

  void OnCollisionEnter(Collision collision) 
  {
    if (state != State.Alive || !collisionsEnabled) { return; } // ignore collisions if dead
    switch (collision.gameObject.tag)
    {
      case "Friendly" :
          break;
      case "Finish":
        StartSuccessSequence();
        break;
      default:
        StartDeathSequence();
        break;
    }
  }
  private void StartSuccessSequence()
  {
    state = State.Transcending;
    audioSource.Stop();
    audioSource.PlayOneShot(success);
    successParticles.Play();
    Invoke("LoadNextScene", levelLoadDelay); //parametesie time
  }
  private void StartDeathSequence()
  {
    state = State.Dying;
    audioSource.Stop();
    audioSource.PlayOneShot(death);
    mainEngineParticles.Stop();
    deathParticles.Play();
    Invoke("ReloadScene", levelLoadDelay);
  }

  private void ReloadScene()
  {
    SceneManager.LoadScene(currentSceneIndex);
  }

  private void LoadNextScene()
  {
    SceneManager.LoadScene(currentSceneIndex + 1);
  }

  private void RespondToThrustInput()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      ApplyThrust();
    }
    else
    {
      audioSource.Stop();
      mainEngineParticles.Stop();
    }
  }

  private void ApplyThrust()
  {
    rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
    if (!audioSource.isPlaying)
    {
      audioSource.PlayOneShot(mainEngine);
    }
    mainEngineParticles.Play();
  }

  private void RespondToRotateInput()
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
