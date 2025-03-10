using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public event Action OnAttackFrame;
    
    public void Attack()
    {
        OnAttackFrame?.Invoke();
    }
}
