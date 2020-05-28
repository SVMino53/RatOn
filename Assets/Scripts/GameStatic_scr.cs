using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameStatic_scr
{
    static public string dir = "SaveData";
    static public string path = "SaveData/save.txt";

    static public byte level = 0;
    static public uint score = 0;
    static public uint money = 0;
    static public uint highScore = 0;


    static public void Save()
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        StreamWriter sw = File.CreateText(path);

        sw.WriteLine(level);
        sw.WriteLine(score);
        sw.WriteLine(money);
        sw.WriteLine(highScore);
        sw.Close();
    }

    static public void Load()
    {
        if (!File.Exists(path))
        {
            return;
        }

        StreamReader sr = File.OpenText(path);

        level = byte.Parse(sr.ReadLine());
        score = uint.Parse(sr.ReadLine());
        money = uint.Parse(sr.ReadLine());
        highScore = uint.Parse(sr.ReadLine());
        sr.Close();
    }
}
