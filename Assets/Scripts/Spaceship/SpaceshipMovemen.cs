using System.Collections;
using UnityEngine;

public class SpaceshipMovemen : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootingPoint;

    float originalSpeed;
    bool isMultiShoot = false;

    Coroutine speedUpCoroutine;
    bool isSpeedUpActive = false;

    public GameObject doubleShipPrefab;
    GameObject doubleShipInstance;

    void Start()
    {
        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Ins.state != GameState.Playing)
            return;

        float xDir = Input.GetAxisRaw("Horizontal");

        if ((xDir < 0 && transform.position.x <= -7.5) || (xDir > 0 && transform.position.x >= 7.5))
        {
            return;
        }
        transform.position += Vector3.right * xDir * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot ()
    {
        if (GameController.Ins.state != GameState.Playing)
            return;

        if (projectile && shootingPoint)
        {
            if (AudioController.Ins)
            {
                AudioController.Ins.PlaySound(AudioController.Ins.hitSound);
            }

            if (isMultiShoot)
            {
                Instantiate(projectile, shootingPoint.position + Vector3.left * 0.5f, Quaternion.identity);
                Instantiate(projectile, shootingPoint.position, Quaternion.identity);
                Instantiate(projectile, shootingPoint.position + Vector3.right * 0.5f, Quaternion.identity);
            }
            else
            {
                Instantiate(projectile, shootingPoint.position, Quaternion.identity);
            }
        }
    }

    public void ActivatePowerUp(PowerUpType type, float duration)
    {
        if (GameController.Ins.state != GameState.Playing)
            return;
        StartCoroutine(HandlePowerUp(type, duration));
    }

    private IEnumerator HandlePowerUp(PowerUpType type, float duration)
    {
        switch (type)
        {
            case PowerUpType.SpeedUp:
                if (isSpeedUpActive)
                {
                    StopCoroutine(speedUpCoroutine);
                    speed = originalSpeed; 
                }

                speed *= 1.5f;
                isSpeedUpActive = true;
                speedUpCoroutine = StartCoroutine(SpeedUp(duration));
                break;

            case PowerUpType.DoubleShip:
                if (!doubleShipInstance && doubleShipPrefab)
                {
                    doubleShipInstance = Instantiate(doubleShipPrefab, transform.position + Vector3.left * 1.5f, Quaternion.identity);

                    DoubleShipShooter shooter = doubleShipInstance.GetComponent<DoubleShipShooter>();
                    if (shooter != null)
                    {
                        shooter.mainShip = this.transform; 
                    }
                }
                yield return new WaitForSeconds(duration);
                if (doubleShipInstance)
                {
                    Destroy(doubleShipInstance);
                    doubleShipInstance = null;
                }
                break;

            case PowerUpType.MultiShoot:
                isMultiShoot = true;
                yield return new WaitForSeconds(duration);
                isMultiShoot = false;
                break;
        }
    }

    private IEnumerator SpeedUp(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        isSpeedUpActive = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy.ToString()))
        {
            GameController.Ins.SetGameOver();
            AudioController.Ins.PlaySound(AudioController.Ins.gameover);

            Destroy(collision.gameObject);
        }
    }
}
