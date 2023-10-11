using DALInterface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using DTO;
using FluentFTP;
using FluentFTP.Exceptions;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class AudiofileDal : IAudiofileDal
    {
        public string UploadFile(IFormFile file, AudiofileDTO audiofileDto)
        {
            string ftpserver = GetFTPServer();
            string ftpusername = GetFTPUsername();
            string ftppassword = GetFTPPassword();
            bool exists = CheckFolderExistence(audiofileDto.Uploaderid);
            if (!exists)
            {
                CreateFTPFolder(audiofileDto.Uploaderid);
                ChangeFolderPermissions(audiofileDto.Uploaderid);
            }

            string response = SendToFtpServer(ftpserver, ftpusername, ftppassword, file, audiofileDto.Uploaderid);
            ChangeFilePermissions(file, audiofileDto.Uploaderid);
            audiofileDto.Path = $"{GetFTPServer()}//files/{audiofileDto.Uploaderid}/{file.FileName}";
            string input = GetFTPServer();
            int index = input.IndexOf(":"); 
            string ip = input.Substring(index + 1);
            audiofileDto.url = $"http:{ip}:9998/{audiofileDto.Uploaderid}/{file.FileName}";
            UploadToMysql(audiofileDto);
            return response;
        }

        private string SendToFtpServer(string ftpserver, string FtpUsername, string FtpPassword, IFormFile file,
            int uploaderid)
        {

            string remotepath = $"{ftpserver}//files/{uploaderid}/{file.FileName}";
            var request = (FtpWebRequest)WebRequest.Create(remotepath);
            request.Method = WebRequestMethods.Ftp.AppendFile;
            request.Credentials = new NetworkCredential(FtpUsername, FtpPassword);
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
                return response.StatusDescription;

            }
        }

        private void CreateFTPFolder(int uploaderid)
        {
            using (FtpClient client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
            {
                client.Connect();
                try
                {
                    // Execute custom FTP command
                    client.CreateDirectory($"files/{uploaderid}", true);
                    // Console.WriteLine("Custom FTP command executed successfully.");
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

        private bool CheckFolderExistence(int uploaderid)
        {
            using (FtpClient client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
            {
                bool exists = false;
                client.Connect();
                try
                {
                    // Execute custom FTP command
                    exists = client.DirectoryExists($"{GetFTPServer()}//files/{uploaderid}");
                    // Console.WriteLine("Custom FTP command executed successfully.");
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

                return exists;
            }
        }

        private void UploadToMysql(AudiofileDTO audiofiledto)
        {
            string? connectionstring = Getconnectionstring();
            MySqlConnection conn = new MySqlConnection(connectionstring);
            conn.Open();
            string query = "INSERT INTO audiofile (name, path, duration, uploaddate, uploaderid, url)" +
                           "VALUES(@Name, @Path, @Duration, @Uploaddate, @Uploaderid, @url)";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Name", audiofiledto.Filename);
                cmd.Parameters.AddWithValue("@Path", audiofiledto.Path);
                cmd.Parameters.AddWithValue("@Duration", audiofiledto.Duration);
                cmd.Parameters.AddWithValue("@Uploaddate", audiofiledto.Uploaddate);
                cmd.Parameters.AddWithValue("@Uploaderid", audiofiledto.Uploaderid);
                cmd.Parameters.AddWithValue("@url", audiofiledto.url);
                cmd.ExecuteNonQuery();
            }
        }

        public void StoreTempFile(IFormFile file)
        {
            string ftpserver = GetFTPServer();
            string ftpusername = GetFTPUsername();
            string ftppassword = GetFTPPassword();
            string remotepath = $"{ftpserver}//files/temp/{file.FileName}";
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
                // return response.StatusDescription;

            }
        }

        public void RemoveTempFile(string remotefilepath)
        {

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

        private string? Getconnectionstring()
        {
            string? jsonfile = "appsettings.json";
            JsonObject? jsonObject = (JsonObject?)JsonObject.Parse(File.ReadAllText(jsonfile));
            string? sqlservervalue = (string?)jsonObject["ConnectionStrings"]["SqlServer"];
            return sqlservervalue;
        }

        private void ChangeFilePermissions(IFormFile file, int uploaderid)
        {
            using (FtpClient client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
            {
                client.Connect();
                try
                {
                    // Execute custom FTP command
                    client.Execute($"SITE CHMOD 604 files/{uploaderid}/{file.FileName}");
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

        private void ChangeFolderPermissions(int uploaderid)
        {
            using (FtpClient client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
            {
                client.Connect();
                try
                {
                    // Execute custom FTP command
                    client.Execute($"SITE CHMOD 705 files/{uploaderid}");
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