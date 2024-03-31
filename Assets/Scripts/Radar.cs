using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    Texture2D _texture;
    Texture2D _textureBackground;
    public static float SceneToRadarRatio = 1;
    public static float RadarToSceneRatio = 1;
    public float groundSize = 1000;
    public int textureSize = 128;
    Vector2 _halfSize;
    RectTransform _rectTransform;
    float _radius;
    float _radiusWithoutBorder;

    void Start()
    {
        _textureBackground = new (textureSize, textureSize);
        _texture = new (textureSize, textureSize);
        _halfSize = new(textureSize / 2f, textureSize / 2f);
        SceneToRadarRatio = groundSize / textureSize;
        RadarToSceneRatio = textureSize / groundSize;
        _rectTransform = GetComponent<RectTransform>();
        _radius = textureSize / 2f;
        float borderWidth = 1;
        _radiusWithoutBorder = _radius - borderWidth;

        GetComponent<Image>().material.mainTexture = _texture;

        FilledCircle(_halfSize, _radius, borderWidth);
        GenerateTerrain();
        _textureBackground.Apply();

        StartCoroutine(RadarUpdateLoop());
    }

    void FilledCircle(Vector2 center, float radius, float borderWidth = 4)  // border acts as inner border
    {
        Color color;

        for (int y = 0; y < _textureBackground.height; ++y)
            for (int x = 0; x < _textureBackground.width; ++x)
            {
                var d = Mathf.Sqrt(Mathf.Pow(center.x - x, 2) + Mathf.Pow(center.y - y, 2));

                if (d < radius - borderWidth)
                    color = new Color(0, 0, 0, 1f);
                else if (IsInRange(d, radius - borderWidth, radius))
                    color = Color.green;
                else
                    color = new Color(0, 0, 0, 0);

                _textureBackground.SetPixel(x, y, color);
            }

        bool IsInRange(float number, float rangeStart, float rangeFinish)
        {
            return number >= rangeStart && number <= rangeFinish;
        }
    }

    IEnumerator RadarUpdateLoop()
    {
        for (;;)
        {
            yield return new WaitForSeconds(.3f);
            RadarUpdate();
        }
    }

    void RadarUpdate()
    {
        Graphics.CopyTexture(_textureBackground, _texture);
        Vector3 pos;

        foreach (Tank enemy in GameController.GC.enemies)
        {
            pos = enemy.transform.position;
            DrawPoint(_texture, ScenePointToRadarPoint(pos), Color.red);
        }

        DrawPoint(_texture, ScenePointToRadarPoint(GameController.GC.player.transform.position), Color.green);

        _texture.Apply();
    }

    void Update()
    {
        _rectTransform.eulerAngles = new(0, 0, GameController.GC.player.transform.eulerAngles.y);        
    }

    Vector2 ScenePointToRadarPoint(Vector3 point)
    {
        return SceneToRadarRatio * new Vector2(point.x, point.z) + _halfSize;
    }

    Vector3 RadarToScenePoint(Vector2 point, float y = 0)
    {
        point -= _halfSize;
        return RadarToSceneRatio * (new Vector3(point.x, y, point.y));
    }

    void SetPixel(Texture2D texture, Vector2 pixel, Color color, bool checkRadius = false)
    {
        if (checkRadius && !IsPointInRadius(pixel))
            return;

        texture.SetPixel((int)pixel.x, (int)pixel.y, color);  // todo: texture as argument
    }

    void SetPixel(Texture2D texture, float x, float y, Color color)
    {
        texture.SetPixel((int)x, (int)y, color);
    }

    void DrawPoint(Texture2D texture, Vector2 point, Color color)
    {
        if (!IsPointInRadius(point))
            return;

        SetPixel(texture, point.x - 1, point.y - 1, color);
        SetPixel(texture, point.x - 1, point.y, color);
        SetPixel(texture, point.x, point.y - 1, color);
        SetPixel(texture, point.x, point.y, color);
    }

    bool IsPointInRadius(Vector2 point)
    {
        return Mathf.Pow(point.x - _halfSize.x, 2) + Mathf.Pow(point.y - _halfSize.y, 2) < Mathf.Pow(_radiusWithoutBorder, 2);
    }

    void GenerateTerrain()
    {
        RaycastHit hit;

        for (float y = 0; y < _textureBackground.height; y += .5f)
            for (float x = 0; x < _textureBackground.width; x += .5f)
            {
                var origin = RadarToScenePoint(new(x, y), 20);

                if (!Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, 1 << GameController.GC.shootableEnvironmentLayer)
                    || hit.point.y < .1f)
                    continue;

                // SetPixel(_textureBackground, ScenePointToRadarPoint(hit.point), Color.grey);
                float colorValue = (hit.point.y * 14 + 5) / 255;
                SetPixel(_textureBackground, ScenePointToRadarPoint(hit.point), new (colorValue, colorValue + .1f, colorValue), true);
            }
    }

}
