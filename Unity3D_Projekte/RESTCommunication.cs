using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using Microsoft.CSharp;

namespace REST_API_connection
{
    class RESTCommunication
    {
        private String secret,username,password;
        private int counter;

        public bool RegisterNewUser(String username, String password)
        {

            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", username));
            parameter.Add(new Parameter("password", password));

            Response respone = Communication("http://81.169.244.213:8080/api/register", GetPostDatafromString(parameter));

            if (respone.Success)
                return true;
            else
                return false;
        }

        public bool LoginUser(String username, String password)
        {
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", username));
            parameter.Add(new Parameter("password", password));
            Response response = Communication("http://81.169.244.213:8080/api/login", GetPostDatafromString(parameter));
            if (response.Success)
            {
                this.secret = response.Session_secret;
                this.counter = 0;
                this.username = username;
                this.password = password;
                return true;
            }
            return false;

        }

        public bool UpdatePosition(float longitude, float latitude) 
        {

            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username",this.username));
            parameter.Add(new Parameter("token",GenrateToken()));
            parameter.Add(new Parameter("longitude", Convert.ToString(longitude)));
            parameter.Add(new Parameter("latitude", Convert.ToString(latitude)));

            Response response = Communication("http://81.169.244.213:8080/api/updateposition", GetPostDatafromString(parameter));

            if (response.Success)
                return true;
            else
                return false;
        }
      
        private Response Communication(String url, byte[] postdata)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            Stream datastream = request.GetRequestStream();
            datastream.Write(postdata, 0, postdata.Length);
            datastream.Close();

            WebResponse response = request.GetResponse();
            datastream = response.GetResponseStream();
            StreamReader streamreader = new StreamReader(datastream);
            String responsestring = streamreader.ReadToEnd();

            Response serverresponse = GetServerResponseByJsonString(responsestring);

            return serverresponse;
        }

        public Response GetServerResponseByJsonString(String serverresponse)
        {
            JavaScriptSerializer javascriptserializer = new JavaScriptSerializer();
            Response sr = javascriptserializer.Deserialize<Response>(serverresponse);
            return sr;
        }

        private byte[] GetPostDatafromString(List<Parameter>parameter) 
        {
            String postdata ="";
            foreach (Parameter p in parameter) 
            {
                postdata += p.Name + "=" + p.Content + "&";
            }
            postdata.Remove(postdata.Length - 1);
            byte[] bytes = Encoding.UTF8.GetBytes(postdata);

            return bytes;
        }

        private string GenrateToken() 
        {
            string token ="";
            String concat_secret_counter = this.secret + this.counter;
            
            MD5 md5 = MD5.Create();
            byte[] bytehash = md5.ComputeHash(Encoding.UTF8.GetBytes(concat_secret_counter));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytehash.Length; i++) 
            {
                sb.Append(bytehash[i].ToString());
            }

            token = sb.ToString();
                        
            this.counter++;

            return token;
        }
    }

    public class Parameter
    {
        string content;
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        public Parameter(String name, String content)
        {
            this.name = name;
            this.content = content;
        }
    }

    public class Response 
    {
        private bool success;
        private string error;
        private string session_secret;
        
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }
        
        public string Session_secret
        {
            get { return session_secret; }
            set { session_secret = value; }
        }
        
        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        public Response()
        {
            this.success = false;
            this.session_secret = "";
            this.error = "";
        }
    }
}
