using System;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace ChatJsonObject
{
   [DataContract]
    public class ChatObject
    {
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Avatar { get; set; }
        [DataMember]
        public ChatCodes Code { get; set; }
        [DataMember]
        public string Message { get; set; }

       public ChatObject()
       {
           
       }

       public ChatObject(string login, string avatar, ChatCodes code, string message)
        {
            Login = login;
            Avatar = Avatar;
            Code = code;
            Message = message;
        }
    }

    public enum ChatCodes
    {
        Conected = 1
        ,CloseConection = 404 
        ,StopServer = 405
        
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
            try
            {
                return _deserializer.Deserialize<ChatObject>(json);
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
            catch (ArgumentException e)
            {
                return null;
            }
        }
    }
}
