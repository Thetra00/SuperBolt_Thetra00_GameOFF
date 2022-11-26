using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] Vector3 rotationDir;
    void Update()
    {
        transform.Rotate(rotationDir * Time.deltaTime);
    }
}
