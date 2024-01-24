using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/DamageBuff")]
public class DamageBuff : PowerUpEffect
{
    [SerializeField] private int _amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerController>()._damage += _amount;
    }
}
