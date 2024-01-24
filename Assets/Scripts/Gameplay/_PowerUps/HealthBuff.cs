using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HealthBuff")]
public class HealthBuff : PowerUpEffect
{
    [SerializeField] private int _amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>()._amountOfHearts += _amount;
        target.GetComponent<PlayerController>()._health += _amount;
    }
}
