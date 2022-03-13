using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSModul : MonoBehaviour
{
    public GameObject Quadrocopter;
    public Rigidbody rb;
    public Vector3 GPS;
    public Vector3 vel;
    public Text Coordinate;
    public Text Velocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = Quadrocopter.GetComponent<Rigidbody>();
    }

    public double getYVelocity()
    {
        return vel.y;
    }
    public double getHeight()
    {
        return GPS.y;
    }

    public Vector3 getGps()
    {
        return GPS;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        vel = rb.velocity;
        GPS = Quadrocopter.transform.position;
        Coordinate.text = "Coordinate x=" + form(GPS.x) + " y=" + form(GPS.y) + " z="+ form(GPS.z);
        Velocity.text = "Velociti x=" + form(vel.x) + " y=" + form(vel.y) + " z=" + form(vel.z);
    }
    
    private string form(double number)
    {
        return string.Format("{0:0.0000}", number);
    }
    

}
