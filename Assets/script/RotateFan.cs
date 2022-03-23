using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFan : MonoBehaviour
{
    public int speedRotate;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speedRotate * Time.deltaTime);
        transform.Rotate(Vector3.up * speedRotate * Time.deltaTime);
    }
}
