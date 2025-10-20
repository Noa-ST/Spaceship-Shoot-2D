using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    public int scoreValue;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = Vector2.down * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.DeadZone.ToString()))
        {
            // GỌI PHƯƠNG THỨC KẾT THÚC GAME TẬP TRUNG
            // Đảm bảo trạng thái GameState được cập nhật thành GameOver
            GameController.Ins.SetGameOver();

            // Việc hiển thị dialog và phát âm thanh Game Over sẽ được xử lý trong GameController.SetGameOver().
            // Chỉ cần hủy đối tượng enemy hiện tại (SetGameOver() cũng hủy tất cả enemy, nhưng làm ở đây để đảm bảo).
            Destroy(gameObject);
        }
    }
}