using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string code = @"function Start()
                        end
                        function OnReleased()
                        end
                        function OnPressed()
                              if (_G['smoke_power_color'].interactable.GetColor()) == '#ff0000' then
                                _G['smoke_power_color'].interactable.SetColor('#66ff99');
                              end
                              if (_G['smoke_power_color'].interactable.GetColor()) == '#ff0000' then
                                _G[''smoke_power_color'].interactable.SetColor('#66ff99');
                              end
                        end";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
