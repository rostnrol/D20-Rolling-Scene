using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour //честно говоря, это скрипт смены курсора, который разработчики ассета сами положили в набор) я только подправил пару моментов + форматирование
{
    [SerializeField] private Image _cursorImage;

    private Texture2D _cursorTexture;

    private void Awake()
    {
        if (_cursorImage != null && _cursorImage.sprite != null)
        {
            _cursorTexture = new Texture2D((int)_cursorImage.sprite.textureRect.width, (int)_cursorImage.sprite.textureRect.height, TextureFormat.RGBA32, false);
           
            for (int y = 0; y < _cursorTexture.height; y++)
            {
                for (int x = 0; x < _cursorTexture.width; x++)
                {
                    _cursorTexture.SetPixel(x, y, _cursorImage.sprite.texture.GetPixel((int)_cursorImage.sprite.textureRect.x + x, (int)_cursorImage.sprite.textureRect.y + y));
                }
            }

            _cursorTexture.Apply();
        }
    }

    private void Start()
    {
        if (_cursorTexture != null)
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}