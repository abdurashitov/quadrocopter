using UnityEngine;

public class Comand
{
    private Vector3 point;
    private float speed;
    private comands comand;

    public Comand(comands comand, Vector3 point, float speed)
    {
        this.comand = comand;
        this.point = point;
        this.speed = speed;
    }

    public enum comands
    {
        up,
        down,
        hover,
        goTo,
    }
    public Vector3 getpoint()
    {
        return point;
    }
    // "goTo;40,23;8"
    public comands getComand()
    {
        return comand;
    }

    public float getSpeed()
    {
        return speed;
    }
}