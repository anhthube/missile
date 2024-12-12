using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform target;           // Player hoặc đối tượng cần theo dõi
    public RectTransform healthBar;   // Thanh máu trong Canvas
    public RectTransform canvasRect;  // RectTransform của Canvas
    public Vector3 offset = new Vector3(0, 2f, 0); // Vị trí offset trên nhân vật
    private Camera mainCamera;         // Camera chính

    void Start()
    {
        // Lấy Camera chính
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (target != null && mainCamera != null)
        {
            // 1. Lấy vị trí thế giới của Player với offset
            Vector3 worldPosition = target.position + offset;

            // 2. Chuyển từ World Space sang Screen Space
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

            // 3. Kiểm tra nếu Player đang trong tầm nhìn Camera
            if (screenPosition.z > 0) // z > 0 nghĩa là đối tượng ở trước Camera
            {
                // 4. Chuyển từ Screen Space sang Canvas Space
                Vector2 canvasPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPosition,
                    mainCamera,
                    out canvasPosition
                );

                // 5. Gán vị trí cho thanh máu
                healthBar.localPosition = canvasPosition;
            }
            else
            {
                // Nếu Player ra khỏi màn hình, ẩn thanh máu
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}
