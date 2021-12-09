using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomColorShader : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Material mat;
    
    private int mainColorPropertyShaderID;
    private int glowColorPropertyShaderID;

    private void Start()
    {
        mainColorPropertyShaderID = Shader.PropertyToID("_Color");
        glowColorPropertyShaderID = Shader.PropertyToID("_GlowColor");
    }

    private void OnEnable()
    {
        Color changeColor = GenerateColor();

        _renderer.color = changeColor;
        mat.SetColor(mainColorPropertyShaderID, changeColor);
        // mat.SetColor(glowColorPropertyShaderID, changeColor);
    }

    private Color GenerateColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}