using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.CompilerServices;
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 0f; // Tốc độ di chuyển của kẻ địch
    public int damage = 1;       // Số lượng damage khi va chạm
    private Transform player;    // Tham chiếu đến người chơi
    private Rigidbody2D rb;      // Rigidbody2D của kẻ địch để di chuyển
    private AIPath aiPath;

    private float currentHealth;
    private float maxHealth = 3f;
    void Start()
    {
        // khoi tao thanh mau 
        currentHealth = maxHealth;


        aiPath = GetComponent<AIPath>();
        // Gán đối tượng người chơi từ tag "Player"
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform; // Gán người chơi làm mục tiêu
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng với tag 'Player'!");
        }
        // Lấy Rigidbody2D để di chuyển
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D not found!"); // Kiểm tra xem có Rigidbody2D không
    }

    void Update()
    {
        if (player != null)
        {
            // Tự động gán vị trí của người chơi vào thuộc tính "destination"
            aiPath.destination = player.position;
        }
        else
        {
            // Nếu không có người chơi, AI sẽ không di chuyển
            aiPath.destination = transform.position;
        }
    }
    public void takeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu va chạm với kẻ địch khác, đẩy chúng ra
        if (other.CompareTag("Enemy"))
        {
            Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
            if (otherRb != null)
            {
                // Tính toán hướng đẩy và áp dụng lực đẩy
                Vector2 direction = (other.transform.position - transform.position).normalized;
                float pushForce = 5f; // Cường độ lực đẩy (có thể điều chỉnh)
                otherRb.AddForce(direction * pushForce, ForceMode2D.Impulse); // Đẩy kẻ địch ra
            }
        }

        // Nếu va chạm với người chơi, gây sát thương cho người chơi và xóa kẻ địch
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage); // Gây sát thương cho người chơi
            }

            // Spawn lại kẻ địch trước khi hủy
            RespawnEnemy();
            Destroy(gameObject); // Hủy kẻ địch sau khi va chạm với người chơi
        }
    }
        // Hàm spawn lại enemy tại vị trí spawn ngẫu nhiên
        private void RespawnEnemy()
        {
            // Lấy danh sách các điểm spawn từ PlayerController
            Transform[] spawnPoints = PlayerController.instance.spawnPointsenemy;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                // Chọn một điểm spawn ngẫu nhiên
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Tạo kẻ địch mới tại vị trí spawn ngẫu nhiên
                Instantiate(PlayerController.instance.enemyPrefab, randomSpawnPoint.position, Quaternion.identity);
            }

        }
    }

