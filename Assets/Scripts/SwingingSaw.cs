using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is being used for swing behaviour
/// altough script is named as SwingingSaw, this script can be used for all swinging obstacles
/// </summary>
public class SwingingSaw : MonoBehaviour
{
    [SerializeField] private float swingSpeed;
    [SerializeField] private float maxSwingAngle;

    private float _initialRotation;

    void Start()
    {
        _initialRotation = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        float angle = maxSwingAngle * Mathf.Sin(Time.time * swingSpeed);

        transform.rotation = Quaternion.Euler(0, 0, _initialRotation + angle);
    }
}
