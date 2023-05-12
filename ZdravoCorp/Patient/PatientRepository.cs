using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ZdravoCorp;

public class PatientRepository
{
    public List<Patient> Patients;
    private string FilePath = "../../../patients.csv";

    public PatientRepository()
    {
        Patients = new List<Patient>();
        ReadPatients();
    }

    public void ReadPatients()
    {
        if (!File.Exists(FilePath)) throw new FileNotFoundException(FilePath);
        StreamReader reader = new StreamReader(FilePath);

        string line;
        Patient temp;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();
            System.Console.WriteLine(line);
            temp = new Patient();
            temp.DecodeFromCSV(line);
            Patients.Add(temp);
        }
        reader.Close();
    }

    public void WritePatients()
    {
        if (!File.Exists(FilePath)) throw new FileNotFoundException(FilePath);
        StreamWriter writer = new StreamWriter(FilePath);
        
        string fileContent = "";
        foreach (Patient patient in Patients)
        {
            fileContent += patient.EncodeToCSV() + "\n";
        }
        fileContent = fileContent.Trim();
        writer.Write(fileContent);
        writer.Close();
    }

    // return true if user exists
    public bool RemovePatient(string username)
    {
        foreach (Patient patient in Patients)
        {
            if (patient.Username.Equals(username))
            {
                Patients.Remove(patient);
                break;
            }
        }

        WritePatients();
        return true;
    }

    public void AddPatient(Patient patient)
    {
        Patients.Add(patient);
        WritePatients();
    }

    public void UpdatePatients()
    {
        WritePatients();
    }

    public bool UsernameExists(string username)
    {
        foreach (Patient patient in Patients)
        {
            if (patient.Username.Equals(username)) return true;
        }
        return false;
    }

    public Patient returnPatient(string patUser)
    {
        foreach(Patient patient in Patients)
        {
            if (patient.Username.Equals(patUser))
            {
                return patient;
            }
        }
        return null;  
    }
}