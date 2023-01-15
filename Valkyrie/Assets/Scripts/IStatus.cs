using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    float HP { get; set; }
    float MaxHP { get; }
    float MP { get; set; }
    float MaxMP { get; }
    float AttackPos { get; }
    float DefensePos { get; }

    Transform transform { get; }
    Action onHPchange { get; set; }
    Action onMPchange { get; set; }

    void AttackTarget(IStatus target);
    void TakeDamage(float damage);

    void Dead();
    
}
