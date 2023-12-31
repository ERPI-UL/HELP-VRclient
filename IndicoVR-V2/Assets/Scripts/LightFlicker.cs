using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Material mat;
    //These variables are being used EVERY frame. Made some variables public so i can use them to create SCRIPTED sequences.
    public int delay = 10;
    [SerializeField] float StartIntensity;
    public float AddIntensity = 0;
    //These variables are being used BEFORE first frame. Only once.
    public string CustomAnim;
    public int AnimationPreset;
    //Not public variables.
    int i;
    float[] anim;
    int fps = 0;

    public Light lighting;
    //Bool to update light animation.
    public bool Reinitialize = false;
    //Some preparations. Also starts animation generation.
    void Start()
    {
        StartIntensity = lighting.intensity;
        GenerateAnimation();
    }
    // This function animates Light. Why did i choose FixedUpdate? To make animation identical on every device/pc.
    void FixedUpdate()
    {
        //Used to update light animation.
        if (Reinitialize)
            GenerateAnimation();
        //Restarts frame counter.
        if (fps > delay)
            fps = 0;
        //Updates light intensity after delay. I recommend using 2-3 for SMOOTH animation. 5-10 for ABRUPT animation.
        if (fps % delay == 0)
        {
            lighting.intensity = ((anim[i] * (StartIntensity + AddIntensity)));
            i++;
            if (i == anim.Length)
                i = 0;
        }
        fps++;
        mat.SetFloat("emission", lighting.intensity);
        mat.SetColor("Color", lighting.color);
    }
    //This function reads CustomAnim value, if empty checks preset value and fills CustomAnim with symbols. Then starts Alphabet() function.
    void GenerateAnimation()
    {
        i = 0;
        fps = 0;
        if (CustomAnim == "")
        {
            switch (AnimationPreset)
            {
                //Broken Light
                case -1:
                    CustomAnim = "a";
                    anim = new float[CustomAnim.Length];
                    break;
                //Normal Light
                case 0:
                    CustomAnim = "m";
                    anim = new float[CustomAnim.Length];
                    break;
                //Flicker 1
                case 1:
                    CustomAnim = "mmnmmommommnonmmonqnmmo";
                    anim = new float[CustomAnim.Length];
                    break;
                //Slow strong pulse
                case 2:
                    CustomAnim = "abcdefghijklmnopqrstuvwxyzyxwvutsrqponmlkjihgfedcba";
                    anim = new float[CustomAnim.Length];
                    break;
                //Candle 1
                case 3:
                    CustomAnim = "mmmmmaaaaammmmmaaaaaabcdefgabcdefg";
                    anim = new float[CustomAnim.Length];
                    break;
                //Fast Strobe
                case 4:
                    CustomAnim = "mamamamamama";
                    anim = new float[CustomAnim.Length];
                    break;
                //Gentle Pulse
                case 5:
                    CustomAnim = "jklmnopqrstuvwxyzyxwvutsrqponmlkj";
                    anim = new float[CustomAnim.Length];
                    break;
                //Flicker 2
                case 6:
                    CustomAnim = "nmonqnmomnmomomno";
                    anim = new float[CustomAnim.Length];
                    break;
                //Candle 2
                case 7:
                    CustomAnim = "mmmaaaabcdefgmmmmaaaammmaamm";
                    anim = new float[CustomAnim.Length];
                    break;
                //Candle 3
                case 8:
                    CustomAnim = "mmmaaammmaaammmabcdefaaaammmmabcdefmmmaaaa";
                    anim = new float[CustomAnim.Length];
                    break;
                //Slow strobe 4
                case 9:
                    CustomAnim = "aaaaaaaazzzzzzzz";
                    anim = new float[CustomAnim.Length];
                    break;
                //Fluorescent Flicker
                case 10:
                    CustomAnim = "mmamammmmammamamaaamammma";
                    anim = new float[CustomAnim.Length];
                    break;
                //Slow pulse not fade to black
                case 11:
                    CustomAnim = "abcdefghijklmnopqrrqponmlkjihgfedcba";
                    anim = new float[CustomAnim.Length];
                    break;
                    //You can add your presets here. Here's the template.
                    //case 12:
                    //  CustomAnim = "WriteSomethingHere"; 
                    //  anim = new float[CustomAnim.Length];
                    //  break;
            }
        }
        else
        {
            anim = new float[CustomAnim.Length];
        }
        foreach (int i in anim)
        {
            Alphabet();
        }
        i = 0;
        Reinitialize = false;
        CustomAnim = "";        //Dunno why did i even add this line...
    }
    // Translates symbols to floats and puts them into array.
    void Alphabet()
    {
        //Not elegant, i know. But atleast i didn't had to create any files. :D
        switch (CustomAnim[i])
        {
            case 'a':
                anim[i] = 0f;
                break;
            case 'b':
                anim[i] = 0.083f;
                break;
            case 'c':
                anim[i] = 0.167f;
                break;
            case 'd':
                anim[i] = 0.25f;
                break;
            case 'e':
                anim[i] = 0.33f;
                break;
            case 'f':
                anim[i] = 0.417f;
                break;
            case 'g':
                anim[i] = 0.5f;
                break;
            case 'h':
                anim[i] = 0.583f;
                break;
            case 'i':
                anim[i] = 0.667f;
                break;
            case 'j':
                anim[i] = 0.75f;
                break;
            case 'k':
                anim[i] = 0.833f;
                break;
            case 'l':
                anim[i] = 0.917f;
                break;
            case 'm':
                anim[i] = 1f;
                break;
            case 'n':
                anim[i] = 1.083f;
                break;
            case 'o':
                anim[i] = 1.167f;
                break;
            case 'p':
                anim[i] = 1.25f;
                break;
            case 'q':
                anim[i] = 1.33f;
                break;
            case 'r':
                anim[i] = 1.417f;
                break;
            case 's':
                anim[i] = 1.5f;
                break;
            case 't':
                anim[i] = 1.583f;
                break;
            case 'u':
                anim[i] = 1.667f;
                break;
            case 'v':
                anim[i] = 1.75f;
                break;
            case 'w':
                anim[i] = 1.833f;
                break;
            case 'x':
                anim[i] = 1.917f;
                break;
            case 'y':
                anim[i] = 2f;
                break;
            case 'z':
                anim[i] = 2.083f;
                break;
            default: break;
        }
        //Counter.
        i++;
    }
}
