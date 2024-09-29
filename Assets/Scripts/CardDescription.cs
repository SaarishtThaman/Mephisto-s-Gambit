using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDescription : MonoBehaviour {
    public int energyCost;
    public int damage;
    public int block;
    public bool attackAll;
    public EffectStatus cardEffect;

    public CardType cardType;

    public enum CardType {
        OFFENSIVE,
        DEFENSIVE
    }

    public bool IsOffensive() {
        return cardType == CardType.OFFENSIVE;
    }

    public bool IsDefensive() {
        return cardType == CardType.DEFENSIVE;
    }

}