using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public List<Collider> Colliders = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (!Colliders.Contains(other))
            Colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Colliders.Remove(other);
    }
}