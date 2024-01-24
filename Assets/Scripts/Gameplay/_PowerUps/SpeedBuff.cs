using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBuff")]
public class SpeedBuff : PowerUpEffect
{
    [SerializeField] private int _amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>()._movementSpeed += _amount;
    }
}
