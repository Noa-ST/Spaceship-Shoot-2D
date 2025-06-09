using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpaceshipMovemen : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip shootingSound;

    GameController gc;

    float originalSpeed;
    bool isMultiShoot = false;

    public GameObject doubleShipPrefab;
    GameObject doubleShipInstance;

    void Start()
    {
        gc = FindObjectOfType<GameController>();
        originalSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.IsGameOver())
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
        if (projectile && shootingPoint)
        {
            if (aus && shootingSound)
            {
                aus.PlayOneShot(shootingSound);
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
        StartCoroutine(HandlePowerUp(type, duration));
    }

    private IEnumerator HandlePowerUp(PowerUpType type, float duration)
    {
        switch (type)
        {
            case PowerUpType.SpeedUp:
                speed *= 1.5f;
                yield return new WaitForSeconds(duration);
                speed = originalSpeed;
                break;

            case PowerUpType.DoubleShip:
                if (!doubleShipInstance && doubleShipPrefab)
                {
                    doubleShipInstance = Instantiate(doubleShipPrefab, transform.position + Vector3.left * 1.5f, Quaternion.identity);

                    DoubleShipShooter shooter = doubleShipInstance.GetComponent<DoubleShipShooter>();
                    if (shooter != null)
                    {
                        shooter.mainShip = this.transform; // Cho nó follow tàu chính
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            gc.SetGameOverState(true);

            Destroy(collision.gameObject);

            Debug.Log("Enemy cham player");
        }
    }
}
