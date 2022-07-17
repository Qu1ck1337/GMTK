using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationCaller : MonoBehaviour
{
    [SerializeField]
    private Player _playerComponent;

    public void AttackUnit_AnimationEvent()
    {
        _playerComponent.AttackUnit_AnimationEvent();
    }
}
