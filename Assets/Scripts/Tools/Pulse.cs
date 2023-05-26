using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    [SerializeField]
    private float   pulseSpeed = 1,
                    min = 0, max = 1;

    [SerializeField]
    private AnimationCurve pulse;

    private float t, range;

    private void Awake()
    {
        range = max - min;
    }
    private void Update()
    {
        t += Time.deltaTime * pulseSpeed;
        transform.localScale = Vector3.one * (min + range * pulse.Evaluate(t));
    }
    private void OnValidate()
    {
        if (pulseSpeed == 0)
            pulseSpeed = 0.001f;

        if (min > max)
            min = max;

        if (max < min)
            max = min;
    }
}
