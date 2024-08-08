using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header ("===== Prefabs =====")]
    [SerializeField] private GameObject SmallBulletPrefab;
    [SerializeField] private GameObject LargeBulletPrefab;
    [SerializeField] private GameObject MissilePrefab;

    [Space (10)]
    [Header ("===== Outer entities =====")]
    [SerializeField] private GameObject m_wings;

    [Space (10)]
    [Header ("===== Variables =====")]
    [SerializeField] private float weaponsCooldown;

    [HideInInspector] public int powerLevel { get; private set; }    // Need to think through the powerup system

    private bool isShooting;

    void Awake(){
        cooldownCounter = weaponsCooldown;
        isHullGunsShooting = true;
        SetPowerLevel(0);
        isShooting = false;
    }

    public void SetPowerLevel(int level){
        if (level > 6){ // this is not nice
            return;
        }
        powerLevel = level;
        m_wings.SetActive(powerLevel > 1);
    }

    public void SetPowerLevel(bool powerUp){
        int targetPower = powerLevel + (powerUp ? 1 : -1);
        SetPowerLevel(targetPower);
    }

    public void Shoot(bool state){
        isShooting = state;
    }

    private void shootMainGun(GameObject projectile){
        Instantiate(projectile, transform.GetChild(0).position, Quaternion.identity);
    }

    private void shootHullGuns(){
        for (int i = 1; i < 3; ++i){
            Instantiate(SmallBulletPrefab, transform.GetChild(i).position, Quaternion.identity);
        }
    }

    private void shootWingGuns(GameObject projectile){
        for (int i = 3; i < 5; ++i){
            Instantiate(projectile, transform.GetChild(i).position, Quaternion.identity);
        }
    }

    private float cooldownCounter;
    private bool isHullGunsShooting;
    void Update(){
        if (isShooting){
            if (cooldownCounter < 0){
                // after small refactor, still looks intimidating
                // dunno how to make it simpler without making it more complicated
                switch (powerLevel){
                    case 0:
                        shootMainGun(SmallBulletPrefab);
                        break;
                    case 1:
                        shootMainGun(SmallBulletPrefab);
                        shootHullGuns();
                        break;
                    case 2:
                        shootMainGun(SmallBulletPrefab);
                        if (isHullGunsShooting){
                            isHullGunsShooting = false;
                            shootHullGuns();
                        }
                        else{
                            isHullGunsShooting = true;
                            shootWingGuns(SmallBulletPrefab);
                        }
                        break;
                    case 3:
                        shootMainGun(SmallBulletPrefab);
                        shootHullGuns();
                        shootWingGuns(SmallBulletPrefab);
                        break;
                    case 4:
                        shootMainGun(LargeBulletPrefab);
                        shootHullGuns();
                        shootWingGuns(SmallBulletPrefab);
                        break;
                    case 5:
                        shootMainGun(LargeBulletPrefab);
                        shootHullGuns();
                        shootWingGuns(LargeBulletPrefab);
                        break;
                    case 6:
                    if (isHullGunsShooting){
                            shootMainGun(MissilePrefab);
                            isHullGunsShooting = false;
                        }
                        else{
                            isHullGunsShooting = true;
                        }
                        shootHullGuns();
                        shootWingGuns(LargeBulletPrefab);
                        break;
                    default:
                        return;
                }
                cooldownCounter = weaponsCooldown;
            }
        }
        if (cooldownCounter > 0){
            cooldownCounter -= Time.deltaTime;
        }
    }
}
