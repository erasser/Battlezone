using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static GameController;

public class Radar : MonoBehaviour
{
    Texture2D _texture;
    Texture2D _textureBackground;
    public static float SceneToRadarRatio = 1;
    public static float RadarToSceneRatio = 1;
    // public float groundSize = 1000;
    public int textureSize = 128;
    Vector2 _halfSize;
    RectTransform _rectTransform;
    float _radius;
    float _radiusWithoutBorder;
    float _zoom = 1;

    void Start()
    {
        _textureBackground = new (textureSize, textureSize);
        _texture = new (textureSize, textureSize);
        _halfSize = new Vector2 (textureSize, textureSize) / 2f;
        SceneToRadarRatio = Gc.groundSize / textureSize;
        RadarToSceneRatio = textureSize / Gc.groundSize;
        _rectTransform = GetComponent<RectTransform>();
        _radius = textureSize / 2f;
        float borderWidth = 1;
        _radiusWithoutBorder = _radius - borderWidth;

        GetComponent<Image>().material.mainTexture = _texture;

        FilledCircle(_halfSize, _radius, borderWidth);
        // DrawPlayerSightLines(_textureBackground);
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
                    color = new Color(0, 0, 0, .9f);
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
            yield return new WaitForSeconds(.1f);
            RadarUpdate();
        }
    }

    void RadarUpdate()
    {
        Graphics.CopyTexture(_textureBackground, _texture);
        Vector3 pos;

        foreach (Tank enemy in Enemies)
        {
            pos = enemy.transform.position;
            DrawPoint(_texture, ScenePointToRadarPoint(pos), Color.red);
        }

        DrawPoint(_texture, ScenePointToRadarPoint(Player.transform.position), Color.green);

        _texture.Apply();
    }

    void Update()
    {
        _rectTransform.eulerAngles = new(0, 0, Player.transform.eulerAngles.y);        
    }

    Vector2 ScenePointToRadarPoint(Vector3 point)
    {
        point -= Player.transform.position;
        return RadarToSceneRatio * _zoom * new Vector2(point.x, point.z) + _halfSize;
    }

    Vector3 RadarToScenePoint(Vector2 point, float y = 0)
    {
        point -= _halfSize;
        return SceneToRadarRatio / _zoom * new Vector3(point.x, y, point.y) + Player.transform.position;
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

    void GenerateTerrain()  // TODO: To bude chtít asi další texturu...
    {
        RaycastHit hit;

        for (float y = 0; y < _textureBackground.height; y += .5f)
            for (float x = 0; x < _textureBackground.width; x += .5f)
            {
                var origin = RadarToScenePoint(new(x, y), 20);

                if (!Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, 1 << GameController.Gc.shootableEnvironmentLayer)
                    || hit.point.y < .1f)
                    continue;

                // SetPixel(_textureBackground, ScenePointToRadarPoint(hit.point), Color.grey);
                float colorValue = (hit.point.y * 14 + 5) / 255;
                SetPixel(_textureBackground, ScenePointToRadarPoint(hit.point), new (colorValue, colorValue + .1f, colorValue), true);
            }
    }

    void DrawPlayerSightLines(Texture2D texture)  // TODO
    {
        Color color = new(0, .5f, 0);
        var o = _radius / Mathf.Sqrt(2);
        DrawLine(texture, _halfSize, new(_halfSize.x - o, _halfSize.y + o), color);
        DrawLine(texture, _halfSize, new(_halfSize.x + o, _halfSize.y + o), color);
    }

    void DrawLine(Texture2D texture, Vector2 p1, Vector2 p2, Color color)
    {
        Vector2 t = p1;
        float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
        float ctr = 0;

        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
        {
            t = Vector2.Lerp(p1, p2, ctr);
            ctr += frac;
            texture.SetPixel((int)t.x, (int)t.y, color);
        }
    }


}
