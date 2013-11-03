﻿using UnityEngine;
using System.Collections;

public class ShootController : MonoBehaviour {
    private ShootWithBullet scriptSWB;
    private ShootWithLine scriptSWL;
	// Use this for initialization
	void Start () {
        scriptSWB = transform.GetChild(0).GetComponent<ShootWithBullet>();
        scriptSWL = transform.GetChild(0).GetComponent<ShootWithLine>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// Direct shoot command to right shoot script
    /// </summary>
    /// <param name="npcEnemy"> target </param>
    /// <param name="towerWeapon"> weapon </param>
    /// <param name="towerTyp"> weapon typ </param>
    public void startShoot(GameObject npcEnemy, GameObject towerWeapon, string towerTyp) {
        switch (towerTyp) {
            
            case "bulletTower":
                
                scriptSWB.shoot(npcEnemy, towerWeapon);
                break;

            case "lineBulletTower":
                scriptSWL.shoot(npcEnemy, towerWeapon);
                break;
            default:
                break;

        }

     
      
    }

    public void stopShooting(string towerTyp) {

        switch (towerTyp) {

            case "bulletTower":
                scriptSWB.stopShooting();
              
                break;

            case "lineBulletTower":
                scriptSWL.stopShooting();
                break;
            default:
                break;

        }
    }
}
