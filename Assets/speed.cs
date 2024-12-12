using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speed : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;

    void Start()
    {
        // Lấy reference đến Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Kiểm tra nếu không có Rigidbody2D
       
    }

    void FixedUpdate()
    {
        
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed;
        float vertical = Input.GetAxis("Vertical") * moveSpeed;

        Debug.Log($"Horizontal: {horizontal}, Vertical: {vertical}");
        // Tạo vector di chuyển trong không gian 2D
        Vector2 movement = new Vector2(horizontal, vertical);

        // Di chuyển đối tượng bằng Rigidbody2D
        rb.MovePosition(rb.position + movement * Time.deltaTime);
    }
}
