using UnityEngine;

public class DoubleShipShooter : MonoBehaviour
{
    public Transform mainShip; // gán từ ngoài vào
    public Vector3 offset = new Vector3(-1.5f, 0, 0); // vị trí lệch bên trái
    public GameObject projectile;
    public Transform shootingPoint;
    public float shootInterval = 0.5f;
    float shootTimer;

    void Update()
    {
        // Luôn theo tàu chính, cách một khoảng offset
        if (mainShip)
            transform.position = mainShip.position + offset;

        // Bắn tự động theo khoảng thời gian
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        if (projectile && shootingPoint)
        {
            Instantiate(projectile, shootingPoint.position, Quaternion.identity);
            if (AudioController.Ins)
            {
                AudioController.Ins.PlaySound(AudioController.Ins.hitSound);
            }
        }
    }
}
