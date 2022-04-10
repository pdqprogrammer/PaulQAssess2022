using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TheaterData
{
    public string name;
    public string openTime;
    public Halls[] halls;
}

[Serializable]
public class Halls
{
    public string name;
    public int rows;
    public int seats;
}