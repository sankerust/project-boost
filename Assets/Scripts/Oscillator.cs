using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
  [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
  [SerializeField] float period = 5f;
  
  // todo remove from inspector later
  float movementFactor; // 0 for not moved, 1 for fully moved
    // Start is called before the first frame update
    Vector3 startingPos;
    Vector3 endingPos;
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      //set movement factor automaticaly
      if (period <= Mathf.Epsilon) { return; }
      float cycles = Time.time / period; // grows continualy from zero

      const float tau = Mathf.PI * 2f;
      float rawSinWave = Mathf.Sin(cycles * tau); //goes from -1 to +1

      movementFactor = rawSinWave / 2f + 0.5f;
      endingPos = startingPos + (movementVector * movementFactor);
      transform.position = endingPos;
    }
}
