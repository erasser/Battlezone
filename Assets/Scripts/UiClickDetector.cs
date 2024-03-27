using UnityEngine;
using UnityEngine.EventSystems;

public class UiClickDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // public bool buttonDown;
    // public bool buttonUp;
    Tank _player;

    void Start()
    {
        _player = GameObject.Find("PLAYER").GetComponent<Tank>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _player.isShooting = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _player.isShooting = false;
    }
}