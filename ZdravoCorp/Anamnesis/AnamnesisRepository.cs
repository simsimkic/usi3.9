using System;
using System.Collections.Generic;
using System.IO;

namespace ZdravoCorp;

public class AnamnesisRepository
{
    
    public List<Anamnesis> Anamneses;
    private string FilePath = "../../../anamneses.csv";

    public AnamnesisRepository()
    {
        Anamneses = new List<Anamnesis>();
        ReadAnamneses();
    }

    public Anamnesis GetAnamnesisByPatientUsername(String username)
    {
        foreach (Anamnesis anamnesis in Anamneses)
        {
            if (anamnesis.patientUsername.Equals(username)) return anamnesis;
        }

        return null;
    }

    public void ReadAnamneses()
    {
        if (!File.Exists(FilePath)) throw new FileNotFoundException(FilePath);
        StreamReader reader = new StreamReader(FilePath);

        string line;
        Anamnesis temp;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            System.Console.WriteLine(line);
            temp = new Anamnesis();
            temp.DecodeFromCSV(line);
            Anamneses.Add(temp);
        }
        reader.Close();
    }

    private void WriteAnamneses()
    {
        if (!File.Exists(FilePath)) throw new FileNotFoundException(FilePath);
        StreamWriter writer = File.CreateText(FilePath);
        
        string line;
        Anamnesis temp;
        foreach (Anamnesis anamnesis in Anamneses)
        {
            writer.Write(anamnesis.EncodeToCSV());
            writer.Write("\n");
        }
        writer.Close();
    }

    // return true if user exists
    public bool RemoveAnamnesis(string username)
    {
        foreach (Anamnesis anamnesis in Anamneses)
        {
            if (anamnesis.patientUsername.Equals(username))
            {
                Anamneses.Remove(anamnesis);
                break;
            }
        }

        WriteAnamneses();
        return true;
    }

    public void AddAnamnesis(Anamnesis anamnesis)
    {
        Anamneses.Add(anamnesis);
        WriteAnamneses();
    }

    public void UpdateAnamnesis()
    {
        WriteAnamneses();
    }

    public bool AnamnesisExists(string username)
    {
        foreach (Anamnesis anamnesis in Anamneses)
        {
            if (anamnesis.patientUsername.Equals(username)) return true;
        }
        return false;
    }}