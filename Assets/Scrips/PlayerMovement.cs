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
        // 스크립트가 비활성화(Pause 상황 등)되어 있으면 회전하지 않음
        if (!this.enabled) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 lookPoint = ray.GetPoint(distance);

            // 캐릭터의 높이와 맞추어 바닥만 바라보게 함 (기울어짐 방지)
            Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);

            // 거리가 너무 가까우면 회전이 튀므로 최소 거리 체크
            if (Vector3.Distance(transform.position, heightCorrectedPoint) > 0.1f)
            {
                transform.LookAt(heightCorrectedPoint);
            }
        }
    }
}