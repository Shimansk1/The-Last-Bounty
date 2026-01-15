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
        // Pojistka, pokud zapomeneš pøiøadit weaponHolder v inspektoru
        if (weaponHolder == null)
        {
            Debug.LogError("CHYBA: WeaponHandler nemá pøiøazený Weapon Holder!");
            return;
        }

        weaponAudioSource = weaponHolder.GetComponent<AudioSource>();
        if (weaponAudioSource == null)
        {
            weaponAudioSource = weaponHolder.gameObject.AddComponent<AudioSource>();
        }
    }

    public void EquipWeapon(InventoryItemData newWeapon)
    {
        // Pokud už tuhle zbraò máme, nic nedìláme (prevence restartu animací atd.)
        if (currentWeapon == newWeapon) return;

        // Nejdøív zahodíme starou zbraò
        UnequipWeapon();

        if (newWeapon.itemType != ItemType.Weapon)
            return;

        currentWeapon = newWeapon;

        if (newWeapon.WeaponInHandPrefab != null)
        {
            // Instanciace zbranì do ruky
            currentWeaponObj = Instantiate(newWeapon.WeaponInHandPrefab, weaponHolder);
            currentWeaponObj.transform.localPosition = Vector3.zero;
            currentWeaponObj.transform.localRotation = Quaternion.identity;

            // --- OPRAVA KOLIZÍ ---
            // Najdeme všechny collidery na zbrani a vypneme je, aby neodstrkovaly hráèe
            Collider[] colliders = currentWeaponObj.GetComponentsInChildren<Collider>();
            foreach (var col in colliders)
            {
                col.enabled = false;
            }
            // Pokud má zbraò Rigidbody, musíme ho nastavit na Kinematic, aby nepadala
            Rigidbody rb = currentWeaponObj.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }
    }

    public void UnequipWeapon()
    {
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

        // Støelba
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

        // Raycast pro hitscan (okamžitý zásah)
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, currentWeapon.weaponRange, enemyLayer))
        {
            if (hit.collider.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.TakeDamage(currentWeapon.weaponDamage);
            }
        }
    }

    void ShootVisualProjectile()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // Upraven smìr støelby pøesnì podle kamery
            Vector3 shootDirection = Camera.main.transform.forward;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));

            BulletMover mover = bullet.AddComponent<BulletMover>();
            mover.direction = shootDirection;
            mover.speed = bulletSpeed;
            mover.enemyLayer = enemyLayer;

            // Ignorovat kolize støely s hráèem (pro jistotu)
            Collider bulletCol = bullet.GetComponent<Collider>();
            // Zkusíme najít collider hráèe (pøedpokládáme CharacterController nebo CapsuleCollider na rodièi WeaponHandleru)
            Collider playerCol = GetComponentInParent<Collider>(); // Nebo CharacterController

            if (bulletCol != null && playerCol != null)
                Physics.IgnoreCollision(bulletCol, playerCol);

            Destroy(bullet, 5f);
        }
    }
}