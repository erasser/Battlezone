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
        var tr = transform;
        var pos = targetPortal.transform.position;
        tr.position = new Vector3(pos.x, tr.position.y, pos.z) - targetPortal.transform.forward * (DistanceToTeleport + 1);

        Sm.PlayClip(Sm.teleportEffect, gameObject);
    }

}
