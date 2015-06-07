using System.Runtime.Serialization;
using System.Web.Script.Serialization;
namespace ServerJsonObject
{
   [DataContract]
    public class ChatObject
    {
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Avatar { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Message { get; set; }

       public ChatObject()
       {
           
       }

        public ChatObject(string login, string avatar, int code, string message)
        {
            Login = login;
            Avatar = Avatar;
            Code = code;
            Message = message;
        }
    }

    public static class JsonSerializer
    {
        //two object for minimalize conflicts
        private static readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private static readonly JavaScriptSerializer _deserializer = new JavaScriptSerializer();
        public static string ObjectToJson(this ChatObject obj)
        {
           return _serializer.Serialize(obj);
        }

        public static ChatObject JsonToObject(this string json)
        {
            return _deserializer.Deserialize<ChatObject>(json);
        }
    }
}
