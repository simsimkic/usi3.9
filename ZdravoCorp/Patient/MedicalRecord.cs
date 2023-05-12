using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZdravoCorp;

public class MedicalRecord
{
    public int Height { get; set; }
    public float Weight { get; set; }
    public List<string> PastConditions { get; set; }
    public List<string> Allergies { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }

    public MedicalRecord(int height, float weight, List<string> pastConditions, List<string> allergies, string gender, int age)
    {
        Height = height;
        Weight = weight;
        PastConditions = pastConditions;
        Allergies = allergies;
        Gender = gender;
        Age = age;
    }

    public MedicalRecord()
    {
        PastConditions = new List<string>();
        Allergies = new List<string>();
    }

    public string EncodeToCSV()
    {
        StringBuilder res = new StringBuilder();
        res.Append(Height);
        res.Append(',');
        res.Append(Weight);
        res.Append(',');
        res.Append(string.Join('|', PastConditions));
        res.Append(',');
        res.Append(string.Join('|', Allergies));
        res.Append(',');
        res.Append(Gender);
        res.Append(",");
        res.Append(Age);

        return res.ToString();
    }

    public void DecodeFromCSV(string[] parts)
    {
        Height = int.Parse(parts[0]);
        Weight = float.Parse(parts[1]);
        PastConditions = parts[2].Split('|').ToList();
        Allergies = parts[3].Split('|').ToList();
        Gender = parts[4];
        Age = int.Parse(parts[5]);
    }
}