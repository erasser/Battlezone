using UnityEngine;
using static Portal;
using static SoundManager;

public class Teleportable : MonoBehaviour
{
    public static float DistanceToTeleport = 4;
    public static float SqrDistanceToTeleport;
    Vector3 _lastPosition;

    void Start()
    {
        SqrDistanceToTeleport = Mathf.Pow(DistanceToTeleport, 2);
        _lastPosition = transform.position;
    }

    void Update()
    {
        foreach (Portal portal in Portals)
            if (GetXZDistanceToPortal(portal.transform.position) < SqrDistanceToTeleport)
                TeleportObjectTo(portal.otherPortal);

        _lastPosition = transform.position;
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

        // RaycastHit hit;
        // var result = Physics.Raycast(transform.position, transform.position - _lastPosition, out hit);  // TODO: jen portaly
        // Vector3 offset;
        // if (!result)
        //     offset = Vector3.zero;
        // else
        //     offset = hit.point - targetPortal.otherPortal.transform.position;

        
        var tr = transform;
        var pos = targetPortal.transform.position;
        tr.position = new Vector3(pos.x, tr.position.y, pos.z) - targetPortal.transform.forward * (DistanceToTeleport + 1);
        // tr.position = offset + pos - targetPortal.transform.forward * (DistanceToTeleport + 1);
        // tr.position = new Vector3(tr.position.x, tr.position.y, tr.position.z);
// TODO
        // tr.Translate(offset);

        // tr.position = pos;
        // tr.Translate(offset);

        // Time.timeScale = 0;
        

        Sm.PlayClip(Sm.teleportEffect, gameObject);
    }

}
