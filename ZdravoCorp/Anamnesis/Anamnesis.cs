using System;
using System.Text;

namespace ZdravoCorp;

public class Anamnesis
{
    public String report { get; set; }
    public TimeOnly startTime { get; set; }
    public DateOnly date { get; set; }
    public String patientUsername { get; set; }

    public Anamnesis(String report, String username, TimeOnly startTime, DateOnly date) {
        this.report = report;
        this.startTime = startTime;
        this.date = date;
        this.patientUsername = username;
    }

    public Anamnesis() { }

    public String EncodeToCSV()
    {
        StringBuilder res = new StringBuilder();
        res.Append(this.patientUsername);
        res.Append(',');
        res.Append(this.report);
        res.Append(',');
        res.Append(this.startTime);
        res.Append(',');
        res.Append(this.date);
        return res.ToString();
    }

    public void DecodeFromCSV(String anamnesisRecord)
    {
        string[] parts = anamnesisRecord.Split(',');
        this.patientUsername = parts[0];
        this.report = parts[1];
        this.startTime = TimeOnly.Parse(parts[2]);
        this.date = DateOnly.Parse(parts[3]);
    }
}