using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speedbullet = 10f;  // Tốc độ di chuyển của viên đạn
    public int damage = 1;          // Sát thương của viên đạn
    private Transform target;       // Mục tiêu của viên đạn
    private Vector3 direction;      // Hướng di chuyển của viên đạn

    void Start()
    {
        // Tìm quái vật gần nhất
        target = FindNearestEnemy();

        if (target != null)
        {
            // Tính toán hướng di chuyển
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            // Nếu không tìm thấy mục tiêu, bắn thẳng lên
            direction = Vector3.up;
        }

        // Tự hủy viên đạn sau 5 giây nếu không trúng mục tiêu
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Di chuyển viên đạn theo hướng đã tính toán
        transform.position += direction * speedbullet * Time.deltaTime;

        // Xoay viên đạn theo hướng di chuyển
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // Nếu va chạm với quái vật
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.takeDamage(damage); // Gây sát thương cho quái vật
            }
            Destroy(gameObject); // Xóa viên đạn
        }
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance) // Chỉ tìm trong phạm vi 10 đơn vị
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }

}
