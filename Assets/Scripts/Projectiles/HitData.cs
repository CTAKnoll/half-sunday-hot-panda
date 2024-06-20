using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitData
{
    public int damage;
    public Vector3 knockback;
    public float hitStun;
    public Vector3 hitPoint;
    public Vector3 normal;
    public IHurtbox hurtbox;
    public IHitDetector hitDetector;

    public bool Validate()
    {
        if (hurtbox == null)
            return false;

        if (!hurtbox.CheckHit(this))
            return false;

        if (hurtbox.HurtResponder != null && !hurtbox.HurtResponder.VerifyHurt(this))
            return false;

        if (hitDetector.HitResponder != null && !hitDetector.HitResponder.VerifyHit(this))
            return false;

        return true;
    }
}

public enum HurtboxType
{
    Player  = 1 << 0,
    Enemy   = 1 << 1,
    NoTeam = 1 << 2,
}

[System.Flags]
public enum HurtboxMask
{
    None    = 0,
    Player  = 1 << 0,
    Enemy   = 1 << 1,
    NoTeam = 1 << 2,
}

public interface IHitResponder
{
    /// <summary>
    /// Allows the <c>IHitResponder</c> to validate hits before they respond to it. Only information about the hitbox cast is typically found in the <paramref name="hitData"/> at this time.
    /// </summary>
    /// <param name="hitData"></param>
    /// <returns></returns>
    public bool VerifyHit(HitData hitData);

    /// <summary>
    /// The final destination after a <paramref name="hitData"/> has been validated. Called at the end of validation
    /// </summary>
    /// <param name="hitData"></param>
    public void HitResponse(HitData hitData);
}

public interface IParticleHitResponder : IHitResponder
{
    public ParticleSystem.Particle[] ParticleResponse(ParticleSystem.Particle[] particles, int particleId, HitData hitData);
}

public interface IHitDetector
{
    public IHitResponder HitResponder { get; set; }
    /// <summary>
    /// Casts into the scene to search for Hurtboxes, Creates & validates a HitData object, 
    /// and passes it to the Hurtboxes
    /// </summary>
    public void Cast();
}

public interface IParticleHitDetector : IHitDetector
{
    public IParticleHitResponder ParticleResponder { get; set; }
    public void Emit();
}

public interface IHurtResponder
{
    public bool VerifyHurt(HitData hitData);
    public void HurtResponse(HitData hitData);
}

public interface IHurtbox
{
    public bool Active { get;set; }
    public GameObject Owner { get; }
    public Transform Transform { get; }
    public IHurtResponder HurtResponder { get; set; }
    public bool CheckHit(HitData hitData);
    public HurtboxType Type { get; set; }
}
