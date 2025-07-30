using UnityEngine;

public class Body : MonoBehaviour
{
    public float maxHP;
    public float hp;

    public void Damage(float amount)
    {
        hp -= amount;
        Debug.Log(gameObject.name + ": Just took " + amount + " damage!");
        if (hp < 0)
        {
            hp = 0;
            Destroy(gameObject); // TEMP, replace with a death animation of some kind
        }
    }
}
