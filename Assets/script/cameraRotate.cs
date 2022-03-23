using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotate : MonoBehaviour
{
    // Start is called before the first frame update

    Transform parrentTransform;
    void Start()
    {
        parrentTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = new Quaternion( 0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }
}
