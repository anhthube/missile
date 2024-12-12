using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 10f;
    public GameObject gameOverPanel;
    public Joystick joystick;
    private float moveHorirontal;
    private float moveVertical;
    private Vector2 movement;

    public GameObject enemyPrefab;            // Prefab của kẻ địch
    public Transform[] spawnPointsenemy;           // Các điểm spawn cho kẻ địch
    public int numberOfEnemiesToSpawn = 5;    // Số lượng kẻ địch muốn spawn mỗi lần
    public int coin = 0;
    public int healt = 3;
    public TextMeshProUGUI cointext;
    public TextMeshProUGUI traitimtext;

    // Coin và Spawn
    public GameObject coinPrefab;   // Tham chiếu đến Prefab Coin
    public Transform[] spawnPoints; // Các điểm spawn cố định
    public int numberOfCoinsToSpawn = 10;  // Số lượng coin muốn spawn trong mỗi lượt chơi
    public float rotationSpeed = 5f;
    public GameObject[] enemyPrefabs;

    public static PlayerController instance;
    // health bar
    [SerializeField] int maxHealth;
    int currenHealth;
    public HealthBar healthBar;
    // time bar
    private Timebar timeBar;
    // bullet attack 
    public GameObject bullet;
    public Transform fireFoint;
    public float bulletspeed;



    private bool canShoot = true;
    public float shootCooldown = 2f; // Thời gian chờ giữa các lần bắn
  
    private void Start()
    {
        GameObject timeObject = GameObject.FindWithTag("TimeBar");
        if (timeObject != null)
        {
            timeBar = timeObject.GetComponent<Timebar>();
        }
        else
        {
            Debug.LogError("Không tìm thấy TimeBar trong scene!");
        }

        currenHealth = maxHealth;

        healthBar.UpdateBar(currenHealth, maxHealth);
        rb = GetComponent<Rigidbody2D>();
        
        gameOverPanel.SetActive(false);
        UpdateHealthUI();
        // Spawn coin khi bắt đầu game
        SpawnEnemies();
        SpawnCoins();
      
    }
    private void Awake()
    {
        // Thiết lập singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        
      
        // Tính toán góc quay từ input người chơi (phím hoặc joystick)
        float moveHorirontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Lấy giá trị từ joystick
        moveHorirontal = joystick.Horizontal;
        moveVertical = joystick.Vertical;

        // tao vecter di chuyen 
        movement = new Vector2 (moveHorirontal, moveVertical);
        // dung velocity de di chuyen muot ma
        //cap nhat vi tri may bay 
        rb.velocity = movement * moveSpeed;

        // Tạo vector di chuyển từ input của người chơi (chỉ xoay, không thay đổi tốc độ)
        Vector2 inputDirection = new Vector2(moveHorirontal, moveVertical);

        if (inputDirection.sqrMagnitude > 0.01f)  // Nếu có input di chuyển
        {
            float targetAngle = CalculateFixedRotation(movement);
            rb.rotation = targetAngle;
        }
    

    }
    public void shootbullet()
    {
        if (canShoot)
        {
            canShoot = false; // Tạm thời không thể bắn
            GameObject projecttile = Instantiate(bullet, fireFoint.position, fireFoint.rotation); // Tạo viên đạn
            Rigidbody2D rb = projecttile.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của viên đạn
            Debug.Log("Đã bắn viên đạn!");
            Invoke(nameof(ResetShoot), shootCooldown); // Gọi ResetShoot sau thời gian chờ
        }
        else
        {
            Debug.Log("Chờ thêm để bắn tiếp!");
        }
    }
    private void ResetShoot()
    {
        canShoot = true; // Đặt lại trạng thái cho phép bắn
        Debug.Log("Sẵn sàng bắn tiếp!");
    }   
    private float CalculateFixedRotation(Vector2 direction)
    {
        // Xác định góc cố định dựa trên hướng di chuyển
        if (direction.x > 0.5f && Mathf.Abs(direction.y) < 0.5f) // Di chuyển phải
            return 270f;
        else if (direction.x < -0.5f && Mathf.Abs(direction.y) < 0.5f) // Di chuyển trái
            return 90f;
        else if (direction.y > 0.5f && Mathf.Abs(direction.x) < 0.5f) // Di chuyển lên
            return 0f;
        else if (direction.y < -0.5f && Mathf.Abs(direction.x) < 0.5f) // Di chuyển xuống
            return 180f;
        else if (direction.x > 0.5f && direction.y > 0.5f) // Phải - Lên
            return 315f;
        else if (direction.x < -0.5f && direction.y > 0.5f) // Trái - Lên
            return 45f;
        else if (direction.x > 0.5f && direction.y < -0.5f) // Phải - Xuống
            return 225f;
        else if (direction.x < -0.5f && direction.y < -0.5f) // Trái - Xuống
            return 135f;

        return rb.rotation; // Giữ nguyên nếu không di chuyển
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("coin"))
        {
            coin++;
            
            cointext.SetText(coin.ToString());
            Destroy(other.gameObject);

            if (timeBar != null)
            {
                timeBar.AddTime(2f); // Thêm 5 giây vào thanh thời gian khi ăn coin
            }
        }

        if (other.CompareTag("Enemy"))
        {
            healt--;
            traitimtext.SetText(healt.ToString());
        }

        
    }
    public void TakeDamage(int damage)
    {
        currenHealth -= damage;

        if (currenHealth <= 0)
        {
            currenHealth = 0;
            GameOver();
        }


        healthBar.UpdateBar(currenHealth, maxHealth);
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        cointext.SetText(coin.ToString());
        traitimtext.SetText(healt.ToString());
    }
    public void RestartGame()
    {
        // Reset giá trị của các biến
        currenHealth = maxHealth;
        healthBar.UpdateBar(currenHealth, maxHealth);
        healt = 3;
        coin = 0;

        timeBar.ResetTime();
        // Reset lại UI
        cointext.SetText(coin.ToString());
        traitimtext.SetText(healt.ToString());
       

        // Reset lại vị trí của player (nếu cần)
        transform.position = Vector3.zero;  // Di chuyển player về vị trí bắt đầu

        // Bắt đầu lại game: reset time scale
        Time.timeScale = 1;  // Khôi phục lại thời gian

        // Ẩn game over panel
        gameOverPanel.SetActive(false);

        // Spawn lại các coin
       ClearObjects();
        SpawnCoins();
        SpawnEnemies();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;  // Đặt lại Time.timeScale để tiếp tục game
        gameOverPanel.SetActive(false);  // Ẩn game over panel
    }

    public void GameOver()
    {
        Time.timeScale = 0; // Dừng trò chơi
        gameOverPanel.SetActive(true); // Hiển thị giao diện kết thúc
    }


            private void SpawnCoins()
            {
                if (spawnPoints.Length == 0)
                {
                    Debug.LogError("Không có điểm spawn nào được gán trong mảng spawnPoints.");
                    return;
                }

                // Lấy số lượng vị trí spawn tối đa
                int coinsToSpawn = Mathf.Min(numberOfCoinsToSpawn, spawnPoints.Length);

                // Tạo danh sách các vị trí spawn ngẫu nhiên
                List<Transform> shuffledPoints = spawnPoints.OrderBy(x => Random.value).ToList();

                for (int i = 0; i < coinsToSpawn; i++)
                {
                    Instantiate(coinPrefab, shuffledPoints[i].position, Quaternion.identity);
                }
            }
    public void SpawnEnemies()
    {
        // Đảm bảo biến prefab và spawnPoints hợp lệ
        if (enemyPrefabs == null || spawnPointsenemy == null || spawnPointsenemy.Length == 0)
        {
            Debug.LogError("Prefab hoặc Spawn Points không hợp lệ!");
            return;
        }

        int enemiesToSpawn = Mathf.Min(numberOfEnemiesToSpawn, spawnPointsenemy.Length);
        List<Transform> shuffledPoints = spawnPointsenemy.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Chọn ngẫu nhiên một loại kẻ địch từ mảng enemyPrefabs
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Spawn kẻ địch tại điểm spawn
            Instantiate(enemyPrefab, shuffledPoints[i].position, Quaternion.identity);
        }

        // Chức năng này có thể spawn nhiều loại kẻ địch ở các điểm spawn khác nhau
    }

    private void ClearObjects()
    {
        // Xóa tất cả các coin
        GameObject[] existingCoins = GameObject.FindGameObjectsWithTag("coin");
        foreach (GameObject coin in existingCoins)
        {
            Destroy(coin);
        }

        // Xóa tất cả các enemy
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            Destroy(enemy);
        }
    }


}
