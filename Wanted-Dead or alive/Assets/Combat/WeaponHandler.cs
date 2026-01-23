using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Transform weaponHolder;
    public LayerMask enemyLayer;

    [Header("UI Crosshair")]
    public GameObject crosshairUI;

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
        if (crosshairUI != null) crosshairUI.SetActive(false);

        if (weaponHolder == null) return;

        weaponAudioSource = weaponHolder.GetComponent<AudioSource>();
        if (weaponAudioSource == null)
        {
            weaponAudioSource = weaponHolder.gameObject.AddComponent<AudioSource>();
        }
    }

    public void EquipWeapon(InventoryItemData newWeapon)
    {
        if (currentWeapon == newWeapon) return;

        UnequipWeapon();

        if (newWeapon.itemType != ItemType.Weapon) return;

        currentWeapon = newWeapon;

        if (crosshairUI != null) crosshairUI.SetActive(true);

        if (newWeapon.WeaponInHandPrefab != null)
        {
            currentWeaponObj = Instantiate(newWeapon.WeaponInHandPrefab, weaponHolder);
            currentWeaponObj.transform.localPosition = Vector3.zero;
            currentWeaponObj.transform.localRotation = Quaternion.identity;

            Collider[] colliders = currentWeaponObj.GetComponentsInChildren<Collider>();
            foreach (var col in colliders)
            {
                col.enabled = false;
            }

            Rigidbody rb = currentWeaponObj.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }
    }

    public void UnequipWeapon()
    {
        if (crosshairUI != null) crosshairUI.SetActive(false);

        if (currentWeaponObj != null)
        {
            Destroy(currentWeaponObj);
        }
        currentWeapon = null;
        currentWeaponObj = null;
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
            if (flintlockShotSound != null) weaponAudioSource.PlayOneShot(flintlockShotSound);
            ShootVisualProjectile();
        }
        else
        {
            if (weaponSwingSound != null) weaponAudioSource.PlayOneShot(weaponSwingSound);
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, currentWeapon.weaponRange, enemyLayer))
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.weaponDamage);
            }
        }
    }

    void ShootVisualProjectile()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;
            float distanceToTarget;

            if (Physics.Raycast(ray, out hit, currentWeapon.weaponRange))
            {
                targetPoint = hit.point;
                distanceToTarget = Vector3.Distance(firePoint.position, hit.point);
            }
            else
            {
                targetPoint = ray.GetPoint(currentWeapon.weaponRange);
                distanceToTarget = currentWeapon.weaponRange;
            }

            Vector3 directionToTarget = (targetPoint - firePoint.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(directionToTarget));

            BulletMover mover = bullet.AddComponent<BulletMover>();
            mover.direction = directionToTarget;
            mover.speed = bulletSpeed;
            mover.enemyLayer = enemyLayer;
            mover.maxDistance = distanceToTarget;

            Collider bulletCol = bullet.GetComponent<Collider>();
            CharacterController playerController = GetComponentInParent<CharacterController>();
            if (bulletCol != null && playerController != null)
                Physics.IgnoreCollision(bulletCol, playerController);

            Destroy(bullet, 5f);
        }
    }
}