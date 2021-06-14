using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnumAxis
{
    X,
    Y,
    Z
}

public class Rotate : MonoBehaviour
{
    float x = 0;
    public EnumAxis axis;
    public float speed;

    private void FixedUpdate()
    {
       switch (axis)
        {
            case EnumAxis.X:
                transform.Rotate(speed, 0, 0);
                break;
            case EnumAxis.Y:
                transform.Rotate(0, speed, 0);
                break;
            case EnumAxis.Z:
                transform.Rotate(0,0 , speed);
                break;
        }
    }

}
