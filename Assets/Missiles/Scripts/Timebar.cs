using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timebar : MonoBehaviour
{
    public Image timeBar;       // Image đại diện cho thanh thời gian
    public float maxTime = 10f; // Thời gian tối đa (giây)

    private float currentTime;  // Thời gian hiện tại
    private PlayerController playerController; // Tham chiếu đến PlayerController

    void Start()
    {
        // Đặt thời gian hiện tại bằng maxTime
        currentTime = maxTime;

        // Tìm đối tượng Player và lấy PlayerController
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Không tìm thấy Player trong scene!");
        }

        // Cập nhật thanh thời gian ban đầu
        UpdateTimeBar();
    }

    void Update()
    {
        // Nếu còn thời gian, giảm dần theo thời gian thực
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Giảm theo thời gian thực
            UpdateTimeBar();              // Cập nhật giao diện
        }
        else
        {
            currentTime = 0; // Đảm bảo không giảm dưới 0
            GameOver();
        }
    }

    void UpdateTimeBar()
    {
        // Nếu đã gán Image, cập nhật fillAmount
        if (timeBar != null)
        {
            timeBar.fillAmount = currentTime / maxTime; // Tỉ lệ giữa thời gian còn lại và thời gian tối đa
        }
    }
    void GameOver()
    {
        // Gọi hàm GameOver của PlayerController nếu có
        if (playerController != null)
        {
            playerController.GameOver();
        }
        else
        {
            Debug.LogWarning("PlayerController chưa được tham chiếu!");
        }
    }
    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
        if (currentTime > maxTime)
        {
            currentTime = maxTime; // Giới hạn thời gian tối đa
        }
        UpdateTimeBar();
    }
    public void ResetTime()
    {
        currentTime = maxTime; // Đặt lại thời gian về tối đa
        UpdateTimeBar(); // Cập nhật giao diện
    }

}
