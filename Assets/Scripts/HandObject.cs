using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandObject : MonoBehaviour
{
    public Vector3 originalPos;
    public Quaternion originalRot;
    public bool purchased = false;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public int position;
    public int cost;
}

