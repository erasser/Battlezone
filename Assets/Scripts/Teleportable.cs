using UnityEngine;
using static GameController;
using static Portal;

public class Teleportable : MonoBehaviour
{
    void Update()
    {
        // TODO: Je potřeba kontrolovat, když se vzdálenost začne zvětšovat.
        
        foreach (Portal portal in Portals)
            if ((portal.transform.position - transform.position).sqrMagnitude < 16)
                TeleportObjectTo(portal.otherPortal);
    }

    void TeleportObjectTo(Portal targetPortal)
    {
        var tr = transform;
        var pos = targetPortal.transform.position;
        tr.position = new Vector3(pos.x, tr.position.y, pos.z) + tr.forward * 5;
        
        Gc.audioSource.Play();


    }

}
