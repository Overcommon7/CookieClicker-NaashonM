using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public static class Utils
{
    public static string[] GetFile(string filename)
    {
        var fullpath = Path.Combine(Application.persistentDataPath, filename);
        if (!File.Exists(fullpath)) return null;
        return File.ReadAllLines(fullpath);
    }

    public static Dictionary<string, string[]> GetFileAsDictionary(string filename)
    {
        Dictionary<string, string[]> dict = new Dictionary<string, string[]>();
        var fullpath = Path.Combine(Application.persistentDataPath, filename);
        if (!File.Exists(fullpath)) return null; 
        char[] delimiters = { ' ', ',', '\t' };
        using (StreamReader sr = new StreamReader(fullpath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(delimiters);
                if (words.Length <= 1) continue;
                dict.Add(words[0], words);
            }
        }
        return dict;       
    }

    public static bool WriteToFile(ref List<string> fileContents, string filename)
    {
        var fullpath = filename;
        if (!fullpath.Contains(Application.persistentDataPath))
            fullpath = Path.Combine(Application.persistentDataPath, filename);
        if (File.Exists(fullpath)) File.Delete(fullpath);
        var fs = File.Create(fullpath);
        fs.Close();
        if (fileContents == null || fileContents.Count == 0) return false;
        File.WriteAllLines(fullpath, fileContents);
        return true;      
    }

    public static string GetPath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, filename);
    }
}
