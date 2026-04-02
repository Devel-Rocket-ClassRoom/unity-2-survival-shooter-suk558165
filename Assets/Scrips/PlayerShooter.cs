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
        // 나중에 총 붙이면 주석 해제
        // if (playerInput.Fire) gun.Fire();
    }
}