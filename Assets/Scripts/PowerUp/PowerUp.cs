using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float duration = 5f; // Thời gian hiệu lực

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Spaceship gắn tag "Player"
        {
            SpaceshipMovemen player = collision.GetComponent<SpaceshipMovemen>();
            if (player != null)
            {
                player.ActivatePowerUp(type, duration);
            }

            Destroy(gameObject);
        }
    }
}
