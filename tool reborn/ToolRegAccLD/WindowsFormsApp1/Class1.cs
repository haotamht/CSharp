//Get Phone
#region
public class PhoneOTPSIM
{
    public int status_code { get; set; }
    public bool success { get; set; }
    public string message { get; set; }
    public DataPhone data { get; set; }
}

public class DataPhone
{
    public string phone_number { get; set; }
    public int network { get; set; }
    public string session { get; set; }
}
#endregion
//Get OTP SMS
#region
public class SMS
{
    public int status_code { get; set; }
    public bool success { get; set; }
    public string message { get; set; }
    public DataSMS data { get; set; }
}

public class DataSMS
{
    public string id { get; set; }
    public string phone_number { get; set; }
    public int service_id { get; set; }
    public string service_name { get; set; }
    public int status { get; set; }
    public MessageSMS[] messages { get; set; }
    public string created_at { get; set; }
    public string done_at { get; set; }
}

public class MessageSMS
{
    public string sms_from { get; set; }
    public string sms_content { get; set; }
    public string otp { get; set; }
    public bool is_audio { get; set; }
    public string created_at { get; set; }
}
#endregion

//Get OTP SMSVOICE
#region
public class VoiceSMS
{
    public int status_code { get; set; }
    public bool success { get; set; }
    public string message { get; set; }
    public DataVoice data { get; set; }
}

public class DataVoice
{
    public string id { get; set; }
    public string phone_number { get; set; }
    public int service_id { get; set; }
    public string service_name { get; set; }
    public int status { get; set; }
    public MessageVoice[] messages { get; set; }
    public string created_at { get; set; }
    public string done_at { get; set; }
}

public class MessageVoice
{
    public string otp { get; set; }
    public string audio_file { get; set; }
    public string audio_content { get; set; }
    public bool is_audio { get; set; }
    public string created_at { get; set; }
}
//Callback

public class CallBackSim
{
    public int status_code { get; set; }
    public bool success { get; set; }
    public string message { get; set; }
    public DataCallBack data { get; set; }
}

public class DataCallBack
{
    public string phone_number { get; set; }
    public int network { get; set; }
    public string session { get; set; }
}

#endregion