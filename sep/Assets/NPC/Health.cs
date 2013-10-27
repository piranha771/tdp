﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    [SerializeField]
    private int currentHealth = 100;

    public int CurrentHealth { get { return Mathf.Max(currentHealth, 0); } set { currentHealth = value; } }
    public bool IsDead { get { return currentHealth == 0; } set { currentHealth = 0; } }

	void Start () {

	}
	
	void Update () {
      
        if (currentHealth < 0) {
            
            Destroy(gameObject);
        }
	}

    public void tackeDamage(int damage) {
        currentHealth -= damage;
    }
}
