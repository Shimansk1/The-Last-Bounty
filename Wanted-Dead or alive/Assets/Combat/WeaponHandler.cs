using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Transform weaponHolder;
    public LayerMask enemyLayer;
    public LayerMask targetLayer;

    [Header("UI Crosshair")]
    public GameObject crosshairUI;

    private InventoryItemData currentWeapon;
    private GameObject currentWeaponObj;
    private float lastAttackTime;
    private AudioSource weaponAudioSource;

    [Header("Visual Effects General")]
    public float recoilAmount = 0.1f;
    public float recoilRecoverSpeed = 5f;
    public float meleeThrustAmount = 0.2f;

    public GameObject bloodEffectPrefab;

    [Header("Projectile settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 30f;

    [Header("--- REVOLVER SETTINGS ---")]
    public Transform revolverFirePoint;
    public GameObject revolverMuzzleFlash;
    public AudioClip revolverShotSound;

    [Header("--- RIFLE SETTINGS ---")]
    public Transform rifleFirePoint;
    public GameObject rifleMuzzleFlash;
    public AudioClip rifleShotSound;

    [Header("Melee Sound Effects")]
    public AudioClip[] weaponSwingSounds;

    private Vector3 originalWeaponPos;

    void Start()
    {
        if (weaponHolder != null)
        {
            originalWeaponPos = weaponHolder.localPosition;
        }

        if (crosshairUI != null) crosshairUI.SetActive(false);

        if (weaponHolder == null) return;

        weaponAudioSource = weaponHolder.GetComponent<AudioSource>();
        if (weaponAudioSource == null)
        {
            weaponAudioSource = weaponHolder.gameObject.AddComponent<AudioSource>();
        }
    }

    public void EquipItem(InventoryItemData newItem)
    {
        if (currentWeapon == newItem) return;

        UnequipWeapon();

        currentWeapon = newItem;

        if (crosshairUI != null)
        {
            crosshairUI.SetActive(newItem.itemType == ItemType.Weapon);
        }

        if (newItem.ItemInHandPrefab != null)
        {
            currentWeaponObj = Instantiate(newItem.ItemInHandPrefab, weaponHolder);
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

        if (weaponHolder != null)
        {
            weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, originalWeaponPos, Time.deltaTime * recoilRecoverSpeed);
        }

        if (Input.GetMouseButtonDown(0) && currentWeapon.itemType == ItemType.Weapon && Time.time >= lastAttackTime + currentWeapon.attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        if (currentWeapon.weaponRange >= 20)
        {
            bool isRifle = currentWeapon.name.ToLower().Contains("rifle") || currentWeapon.name.ToLower().Contains("puska");

            Transform activeFirePoint = isRifle ? rifleFirePoint : revolverFirePoint;
            GameObject activeMuzzleFlash = isRifle ? rifleMuzzleFlash : revolverMuzzleFlash;
            AudioClip activeShotSound = isRifle ? rifleShotSound : revolverShotSound;

            if (activeShotSound != null) weaponAudioSource.PlayOneShot(activeShotSound);

            ApplyRecoil();
            SpawnMuzzleFlash(activeMuzzleFlash, activeFirePoint);
            ShootVisualProjectile(activeFirePoint);
        }
        else
        {
            if (weaponSwingSounds != null && weaponSwingSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, weaponSwingSounds.Length);
                AudioClip randomSound = weaponSwingSounds[randomIndex];

                if (randomSound != null)
                {
                    weaponAudioSource.PlayOneShot(randomSound);
                }
            }

            ApplyMeleeThrust();
        }

        LayerMask hitMask = enemyLayer | targetLayer;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, currentWeapon.weaponRange, hitMask))
        {
            EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.weaponDamage);

                if (bloodEffectPrefab != null)
                {
                    GameObject blood = Instantiate(bloodEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(blood, 2f);
                }
            }

            BottleTarget bottle = hit.collider.GetComponent<BottleTarget>();
            if (bottle != null)
            {
                bottle.Shatter(hit.point, Camera.main.transform.forward);

                if (ShootingRangeManager.Instance != null)
                {
                    ShootingRangeManager.Instance.AddScore();
                }
            }
        }
    }

    void SpawnMuzzleFlash(GameObject flashPrefab, Transform fp)
    {
        if (flashPrefab != null && fp != null)
        {
            GameObject flash = Instantiate(flashPrefab, fp.position, fp.rotation);
            Destroy(flash, 2f);
        }
    }

    void ApplyRecoil()
    {
        if (weaponHolder != null)
        {
            weaponHolder.localPosition -= Vector3.forward * recoilAmount;
        }
    }

    void ApplyMeleeThrust()
    {
        if (weaponHolder != null)
        {
            weaponHolder.localPosition += Vector3.forward * meleeThrustAmount;
        }
    }

    void ShootVisualProjectile(Transform fp)
    {
        if (bulletPrefab != null && fp != null)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;
            float distanceToTarget;

            if (Physics.Raycast(ray, out hit, currentWeapon.weaponRange))
            {
                targetPoint = hit.point;
                distanceToTarget = Vector3.Distance(fp.position, hit.point);
            }
            else
            {
                targetPoint = ray.GetPoint(currentWeapon.weaponRange);
                distanceToTarget = currentWeapon.weaponRange;
            }

            Vector3 directionToTarget = (targetPoint - fp.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, fp.position, Quaternion.LookRotation(directionToTarget));

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