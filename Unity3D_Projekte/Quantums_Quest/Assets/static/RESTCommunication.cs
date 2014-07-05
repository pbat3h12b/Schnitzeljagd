using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using SimpleJSON;

	class RESTCommunication : MonoBehaviour
    {
        private static String secret,username,password;
        private static int counter;

        public bool RegisterNewUser(String username, String password)
        {
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", username));
            parameter.Add(new Parameter("password", password));

            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/register", GetPostDatafromString(parameter)));

            if (response["success"].AsBool)
                return true;
            else
                return false;
        }

        public bool LoginUser(String _username, String _password)
        {
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", _username));
            parameter.Add(new Parameter("password", _password));
            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/login", GetPostDatafromString(parameter)));
            if (response["success"].AsBool)
            {
                secret = response["session_secret"].Value;
                counter = 0;
                username = _username;
                password = _password;
                return true;
            }
            return false;

        }

        public bool UpdatePosition(float longitude, float latitude) 
        {

            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username",username));
            parameter.Add(new Parameter("token",GenrateToken()));
            parameter.Add(new Parameter("longitude", Convert.ToString(longitude)));
            parameter.Add(new Parameter("latitude", Convert.ToString(latitude)));

            var response = JSON.Parse(Communication("http://81.169.244.213:8080/api/updatePosition", GetPostDatafromString(parameter)));

            if (response["success"].AsBool)
                return true;
            else
                return false;
        }
      
        private string Communication(String url, byte[] postdata)
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
                      

            return responsestring;
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
            String concat_secret_counter = secret + counter;
            
            MD5 md5 = MD5.Create();
            byte[] bytehash = md5.ComputeHash(Encoding.UTF8.GetBytes(concat_secret_counter));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < bytehash.Length; i++) 
            {
                sb.Append(bytehash[i].ToString("x2"));
            }

            token = sb.ToString();
			Debug.Log (token);
			Debug.Log (counter);
                        
            counter++;

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

    //public class Response 
    //{
    //    private bool success;
    //    private string error;
    //    private string session_secret;
        
    //    public bool Success
    //    {
    //        get { return success; }
    //        set { success = value; }
    //    }
        
    //    public string Session_secret
    //    {
    //        get { return session_secret; }
    //        set { session_secret = value; }
    //    }
        
    //    public string Error
    //    {
    //        get { return error; }
    //        set { error = value; }
    //    }

    //    public Response()
    //    {
    //        this.success = false;
    //        this.session_secret = "";
    //        this.error = "";
    //    }
    //}

