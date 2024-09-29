using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public abstract class CharacterHealthManager : MonoBehaviour {
    public TMP_Text healthText, energyText;
    public float maxHealth = 80, health, shield, energy, baseEnergy = 3;
    public List<EffectStatus> activeEffects;
    protected CardsManager cardsManager;
    public GameObject shockObject, burnObject, bleedObject;
    public Animator animator;
    // effect - (damage per turn, no of turns it lasts)
    public Dictionary<EffectStatus,Tuple<int,int>> effectStats = new Dictionary<EffectStatus,Tuple<int,int>>();

    public virtual void Start() {
        cardsManager = CardsManager.instance;
        health = maxHealth;
        activeEffects = new List<EffectStatus>();
        UpdateHealthText();
        animator = GetComponent<Animator>();

        if(!effectStats.ContainsKey(EffectStatus.BURN)) effectStats.Add(EffectStatus.BURN, Tuple.Create(3,2));
        if(!effectStats.ContainsKey(EffectStatus.BLEED)) effectStats.Add(EffectStatus.BLEED, Tuple.Create(3,3));
        if(!effectStats.ContainsKey(EffectStatus.SHOCK)) effectStats.Add(EffectStatus.SHOCK, Tuple.Create(2,1));
    }

    public void StatusEffectsApply() {
        bool hasShock = false, hasBurn = false, hasBleed = false;
        foreach (EffectStatus effect in activeEffects) {
            if(effect == EffectStatus.SHOCK && !hasShock) {
                AddEffects(effectStats[effect].Item1,0,EffectStatus.NONE);
                hasShock = true;
            }
            if(effect == EffectStatus.BURN && !hasBurn) {
                AddEffects(effectStats[effect].Item1,0,EffectStatus.NONE);
                hasBurn = true;
            }
            if(effect == EffectStatus.BLEED && !hasBleed) {
                AddEffects(effectStats[effect].Item1,0,EffectStatus.NONE);
                hasBleed = true;
            }
        }
        if(hasShock) {
            activeEffects.RemoveAt(activeEffects.IndexOf(EffectStatus.SHOCK));
        }
        if(hasBurn) {
            activeEffects.RemoveAt(activeEffects.IndexOf(EffectStatus.BURN));
        }
        if(hasBleed) {
            activeEffects.RemoveAt(activeEffects.IndexOf(EffectStatus.BLEED));
        }
        StatusEffectDisplay();
    }

    public void StatusEffectDisplay() {
        bool hasShock = false, hasBurn = false, hasBleed = false;
        foreach (EffectStatus effect in activeEffects) {
            if(effect == EffectStatus.SHOCK) hasShock = true;
            if(effect == EffectStatus.BURN) hasBurn = true;
            if(effect == EffectStatus.BLEED) hasBleed = true;
        }
        if(hasShock) {
            shockObject.SetActive(true);
        } else {
            shockObject.SetActive(false);
        }
        if(hasBleed) {
            bleedObject.SetActive(true);
        } else {
            bleedObject.SetActive(false);
        }
        if(hasBurn) {
            burnObject.SetActive(true);
        } else {
            burnObject.SetActive(false);
        }
    }

    public void EnergyLowAnim() {
        if(energyText == null) return;
        energyText.GetComponent<Animator>().Play("depleted");
    }

    public void UpdateHealthText() {
        health = Math.Max(0,health);
        healthText.text = health+"/"+maxHealth;
        if(shield > 0) healthText.text = healthText.text + " (+"+shield+")";
    }

    public void AddEffects(CardDescription cardDescription) {
        if(cardDescription.damage > 0) {
            animator.Play("hurt");
            GetComponent<AudioSource>().Play();
        } else if(cardDescription.block > 0) {
            animator.Play("block");
        }
        int attack = cardDescription.damage;
        if(shield > 0) {
            shield -= attack;
            attack = 0;
        }
        if(shield < 0) {
            health += shield;
            shield = 0;
        }
        if(attack != 0) {
            health -= cardDescription.damage;
        }
        if(cardDescription.damage > 0) {
            if(cardDescription.block > 0 && PlayerHealthManager.instance.shield < 7) {
                //medusa fix
                PlayerHealthManager.instance.shield += cardDescription.block;
                PlayerHealthManager.instance.UpdateHealthText();
            }
        } else {
            shield += cardDescription.block;
        }
        if(cardDescription.cardEffect != EffectStatus.NONE) {
            if(!activeEffects.Contains(cardDescription.cardEffect)) {
                for(int i=0;i<effectStats[cardDescription.cardEffect].Item2;i++) {
                    activeEffects.Add(cardDescription.cardEffect);
                }
            }
        }
        UpdateHealthText();
        UpdateEnergyText();
        StatusEffectDisplay();
    }

    public void AddEffects(int damage, int block, EffectStatus status) {
        if(damage > 0) {
            animator.Play("hurt");
            GetComponent<AudioSource>().Play();
        } else if(block > 0) {
            animator.Play("block");
        }
        int attack = damage;
        if(shield > 0) {
            shield -= attack;
            attack = 0;
        }
        if(shield < 0) {
            health += shield;
            shield = 0;
        }
        if(attack != 0) {
            health -= damage;
        }
        shield += block;
        if(status != EffectStatus.NONE) {
            if(!activeEffects.Contains(status)) {
                for(int i=0;i<effectStats[status].Item2;i++) {
                    activeEffects.Add(status);
                }
            }
        }
        UpdateHealthText();
        StatusEffectDisplay();
    }

    public void UpdateEnergyText() {
        if(energyText == null) return;
        energyText.text = energy+"/"+baseEnergy;
    }

    public virtual void PlayTurn() {
        energy = baseEnergy;
        UpdateEnergyText();
        UpdateHealthText();
    }

}