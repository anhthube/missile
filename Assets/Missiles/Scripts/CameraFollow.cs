using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Player hoặc đối tượng bạn muốn camera theo dõi
    public Vector3 offset;    // Khoảng cách giữa camera và đối tượng
    public float smoothSpeed = 0.125f; // Tốc độ di chuyển của camera

    void Start()
    {

        
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, -2f);
        }
    }

    void LateUpdate()
    {
        
        if (target != null)
        {
            // Tính toán vị trí mới của camera
            Vector3 desiredPosition = target.position + offset;
            // Làm mượt chuyển động camera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            
            // Cập nhật vị trí camera
            transform.position = smoothedPosition;
        }
    }
}
