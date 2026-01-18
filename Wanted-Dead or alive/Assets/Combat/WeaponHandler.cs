using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Transform weaponHolder;
    public LayerMask enemyLayer;

    [Header("UI Crosshair")]
    public GameObject crosshairUI; // SEM PØETÁHNEŠ TEN OBRÁZEK Z CANVASU

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
        // Pro jistotu na zaèátku crosshair schováme
        if (crosshairUI != null) crosshairUI.SetActive(false);

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
        if (currentWeapon == newWeapon) return;

        UnequipWeapon(); // Tím se crosshair vypne...

        if (newWeapon.itemType != ItemType.Weapon)
            return;

        currentWeapon = newWeapon;

        // ... a tady ho ZAPNEME, protože máme zbraò
        if (crosshairUI != null) crosshairUI.SetActive(true);

        if (newWeapon.WeaponInHandPrefab != null)
        {
            currentWeaponObj = Instantiate(newWeapon.WeaponInHandPrefab, weaponHolder);
            currentWeaponObj.transform.localPosition = Vector3.zero;
            currentWeaponObj.transform.localRotation = Quaternion.identity;

            // Vypnutí kolizí zbranì
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
        // Tady ho VYPNEME, když zbraò dáváme pryè
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
            // 1. ZJISTÍME CÍL A VZDÁLENOST
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;
            float distanceToTarget; // Tuhle promìnnou pošleme kulce

            // Pokud nìco trefíme (nepøítele, zeï)
            if (Physics.Raycast(ray, out hit, currentWeapon.weaponRange))
            {
                targetPoint = hit.point;
                // Vzdálenost od hlavnì k bodu zásahu
                distanceToTarget = Vector3.Distance(firePoint.position, hit.point);
            }
            else
            {
                // Pokud míøíme do vzduchu, poletí to na maximální dostøel
                targetPoint = ray.GetPoint(currentWeapon.weaponRange);
                distanceToTarget = currentWeapon.weaponRange;
            }

            // 2. VÝPOÈET SMÌRU
            Vector3 directionToTarget = (targetPoint - firePoint.position).normalized;

            // 3. INSTANCIACE
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(directionToTarget));

            BulletMover mover = bullet.AddComponent<BulletMover>();
            mover.direction = directionToTarget;
            mover.speed = bulletSpeed;
            mover.enemyLayer = enemyLayer;

            // --- TADY TO PØEDÁVÁME ---
            mover.maxDistance = distanceToTarget;
            // -------------------------

            // Ignorace kolizí s hráèem
            Collider bulletCol = bullet.GetComponent<Collider>();
            CharacterController playerController = GetComponentInParent<CharacterController>();
            if (bulletCol != null && playerController != null)
                Physics.IgnoreCollision(bulletCol, playerController);

            // Pojistka (kdyby náhodou letìla déle než 5s)
            Destroy(bullet, 5f);
        }
    }
}