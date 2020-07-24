using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EPSuzduotis
{
    class HTTPHeaders
    {

        public string HeaderSuccess(int length)
        {
            DateTime date = DateTime.Now;
            string header = "HTTP/1.1 200 OK\n";
            header += "Date: Sun, 10 Oct " + date.Year + " " + string.Format("{0}:{1}:{2}", date.Hour, date.Minute, date.Second) + " GMT\n";
            header += "Server: Apache / 2.2.8(Ubuntu) mod_ssl / 2.2.8 OpenSSL / 0.9.8g\n";
            header += @"ETag: ""45b6-834-49130cc1182c0""\n";
            header += "Accept - Ranges: bytes\n";
            header += "Content - Length: " + length + "\n";
            header += "Connection: close\n";
            header += "Content - Type: text / html\n";
            header += '\n';


            return header;
        }

        public string HeaderNotFound()
        {
            DateTime date = DateTime.Now;
            string header = "HTTP/1.1 404 Not Found\n";
            header += "Date: Sun, 10 Oct " + date.Year + " " + string.Format("{0}:{1}:{2}", date.Hour, date.Minute, date.Second) + " GMT\n";
            header += "Server: Apache / 2.2.8(Ubuntu) mod_ssl / 2.2.8 OpenSSL / 0.9.8g\n";
            header += @"ETag: ""45b6-834-49130cc1182c0""\n";
            header += "Accept - Ranges: bytes\n";
            header += "Content - Length: 19\n";
            header += "Connection: close\n";
            header += "Content - Type: text / html\n";
            header += '\n';
            header += "404 File not found!\n";


            return header;
        }

        public string HeaderBadRequest()
        {
            DateTime date = DateTime.Now;
            string header = "HTTP/1.1 400 Bad Request\n";
            header += "Date: Sun, 10 Oct " + date.Year + " " + string.Format("{0}:{1}:{2}", date.Hour, date.Minute, date.Second) + " GMT\n";
            header += "Server: Apache / 2.2.8(Ubuntu) mod_ssl / 2.2.8 OpenSSL / 0.9.8g\n";
            header += @"ETag: ""45b6-834-49130cc1182c0""\n";
            header += "Accept - Ranges: bytes\n";
            header += "Content - Length: 16\n";
            header += "Connection: close\n";
            header += "Content - Type: text / html\n";
            header += '\n';
            header += "400 Bad Request!\n";


            return header;
        }

    }
}
