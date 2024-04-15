using System.Collections;
using UnityEngine;
using static GameController;

public class DamageDirectionIndicator : MonoBehaviour
{
    [HideInInspector]
    public Vector3 shotInitialPosition;
    RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        UpdateRotation();

        StartCoroutine(DestroyMe());
    }

    void Update()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
        var tr = Player.transform;
        var direction = tr.position - shotInitialPosition;
        _rectTransform.eulerAngles = new(0, 0, tr.eulerAngles.y + Mathf.Rad2Deg * Mathf.Atan2(direction.z, direction.x));
    }

    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(2);
        
        Destroy(gameObject);
    }

}
