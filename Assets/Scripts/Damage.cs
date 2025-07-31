using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public enum Type
    {
        Generic,    // Unblockable damage
        Kinetic,    // Any physical damage (collisions)
        Magic,      // Magic sources
        Incendiary, // Fire (ex. thruster fire, explosions, sparks)
    }

    public float amount;
    public Type type;
    public Body source;

    public Damage(float amt)
    {
        amount = amt;
        type = Type.Generic;
    }

    public Damage(float amt, Type typ)
    {
        amount = amt;
        type = typ;
    }

    public Damage(float amt, Type typ, Body src)
    {
        amount = amt;
        type = typ;
        source = src;
    }

    public float Evaluate(List<Resistance> resists)
    {
        float amt = amount;
        if (resists != null)
        {
            int index = resists.IndexOf(new Resistance(type));
            if (index != -1)
            {
                amt *= resists[index].mult;
            }
        }
        return amt;
    }
}
