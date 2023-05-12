using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace ZdravoCorp;

public class Patient : User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Blocked { get; set; }
    public MedicalRecord MedicalRecord { get; set; }

    public Patient(string username, string password, string firstName, string lastName, bool blocked, MedicalRecord medicalRecord) : base(username, password, Role.PATIENT)
    {
        FirstName = firstName;
        LastName = lastName;
        Blocked = blocked;
        MedicalRecord = medicalRecord;
    }

    public Patient(): base("", "", Role.PATIENT)
    {
        MedicalRecord = new MedicalRecord();
    }

    public string EncodeToCSV()
    {
        StringBuilder res = new StringBuilder();
        res.Append(base.Username);
        res.Append(',');
        res.Append(base.Password);
        res.Append(',');
        res.Append(FirstName);
        res.Append(',');
        res.Append(LastName);
        res.Append(',');
        res.Append(Blocked);
        res.Append(',');
        res.Append(MedicalRecord.EncodeToCSV());
        return res.ToString();
    }

    public void DecodeFromCSV(string patient)
    {
        string[] parts = patient.Split(',');
        Username = parts[0];
        Password = parts[1];
        FirstName = parts[2];
        LastName = parts[3];
        Blocked = bool.Parse(parts[4]);
        MedicalRecord = new MedicalRecord();
        MedicalRecord.DecodeFromCSV(parts[5..]);
    }

    public override string ToString()
    {
        return this.FirstName + " " + this.LastName;
    }

   
}