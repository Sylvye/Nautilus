using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class Vessel : MonoBehaviour
{
    public List<Thruster> thrusters;
    public List<Cannon> cannons;

    public void Move(Vector2 input)
    {
        for (int i = 0; i < thrusters.Count; i++)
        {
            // calculate how much thrust to give to each thruster based on how close the input vector is to it.
        }
    }
}
