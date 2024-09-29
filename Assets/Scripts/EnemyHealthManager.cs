using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealthManager : CharacterHealthManager {

    public float maxAttack, maxDefense;
    public EffectStatus canApplyStatusType = EffectStatus.NONE;
    public float chanceOfStatusEffect = 0.1f;

    public override void Start() {
        base.Start();
    }

    void OnMouseDown() {
        if(!cardsManager.isCardActive || cardsManager.activeCard.IsDefensive()) return;
        if(!cardsManager.PlayActiveCard(this)) {
            //say not enough energy
            PlayerHealthManager.instance.EnergyLowAnim();
        }
    }

    void Update() {
        if(health <= 0) {
            animator.Play("death");
            Destroy(gameObject, 0.25f);
        }
    }

    public override void PlayTurn() {
        bool defend = UnityEngine.Random.Range(0f, 1f) < 0.2f ? true : false;
        if(defend) {
            int block = (int)UnityEngine.Random.Range(maxDefense/2, maxDefense);
            CardPlayer.PlayAI(0,block,EffectStatus.NONE,this);
        } else {
            animator.Play("attack");
            int attack = (int)UnityEngine.Random.Range(maxAttack/2, maxAttack);
            bool applyStatus = false;
            EffectStatus statusToApply = EffectStatus.NONE;
            if(canApplyStatusType != EffectStatus.NONE) {
                applyStatus = UnityEngine.Random.Range(0f, 1f) < chanceOfStatusEffect ? true : false;
            }
            if(applyStatus) statusToApply = canApplyStatusType;
            CardPlayer.PlayAI(attack,0,statusToApply,PlayerHealthManager.instance);
        }
    }

}
