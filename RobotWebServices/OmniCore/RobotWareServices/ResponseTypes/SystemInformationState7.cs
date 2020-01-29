using Newtonsoft.Json;

namespace RWS.IRC5.RobotWareServices.ResponseTypes
{


    public class SystemInformationState7
    {
        [JsonProperty(PropertyName = "_type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "_title")]
        public string Title { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string Build { get; set; }
        public string Revision { get; set; }
        public string Subrevision { get; set; }
        public string BuildTag { get; set; }
        public string RobApiCompatibilityRevision { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string RwVersion { get; set; }
        public string SysId { get; set; }
        public string StartTm { get; set; }
        public string RWVersionName { get; set; }
    }




    //public class Rootobject
    //{
    //    public _Links _links { get; set; }
    //    public State[] state { get; set; }
    //    public _Embedded _embedded { get; set; }
    //}

    //public class _Links
    //{
    //    public Base _base { get; set; }
    //    public Self self { get; set; }
    //}

    //public class Base
    //{
    //    public string href { get; set; }
    //}

    //public class Self
    //{
    //    public string href { get; set; }
    //}

    //public class _Embedded
    //{
    //    public Resource[] resources { get; set; }
    //}

    //public class Resource
    //{
    //    public _Links1 _links { get; set; }
    //    public string _type { get; set; }
    //    public string _title { get; set; }
    //    public Option[] options { get; set; }
    //}

    //public class _Links1
    //{
    //    public Self1 self { get; set; }
    //}

    //public class Self1
    //{
    //    public string href { get; set; }
    //}

    //public class Option
    //{
    //    public string _type { get; set; }
    //    public string _title { get; set; }
    //    public string option { get; set; }
    //}

    //public class State
    //{
    //    public string _type { get; set; }
    //    public string _title { get; set; }
    //    public string major { get; set; }
    //    public string minor { get; set; }
    //    public string build { get; set; }
    //    public string revision { get; set; }
    //    public string subrevision { get; set; }
    //    public string buildtag { get; set; }
    //    public string robapicompatibilityrevision { get; set; }
    //    public string title { get; set; }
    //    public string type { get; set; }
    //    public string description { get; set; }
    //    public string date { get; set; }
    //    public string name { get; set; }
    //    public string rwversion { get; set; }
    //    public string sysid { get; set; }
    //    public string starttm { get; set; }
    //    public string rwversionname { get; set; }
    //}





}
