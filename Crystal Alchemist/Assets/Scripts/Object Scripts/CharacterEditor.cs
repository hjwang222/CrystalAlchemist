using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditor : Interactable
{
    [SerializeField]
    private SimpleSignal openMenuSignal;

    public override void doSomethingOnSubmit()
    {       
        openMenuSignal.Raise();
    }
}
