using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class DamageDirectionIndicator : MonoBehaviour
{
    [HideInInspector]
    public Vector3 shotInitialPosition;
    RectTransform _rectTransform;
    Color _matColor;
    Material _material;
    const float LifeTime = 2;
    float _actualLife;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();

        UpdateRotation();

        StartCoroutine(DestroyMe());
        _material = GetComponent<Image>().material;
        _matColor = _material.color;
    }

    void Update()
    {
        _actualLife += Time.deltaTime;

        UpdateRotation();

        UpdateOpacity();

        // _rectTransform.localScale = new(1 - _actualLife / LifeTime, 1 - _actualLife / LifeTime, 1 - _actualLife / LifeTime);
    }

    void UpdateRotation()
    {
        var tr = Player.transform;
        var direction = tr.position - shotInitialPosition;
        _rectTransform.eulerAngles = new(0, 0, tr.eulerAngles.y + Mathf.Rad2Deg * Mathf.Atan2(direction.z, direction.x));
    }

    void UpdateOpacity()
    {
        _material.SetColor("_Color", new(1, 1, 1, 1 - _actualLife / LifeTime));
    }

    IEnumerator DestroyMe()
    {
        yield return new WaitForSeconds(LifeTime);
        
        Destroy(gameObject);
    }

}
