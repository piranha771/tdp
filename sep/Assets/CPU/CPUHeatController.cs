﻿using UnityEngine;
using System.Collections;

public class CPUHeatController : MonoBehaviour {

    public readonly float NPCHealthMultiplier = 0.1f; 

    [SerializeField]
    private float normalTemp = 40;
    [SerializeField]
    private float meltdownTemp = 120;
    [SerializeField]
    private float cooldownRatePerSecond = 1f;
    [SerializeField]
    private float currentTemp;
    [SerializeField]
    private Color coolColor;
    [SerializeField]
    private Color heatColor;

    private GUIText guiCPUTempDisplay;
    private float deltaTime = 0;
    private Color currentColor;

    public float NormalHeat { get { return normalTemp; } set { normalTemp = value; } }
    public float MeltdownTemp { get { return meltdownTemp; } set { meltdownTemp = value; } }
    public float CooldownRatePerSecond { get { return cooldownRatePerSecond; } set { cooldownRatePerSecond = value; } }
    public float CurrentTemp { get { return currentTemp; } set { currentTemp = value; } }
    public Color CoolColor { get { return coolColor; } set { coolColor = value; } }
    public Color HeatColor { get { return heatColor; } set { heatColor = value; } }

	void Start () {
        
        currentTemp = normalTemp;
        // Get reference of the GUITemp Text
        guiCPUTempDisplay = GetComponentInChildren<GUIText>();
	}
	
	void Update () {
        deltaTime += Time.deltaTime;
        if (deltaTime >= 1.0) {
            deltaTime = 0;

            currentTemp -= cooldownRatePerSecond;
            currentTemp = Mathf.Max(currentTemp, normalTemp);
            tempDisplay();
        }
	}

    /// <summary>
    /// Called when a evil NPC bit hits the cpu
    /// </summary>
    /// <param name="npcEvilBit"></param>
    public void Impact(GameObject npcEvilBit) {
        Health health = npcEvilBit.GetComponent<Health>();
        currentTemp += (health.CurrentHealth * NPCHealthMultiplier);

        GameObject[] go = GameObject.FindGameObjectsWithTag("cpuglowmodel");
        
        foreach (var obj in go) {
            Color oldCol = obj.renderer.material.color;
            Color newCol =  new Color(oldCol.r * 1.1f, oldCol.g * 0.9f, oldCol.b * 0.9f);
            obj.renderer.material.color = newCol;
        
        }
        go = GameObject.FindGameObjectsWithTag("cpuglowparticle");
        Debug.Log("NUM: " + go.Length);
        foreach (var obj in go) {
            Color oldCol = obj.renderer.material.GetColor("_TintColor");
            Color newCol = new Color(oldCol.r * 1.1f, oldCol.g * 0.9f, oldCol.b * 0.9f);
            obj.renderer.material.SetColor("_TintColor", newCol);
            Debug.Log("Old" + oldCol.ToString() + " New " + newCol);
        }

        Color oldCol2 = GetComponentInChildren<Light>().color;
        GetComponentInChildren<Light>().color =  new Color(oldCol2.r + 0.1f, oldCol2.g * 0.9f, oldCol2.b * 0.9f);
    }

    /// <summary>
    /// Rewrites the text on the GUI for temperature
    /// </summary>
    private void tempDisplay() {
        guiCPUTempDisplay.text = "CPU Heat\r\n"+currentTemp+" °C";
    }
}
