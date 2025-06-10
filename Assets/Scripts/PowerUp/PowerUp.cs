using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float duration = 5f; // Thời gian hiệu lực

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Player.ToString())) 
        {
            SpaceshipMovemen player = collision.GetComponent<SpaceshipMovemen>();
            if (player != null)
            {
                player.ActivatePowerUp(type, duration);
                AudioController.Ins.PlaySound(AudioController.Ins.gotCollectable);
            }
            Destroy(gameObject);
        }

        if (collision.CompareTag(GameTag.DeadZone.ToString()))
        {
            Destroy(gameObject);
        }
    }
}
