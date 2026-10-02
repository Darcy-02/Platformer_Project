using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;          
    public float bulletSpeed = 12f;
    public float fireCooldown = 0.3f;
    public KeyCode shootKey = KeyCode.F;

    [Header("Shield")]
    public GameObject shieldVisual;      
    public KeyCode shieldKey = KeyCode.LeftShift;

    public bool isShielding { get; private set; }
    private float nextFireTime;

    void Start()
    {
        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;   


        isShielding = Input.GetKey(shieldKey);
        if (shieldVisual != null) shieldVisual.SetActive(isShielding);


        if (!isShielding && Input.GetKeyDown(shootKey) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    public Vector2 spawnOffset = new Vector2(0.8f, 0.2f);

    void Shoot()
    {
        float dir = Mathf.Sign(transform.localScale.x);

        Vector3 spawnPos = transform.position + new Vector3(dir * spawnOffset.x, spawnOffset.y, 0f);
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(dir * bulletSpeed, 0f);

        Vector3 s = bullet.transform.localScale;
        s.x = Mathf.Abs(s.x) * dir;
        bullet.transform.localScale = s;

        Destroy(bullet, 2f);
    }
}
