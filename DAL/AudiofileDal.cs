using System.Net;
using System.Text.Json.Nodes;
using DALInterface;
using DTO;
using FluentFTP;
using FluentFTP.Exceptions;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace DAL;

public class AudiofileDal : IAudiofileDal
{
    public string UploadFile(IFormFile file, AudiofileDTO audiofileDto)
    {
        var ftpserver = GetFTPServer();
        var ftpusername = GetFTPUsername();
        var ftppassword = GetFTPPassword();
        var ftppath = GetFTPPath();
        var exists = CheckFolderExistence(audiofileDto.Uploaderid);
        if (!exists)
        {
            CreateFTPFolder(audiofileDto.Uploaderid);
            ChangeFolderPermissions(audiofileDto.Uploaderid);
        }

        var response = SendToFtpServer(ftpserver, ftpusername, ftppassword, ftppath ,file, audiofileDto.Uploaderid);
        ChangeFilePermissions(file, audiofileDto.Uploaderid);
        audiofileDto.Path = $"{GetFTPServer()}//{ftppath}/{audiofileDto.Uploaderid}/{file.FileName}";
        var input = GetFTPServer();
        var index = input.IndexOf(":");
        var ip = input.Substring(index + 1);
        audiofileDto.url = $"http:{ip}:9998/{audiofileDto.Uploaderid}/{file.FileName}";
        if (!CheckUrlExistance(audiofileDto.url)) UploadToMysql(audiofileDto);
        return response;
    }

    public List<AudiofileDTO> GetFiles(int userid)
    {
        List<AudiofileDTO> ListFiles = new List<AudiofileDTO>();
        string? connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        string query = "SELECT * FROM audiofile WHERE uploaderid = @Userid";
        MySqlCommand command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@Userid", userid);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            AudiofileDTO audiofileDto = new AudiofileDTO();
            audiofileDto.Uploaderid = reader.GetInt32("uploaderid");
            audiofileDto.Id = reader.GetInt32("id");
            audiofileDto.Path = reader.GetString("path");
            audiofileDto.Filename = reader.GetString("name");
            audiofileDto.url = reader.GetString("url");
            audiofileDto.Uploaddate = reader.GetDateTime("uploaddate");
            ListFiles.Add(audiofileDto);
            
        }
        conn.Close();
        return ListFiles;
    }

    public bool DeleteFile(int audiofileid, int userid, string ftppath)
    {

        bool deleted = DeleteFileFromServer(userid, ftppath);
        if(deleted)
        {
            string? connectionstring = Getconnectionstring();
            var conn = new MySqlConnection(connectionstring);
            conn.Open();
            string query = "DELETE FROM audiofile WHERE uploaderid = @Userid AND id = @Audiofileid";
            using (var cmd = new MySqlCommand(query,conn))
            {
                cmd.Parameters.AddWithValue("@Userid", userid);
                cmd.Parameters.AddWithValue("@Audiofileid", audiofileid);
                int rowsaffected = cmd.ExecuteNonQuery();
                conn.Close();
                return rowsaffected > 0; 
            }
            
        }
        
        return false;
    }

    private bool DeleteFileFromServer(int userid, string ftppath)
    {
        string ftpUsername = GetFTPUsername();
        string ftpPassword = GetFTPPassword();
        try
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftppath);
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            //Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }            
    }

    private string SendToFtpServer(string ftpserver, string FtpUsername, string FtpPassword, string ftppath ,IFormFile file,
        int? uploaderid)
    {
        var remotepath = $"{ftpserver}//{ftppath}/{uploaderid}/{file.FileName}";
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

    private void CreateFTPFolder(int? uploaderid)
    {
        using (var client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
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

    private bool CheckFolderExistence(int? uploaderid)
    {
        using (var client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
        {
            var exists = false;
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

    private bool CheckUrlExistance(string urlpath)
    {
        var connectionstring = Getconnectionstring();
        var connection = new MySqlConnection(connectionstring);
        connection.Open();
        var query = "SELECT * FROM audiofile WHERE url = @Url";
        using (var cmd = new MySqlCommand(query, connection))
        {
            cmd.Parameters.AddWithValue("@Url", urlpath);
            using (var reader = cmd.ExecuteReader())
            {
                return reader.HasRows;
            }
        }
    }

    private void UploadToMysql(AudiofileDTO audiofiledto)
    {
        var connectionstring = Getconnectionstring();
        var conn = new MySqlConnection(connectionstring);
        conn.Open();
        var query = "INSERT INTO audiofile (name, path, duration, uploaddate, uploaderid, url)" +
                    "VALUES(@Name, @Path, @Duration, @Uploaddate, @Uploaderid, @url)";
        using (var cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Name", audiofiledto.Filename);
            cmd.Parameters.AddWithValue("@Path", audiofiledto.Path);
            cmd.Parameters.AddWithValue("@Duration", audiofiledto.Duration);
            cmd.Parameters.AddWithValue("@Uploaddate", audiofiledto.Uploaddate);
            cmd.Parameters.AddWithValue("@Uploaderid", audiofiledto.Uploaderid);
            cmd.Parameters.AddWithValue("@url", audiofiledto.url);
            cmd.ExecuteNonQuery();
        }
        conn.Close();
    }
    
    


    private JsonObject GetAppsettings()
    {
        var jsonfile = "appsettings.json";
        var jsonObject = (JsonObject?)JsonNode.Parse(File.ReadAllText(jsonfile));
        return jsonObject;
    }

    private string GetFTPServer()
    {
        var jsonObject = GetAppsettings();
        var ftpserver = (string?)jsonObject["ConnectionStrings"]["ftpServer"];
        return ftpserver;
    }

    private string GetFTPUsername()
    {
        var jsonObject = GetAppsettings();
        var ftpusername = (string?)jsonObject["ConnectionStrings"]["ftpUsername"];
        return ftpusername;
    }

    private string GetFTPPassword()
    {
        var jsonObject = GetAppsettings();
        var ftppassword = (string?)jsonObject["ConnectionStrings"]["ftpPassword"];
        return ftppassword;
    }

    private string? Getconnectionstring()
    {
        var jsonfile = "appsettings.json";
        var jsonObject = (JsonObject?)JsonNode.Parse(File.ReadAllText(jsonfile));
        var sqlservervalue = (string?)jsonObject["ConnectionStrings"]["SqlServer"];
        return sqlservervalue;
    }

    private string? GetFTPPath()
    {
        var jsonObject = GetAppsettings();
        var ftppath = (string?)jsonObject["ConnectionStrings"]["ftpPath"];
        return ftppath;
    
    }

    private void ChangeFilePermissions(IFormFile file, int? uploaderid)
    {
        using (var client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
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

    private void ChangeFolderPermissions(int? uploaderid)
    {
        using (var client = new FtpClient(GetFTPServer(), GetFTPUsername(), GetFTPPassword()))
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