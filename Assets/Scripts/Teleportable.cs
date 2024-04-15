using UnityEngine;
using static Portal;
using static SoundManager;

public class Teleportable : MonoBehaviour
{
    public static float DistanceToTeleport = 4;
    public static float SqrDistanceToTeleport;

    void Start()
    {
        SqrDistanceToTeleport = Mathf.Pow(DistanceToTeleport, 2);
    }

    void Update()
    {
        foreach (Portal portal in Portals)
            if (GetXZDistanceToPortal(portal.transform.position) < SqrDistanceToTeleport)
                TeleportObjectTo(portal.otherPortal);
    }

    float GetXZDistanceToPortal(Vector3 portalPosition)
    {
        return Mathf.Pow(portalPosition.x - transform.position.x, 2) + Mathf.Pow(portalPosition.z - transform.position.z, 2);
    }

    void TeleportObjectTo(Portal targetPortal)
    {
        // var dummy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // dummy.transform.localScale = Vector3.left * .3f;
        // dummy.transform.position = transform.position;

        var tr = transform;
        var pos = targetPortal.transform.position;
        tr.position = new Vector3(pos.x, tr.position.y, pos.z) - targetPortal.transform.forward * (DistanceToTeleport + 1);
        var offset = tr.position - targetPortal.otherPortal.transform.position;  // targetPortal.otherPortal = this portal LOL
        // tr.position = pos + offset - targetPortal.transform.forward * (DistanceToTeleport + 1);
        tr.Translate(new(- offset.z, 0, - offset.x));

        // TODO...
        // Time.timeScale = 0;
        
        Sm.PlayClip(Sm.teleportEffect, gameObject);
    }

}
