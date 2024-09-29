using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlayer : MonoBehaviour {

    public static bool CanPlay(CardDescription cardToPlay, CharacterHealthManager target) {
        CharacterHealthManager initiator = TurnManager.instance.currentTurnCharacter;
        if(initiator.energy < cardToPlay.energyCost) {
            return false;
        }
        return true;
    }

    public static void PlayCard(CardDescription cardToPlay, CharacterHealthManager target) {
        CharacterHealthManager initiator = TurnManager.instance.currentTurnCharacter;
        initiator.energy -= cardToPlay.energyCost;
        initiator.energy = Math.Max(initiator.energy, 0);
        initiator.UpdateEnergyText();
        if(cardToPlay.damage > 0) {
            initiator.animator.Play("attack");
        }
        target.AddEffects(cardToPlay);
    }

    public static void PlayAI(int damage, int block, EffectStatus status, CharacterHealthManager target) {
        target.AddEffects(damage,block,status);
    }

}
