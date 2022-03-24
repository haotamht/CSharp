
public class LocalVietNam
{
    public Datum[] data { get; set; }
}

public class Datum
{
    public string id { get; set; }
    public string name { get; set; }
    public District[] districts { get; set; }
    public string code { get; set; }
}

public class District
{
    public string id { get; set; }
    public string name { get; set; }
    public Ward[] wards { get; set; }
    public Street[] streets { get; set; }
    public Project[] projects { get; set; }
}

public class Ward
{
    public string id { get; set; }
    public string name { get; set; }
    public string prefix { get; set; }
}

public class Street
{
    public string id { get; set; }
    public string name { get; set; }
    public string prefix { get; set; }
}

public class Project
{
    public string id { get; set; }
    public string name { get; set; }
    public string lat { get; set; }
    public string lng { get; set; }
}
