using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ocillator : MonoBehaviour {

    [SerializeField] private Vector3 movementVector = new Vector3(15f, 0f, 0f);
    [SerializeField] private float period = 2.0f;

    private Vector3 startPos;
    private float movementFactor;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2.0f;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2.0f + 0.5f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startPos + offset;
    }
}
