using System.Collections;
using UnityEngine;
using static GameController;

public class Radar : MonoBehaviour
{
    Texture2D _texture;
    Texture2D _textureBackground;
    public static float RadarToSceneRatio = 1;
    public int size = 128;
    int _halfSize;

    void Start()
    {
        _textureBackground = new (size, size);
        _texture = new (size, size);
        _halfSize = size / 2;

        GetComponent<Renderer>().material.mainTexture = _texture;

        FilledCircle(_halfSize, _halfSize, 256);

        _textureBackground.Apply();

        StartCoroutine(RadarUpdateLoop());
    }

    void FilledCircle(float cx, float cy, float radius, float borderWidth = 4)  // border acts as inner border
    {
        Color color;

        for (int y = 0; y < _textureBackground.height; ++y)
            for (int x = 0; x < _textureBackground.width; ++x)
            {
                var d = Mathf.Sqrt(Mathf.Pow(cx - x, 2) + Mathf.Pow(cy - y, 2));

                if (d < radius - borderWidth)
                    color = Color.black;
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
            yield return new WaitForSeconds(.4f);
            RadarUpdate();
        }
    }

    void RadarUpdate()
    {
        Graphics.CopyTexture(_textureBackground, _texture);
        Vector3 pos;

        foreach (Tank enemy in GC.enemies)
        {
            pos = enemy.transform.position;
            print("updating: " + pos);
            _texture.SetPixel((int)pos.x + _halfSize, (int)pos.z + _halfSize, Color.green);

        }
        _texture.Apply();
        
    }

}
