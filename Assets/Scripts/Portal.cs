using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class Portal : MonoBehaviour
{
    public static List<Portal> Portals = new();
    // public int listIndex;
    Transform _cameraTransform;
    float _maxTriggerDistance;
    public Portal otherPortal;

    void Awake()
    {
        // listIndex = Portals.Count;
        Portals.Add(this);

        otherPortal = transform.GetComponentInChildren<PortalCamera>().portal.GetComponent<Portal>();  // Jo to other portal, ale přiřadil jsem jim ty kamery prohozeně
    }

    void Start()
    {
        /*_cameraTransform = transform.Find("Camera");
        _maxTriggerDistance = GetComponent<SphereCollider>().radius;*/
    }

    // public Portal GetOtherPortal()
    // {
    //     return Portals[listIndex == ];
    // }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, Player.transform.eulerAngles.y + 180, 0);

        // transform.LookAt(Player.transform);
        // transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);

        /*
        // _cameraTransform.localPosition = new(0, 0, (anotherPortal.position - Player.transform.position).magnitude + 4);
        var d = Player.transform.position - anotherPortal.position;

        // _cameraTransform.localPosition = new(d.x, 0, d.z);
        _cameraTransform.position = transform.position + new Vector3(d.x, 0, d.z);
    */
    }
}
