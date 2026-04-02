using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private Animator animator;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        
    }
}