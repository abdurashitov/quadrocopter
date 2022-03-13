using UnityEngine;

public class Comand
{
    private Vector3 point;
    private comands comand;

    Comand(comands comand, Vector3 point)
    {
        this.comand = comand;
        this.point = point;
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

    public comands getComand()
    {
        return comand;
    }
}