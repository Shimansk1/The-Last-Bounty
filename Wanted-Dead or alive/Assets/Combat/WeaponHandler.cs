using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Transform weaponHolder;
    public LayerMask enemyLayer;

    private InventoryItemData currentWeapon;
    private GameObject currentWeaponObj;
    private float lastAttackTime;
    private AudioSource weaponAudioSource;

    [Header("Projectile settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 30f;

    [Header("Sound Effects")]
    public AudioClip weaponSwingSound;
    public AudioClip flintlockShotSound;

    void Start()
    {
        weaponAudioSource = weaponHolder.GetComponent<AudioSource>();
        if (weaponAudioSource == null)
        {
            weaponAudioSource = weaponHolder.gameObject.AddComponent<AudioSource>();
        }
    }

    public void EquipWeapon(InventoryItemData newWeapon)
    {
        if (newWeapon.itemType != ItemType.Weapon)
            return;

        if (currentWeaponObj != null)
            Destroy(currentWeaponObj);

        currentWeapon = newWeapon;

        if (newWeapon.WeaponInHandPrefab != null && weaponHolder != null)
        {
            currentWeaponObj = Instantiate(newWeapon.WeaponInHandPrefab, weaponHolder);
            currentWeaponObj.transform.localPosition = Vector3.zero;
            currentWeaponObj.transform.localRotation = Quaternion.identity;
        }
    }

    void Update()
    {
        if (currentWeapon == null) return;

        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + currentWeapon.attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        if (currentWeapon.weaponRange >= 20)
        {
            if (flintlockShotSound != null)
            {
                weaponAudioSource.PlayOneShot(flintlockShotSound);
            }
            ShootVisualProjectile();
        }
        else
        {
            if (weaponSwingSound != null)
            {
                weaponAudioSource.PlayOneShot(weaponSwingSound);
            }
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, currentWeapon.weaponRange, enemyLayer))
        {
            if (hit.collider.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.TakeDamage(currentWeapon.weaponDamage);
            }
        }

        // TODO: animace
    }

    void ShootVisualProjectile()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 shootDirection = (Camera.main.transform.forward + Camera.main.transform.right * -0.05f).normalized;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));

            BulletMover mover = bullet.AddComponent<BulletMover>();
            mover.direction = shootDirection;
            mover.speed = bulletSpeed;
            mover.enemyLayer = enemyLayer;

            Collider bulletCol = bullet.GetComponent<Collider>();
            Collider playerCol = GetComponentInParent<Collider>();
            if (bulletCol != null && playerCol != null)
                Physics.IgnoreCollision(bulletCol, playerCol);

            Destroy(bullet, 5f);
        }
    }
}