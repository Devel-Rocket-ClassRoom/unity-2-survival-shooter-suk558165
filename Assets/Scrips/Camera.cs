using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // 플레이어
    public Vector3 offset = new Vector3(-5, 18, -13);          // 카메라 오프셋
    public float smoothSpeed = 5f;  // 부드러움 정도

    private void Start()
    {
        // 현재 카메라와 플레이어 간의 거리를 자동으로 offset으로 설정
        if (target != null)
            offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
