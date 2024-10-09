using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject InfoView;


    public void ToggleInfoView()
    {
      
        InfoView.SetActive(!InfoView.activeSelf);
    }
}
