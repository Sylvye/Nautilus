using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "ExplosionData")]
public class ExplosionData : ScriptableObject
{
    public float damage;
    public Vector2 speedRange;
    public int particles;
}
