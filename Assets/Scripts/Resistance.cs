using System;

[System.Serializable]
public class Resistance
{
    public Damage.Type type;
    public float mult;

    public Resistance(Damage.Type typ)
    {
        type = typ;
        mult = 1;
    }

    public Resistance(Damage.Type typ, float mlt)
    {
        type = typ;
        mult = mlt;
    }

    // resistances are "equal" if they have the same type
    public override bool Equals(Object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Resistance other = (Resistance)obj;

        return type == other.type;
    }

    public override int GetHashCode()
    {
        return type.GetHashCode();
    }
}
