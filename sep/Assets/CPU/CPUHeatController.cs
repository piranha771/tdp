﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CPUHeatController : MonoBehaviour {

    public readonly float NPCHealthMultiplier = 2f; 

    [SerializeField]
    private float normalTemp = 40;
    [SerializeField]
    private float meltdownTemp = 120;
    [SerializeField]
    private float cooldownRatePerSecond = 2f;
    [SerializeField]
    private float additionalHeatOnEvilBit = 10f;
    [SerializeField]
    private float tempFreezeTimeOnUnknownBit = 10f;

    [SerializeField]
    private List<GameObject> glowParts;
    [SerializeField]
    private List<GameObject> glowSprites;
    [SerializeField]
    private Light glowLight;

    [SerializeField]
    private Color coolColorGlowParts;
    [SerializeField]
    private Color hotColorGlowParts;
    [SerializeField]
    private Color coolColorSpriteParts;
    [SerializeField]
    private Color hotColorSpriteParts;
    [SerializeField]
    private Color coolColorLight;
    [SerializeField]
    private Color hotColorLight;

    private float currentTemp;
    private float deltaTimeUpdateTemp = 0;
    private float deltaTimeFreezeTemp = 0;

    #region Getter And Setter
    public float NormalHeat { get { return normalTemp; } set { normalTemp = value; } }
    public float MeltdownTemp { get { return meltdownTemp; } set { meltdownTemp = value; } }
    public float CooldownRatePerSecond { get { return cooldownRatePerSecond; } set { cooldownRatePerSecond = value; } }
    public float AdditionalHeatOnEvilBit { get { return additionalHeatOnEvilBit; } set { additionalHeatOnEvilBit = value; } }
    public float TempFreezeTimeOnUnknownBit { get { return tempFreezeTimeOnUnknownBit; } set { tempFreezeTimeOnUnknownBit = value; } }
    public float CurrentTemp { get { return currentTemp; } set { currentTemp = value; } }
    #endregion

    void Start () {
        currentTemp = normalTemp;

	}
	
	void Update () {
        if (deltaTimeFreezeTemp > 0) {
            deltaTimeFreezeTemp -= Time.deltaTime;
            CalculateColors();
            return;
        }

        deltaTimeUpdateTemp -= Time.deltaTime;
        if (deltaTimeUpdateTemp <= 0) {
            deltaTimeUpdateTemp = 1.0f;

            currentTemp = Mathf.Max(currentTemp - cooldownRatePerSecond, normalTemp);
            CalculateColors();
        }
	}

    /// <summary>
    /// Called when a evil NPC bit hits the cpu
    /// </summary>
    public void ImpactEvil() {
        currentTemp = Mathf.Min(currentTemp + additionalHeatOnEvilBit, meltdownTemp);
    }

    public void ImpactUnknown() {
        deltaTimeFreezeTemp = tempFreezeTimeOnUnknownBit;
    }

    /// <summary>
    /// Creates a gradient color upon two colors and a percentage
    /// </summary>
    /// <param name="startColor">First color the gradiend starts from</param>
    /// <param name="endColor">Second color the gradient ends with</param>
    /// <param name="modifier">Percentage with 1.0 means 100 percent</param>
    /// <returns></returns>
    private Color GetGradientColor(Color startColor, Color endColor, float modifier) {
        return new Color(startColor.r * (1.0f - modifier) + endColor.r * modifier,
            startColor.g * (1.0f - modifier) + endColor.g * modifier,
            startColor.b * (1.0f - modifier) + endColor.b * modifier);
    }

    private void CalculateColors() {
        float percentage = (currentTemp - normalTemp) / (meltdownTemp - normalTemp);

        foreach (var obj in glowParts) {
            obj.renderer.material.color = GetGradientColor(coolColorGlowParts, hotColorGlowParts, percentage);
        }

        foreach (var obj in glowSprites) {
            obj.renderer.material.SetColor("_TintColor", GetGradientColor(coolColorSpriteParts, hotColorSpriteParts, percentage));
        }

        glowLight.color = GetGradientColor(coolColorLight, hotColorLight, percentage);
    }
}
