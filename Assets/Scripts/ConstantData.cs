using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantData : MonoBehaviour {

    public static ConstantData instance;
    public List<CardDescription> deck;

    private void Awake() {
        instance = this;
        DontDestroyOnLoad(instance);
    }

}