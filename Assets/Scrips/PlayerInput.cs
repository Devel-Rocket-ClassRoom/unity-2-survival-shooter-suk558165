using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MoveX { get; private set; }
    public float MoveZ { get; private set; }
    public bool Fire { get; private set; }

    private void Update()
    {
        MoveX = Input.GetAxisRaw("Horizontal");
        MoveZ = Input.GetAxisRaw("Vertical");
        Fire = Input.GetButton("Fire1");
    }
}