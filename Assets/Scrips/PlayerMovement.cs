using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    Rigidbody playerrigid;
    PlayerInput playerInput;
    Animator animator;
    Vector3 moveInput;

    public void Awake()
    {
        playerrigid = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        moveInput = new Vector3(playerInput.MoveX, 0, playerInput.MoveZ).normalized;
        animator.SetBool("isMoving", moveInput.magnitude > 0);
        LookAtMouse();
    }

    private void FixedUpdate()
    {
        playerrigid.MovePosition(playerrigid.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // 비활성화 시 즉시 정지
    private void OnDisable()
    {
        moveInput = Vector3.zero;
        animator.SetBool("isMoving", false);
    }

    private void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 lookPoint = ray.GetPoint(distance);
            lookPoint.y = transform.position.y;
            transform.LookAt(lookPoint);
        }
    }
}