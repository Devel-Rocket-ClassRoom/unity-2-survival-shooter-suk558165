using UnityEngine;

[CreateAssetMenu(fileName = "AnemyData", menuName = "Data/AnemyData")]
public class AmenyData : ScriptableObject
{
    public float maxHP = 100f;
    public float damage = 10f;
    public float speed = 3f;
}
