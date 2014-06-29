using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

namespace REST_API_connection
{
    class RESTCommunication
    {
        //private Authentication auth;
        public bool RestNewUser(String username, String password)
        {

            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", username));
            parameter.Add(new Parameter("password", password));

            dynamic respone = Communication("http://81.169.244.213:8080/api/register", GetPostDatafromString(parameter));

            if (respone["success"])
                return true;
            else
                return false;
        }

        public bool LoginUser(String username, String password)
        {
            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username", username));
            parameter.Add(new Parameter("password", password));
            var response = Communication("http://81.169.244.213:8080/api/login", GetPostDatafromString(parameter));
            if (true)
            {
                //this.auth = new Authentication(username,response);
                return true;
            }
            //else
            //    return false;
        }

        public bool UpdatePosition(String username, float longitude, float latitude) 
        {

            List<Parameter> parameter = new List<Parameter>();
            parameter.Add(new Parameter("username",username));
            parameter.Add(new Parameter("longitude", Convert.ToString(longitude)));
            parameter.Add(new Parameter("latitude", Convert.ToString(latitude)));

            dynamic response = Communication("http://81.169.244.213:8080/api/updateposition", GetPostDatafromString(parameter));

            if (response["success"])
                return true;
            else
                return false;
        }





        private dynamic Communication(String url, byte[] postdata)
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

            dynamic serverresponse = GetServerResponseByJsonString(responsestring);

            return serverresponse;
        }

        private dynamic GetServerResponseByJsonString(String serverresponse)
        {
            JavaScriptSerializer javascriptserializer = new JavaScriptSerializer();
            dynamic sr = javascriptserializer.Deserialize<dynamic>(serverresponse);
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
}
