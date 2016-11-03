using UnityEngine;
using System.Collections;

public class WeaponGraphics : MonoBehaviour {

    public ParticleSystem effect;
    public GameObject hitEffectPrefab;

    public WeaponGraphics(WeaponGraphics g)
    {
        effect = g.effect;
        hitEffectPrefab = g.hitEffectPrefab;
    }
}
