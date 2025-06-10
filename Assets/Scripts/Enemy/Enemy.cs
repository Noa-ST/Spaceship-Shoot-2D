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
            UIManager.Ins.gameoverDialog.Show(true);
            AudioController.Ins.PlaySound(AudioController.Ins.gameover);
            Destroy(gameObject);
        }
    }
}
