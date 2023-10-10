using DALInterface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using FluentFTP;
using FluentFTP.Exceptions;

namespace DAL
{
    public class AudiofileDal : IAudiofileDal
    {
        public string UploadFile(IFormFile file)
        {
            string ftpserver = GetFTPServer();
            string ftpusername = GetFTPUsername();
            string ftppassword = GetFTPPassword();

            string remotepath = $"{ftpserver}//files/{file.FileName}";
            var request = (FtpWebRequest)WebRequest.Create(remotepath);
            request.Method = WebRequestMethods.Ftp.AppendFile;
            request.Credentials = new NetworkCredential(ftpusername, ftppassword);
            request.UsePassive = true;
            request.UseBinary = true;
            request.UsePassive = true;

            using (var stream = request.GetRequestStream())
            {
                file.CopyTo(stream);
            }

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete. Status: {response.StatusDescription}");
                ChangeFilePermissions(file);
                return response.StatusDescription;
            }
        }

        private JsonObject GetAppsettings()
        {
            string? jsonfile = "appsettings.json";
            JsonObject? jsonObject = (JsonObject?)JsonObject.Parse(File.ReadAllText(jsonfile));
            return jsonObject;
        }
        private string GetFTPServer()
        {
            JsonObject jsonObject = GetAppsettings();
            string? ftpserver = (string?)jsonObject["ConnectionStrings"]["ftpServer"];
            return ftpserver;
        }
        private string GetFTPUsername()
        {
            JsonObject jsonObject = GetAppsettings();
            string ftpusername = (string?)jsonObject["ConnectionStrings"]["ftpUsername"];
            return ftpusername;
        }
        private string GetFTPPassword()
        {
            JsonObject jsonObject = GetAppsettings();
            string ftppassword = (string?)jsonObject["ConnectionStrings"]["ftpPassword"];
            return ftppassword;

        }
        private void ChangeFilePermissions(IFormFile file)
        {
            using (FtpClient client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
            {
                client.Connect();
                try
                {
                    // Execute custom FTP command
                    client.Execute($"SITE CHMOD 604 files/{file.FileName}");
                    Console.WriteLine("Custom FTP command executed successfully.");
                }
                catch (FtpCommandException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    // Disconnect from the FTP server
                    client.Disconnect();
                }
            }
        }



    }
}
