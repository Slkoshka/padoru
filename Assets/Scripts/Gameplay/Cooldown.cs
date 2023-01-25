using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown
{
    public float TimeLeft { get; private set; } = 0;
    public float CooldownTime { get; set; }
    public bool IsOnCooldown => TimeLeft > 0;

    public Cooldown(float cooldown)
    {
        CooldownTime = cooldown;
    }

    public bool Trigger()
    {
        if (IsOnCooldown)
        {
            return false;
        }

        TimeLeft = CooldownTime;
        return true;
    }

    public void Update(float dt)
    {
        TimeLeft -= dt;
    }
}
