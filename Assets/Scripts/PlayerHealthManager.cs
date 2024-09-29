using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthManager : CharacterHealthManager {

    public static PlayerHealthManager instance;

    private void Awake() {
        instance = this;
    }

    public override void Start() {
        base.Start();
        energy = 100;
    }

    void Update() {
        if(health <= 0) {
           GameManager.instance.LoseScreen();
        }
    }

    public override void PlayTurn() {
        energy = baseEnergy;
        UpdateEnergyText();
        UpdateHealthText();
    }

    void OnMouseDown() {
        if(!cardsManager.isCardActive || cardsManager.activeCard.IsOffensive()) return;
        if(!cardsManager.PlayActiveCard(this)) {
            //say not enough energy
            PlayerHealthManager.instance.EnergyLowAnim();
        }
    }

}