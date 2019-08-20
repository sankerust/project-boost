using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
  [SerializeField] float rcsThrust = 120f;
  [SerializeField] float mainThrust = 20f;
  Rigidbody rigidBody;
  AudioSource thrustSound;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        thrustSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      Thrust();
      Rotate();
    }

  void OnCollisionEnter(Collision collision) 
  {
    switch (collision.gameObject.tag)
    {
      case "Friendly" :
        print("hit friendly");
          break;
      case "Fuel" :
        print("refueling station");
          break;
      default:
        print("dead");
          break;
    }
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
