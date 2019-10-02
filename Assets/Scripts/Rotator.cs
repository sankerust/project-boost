using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
  [SerializeField] float rotationFactor = 5f;

  // todo remove from inspector later
  float movementFactor; // 0 for not moved, 1 for fully moved
                        // Start is called before the first frame update
  Vector3 startingPos;
  Vector3 endingPos;
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    transform.Rotate(Vector3.forward * rotationFactor);
  }
}
