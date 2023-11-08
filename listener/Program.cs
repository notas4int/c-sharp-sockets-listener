using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace listener {
    internal class Program {
        private static Student student = student = new Student("Vova","Sheshin", 
            20, 2, 
            "Driving", "Rayan Gosling");

        private const int port = 8080; 
        
        public static void Main(string[] args) {
            Thread firstThread = new Thread(firstListener);
            Thread secondThread = new Thread(secondListener);
            Thread thirdThread = new Thread(thirdListener);
            Thread fourthThread = new Thread(fourthListener);
            
            firstThread.Start();
            secondThread.Start();
            thirdThread.Start();
            fourthThread.Start();
            
            Console.WriteLine("-----All listeners are starting-----");
        }

        private static void firstListener() {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/aboutme/");
            listener.Start();

            while (true) {
                var context = listener.GetContext();

                string res =
                    $"{student.name} {student.surname}, {student.age} years old - {student.numberOfCourse} course";
                Console.WriteLine(res + "\n\n");
                
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "Status OK";
                byte[] buffer = Encoding.UTF8.GetBytes(res);
                response.ContentLength64 = buffer.Length;
                
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.FlushAsync();
            }
        }
        
        private static void secondListener() {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/hobby/");
            listener.Start();

            while (true) {
                var context = listener.GetContext();

                string res = student.hobby;
                Console.WriteLine(res + "\n\n");
                
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "Status OK";
                byte[] buffer = Encoding.UTF8.GetBytes(res);
                response.ContentLength64 = buffer.Length;
                
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.FlushAsync();
            }
        }
        
        private static void thirdListener() {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/profession/");
            listener.Start();

            while (true) {
                var context = listener.GetContext();

                string res = student.profession;
                    Console.WriteLine(res + "\n\n");
                
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "Status OK";
                byte[] buffer = Encoding.UTF8.GetBytes(res);
                response.ContentLength64 = buffer.Length;
                
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.FlushAsync();
            }
        }

        private static void fourthListener() {
            var listener = new HttpListener();
            listener.Prefixes.Add($"http://127.0.0.1:{port}/connect/");
            listener.Start();

            while (true) {
                var context = listener.GetContext();

                var request = context.Request;
                Console.WriteLine($"Request from local pc - {request.IsLocal}");
                Console.WriteLine($"IP address server - {request.LocalEndPoint}");
                Console.WriteLine($"IP address client - {request.RemoteEndPoint}");
                Console.WriteLine($"Request URL - {request.RawUrl}");
                Console.WriteLine($"Request URL object - {request.Url}");
                Console.WriteLine("AcceptLanguage - " + request.Headers.Get("AcceptLanguage"));
                Console.WriteLine("Content-Length - " + request.Headers.Get("Content-Length"));
            }
        }
    }

    class Student {
        public Student(string name, string surname, short age, short numberOfCourse, string hobby, string profession) {
            this.name = name;
            this.surname = surname;
            this.age = age;
            this.numberOfCourse = numberOfCourse;
            this.hobby = hobby;
            this.profession = profession;
        }

        public string name { get; set; }
        public string surname { get; set; }
        public short age { get; set; }
        public short numberOfCourse { get; set; }
        public string hobby { get; set; }
        public string profession { get; set; }
    }
}