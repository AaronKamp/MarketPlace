using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Configuration;
using System.Net;
using WinSCP;
using System.Net.Mail;
using Marketplace.Admin.Core;
using Marketplace.Admin.Data.Repository;
using Marketplace.Admin.Data.Infrastructure;

namespace Marketplace.Admin.Extract
{
    public class Process
    {
        private readonly SettingsRepository _settingsRepository;
        const string Delimiter = "|";
        readonly DataSet DataSet = new DataSet();
        private string RootPath = AppDomain.CurrentDomain.BaseDirectory + "CSV_Extracts";

        public Process()
        {
            _settingsRepository = new SettingsRepository(new DbFactory());
        }
        public void UploadFiles()
        {
            var xml = XDocument.Load(@"Queries.xml");
            var dataSource = DeserializeObject<DataSource>(xml.ToString());

            foreach (var query in dataSource.Queries)
            {
                ProcessQuery(query);
            }

            Model.ConfigurationSettings settings = GetSettings();
                 
            BuildDelimittedFiles();

            string message = UploadToFTP(settings);
            
            SendEmail(message, settings);
        }
        

        private Model.ConfigurationSettings GetSettings()
        {
            var settings = _settingsRepository.GetAll().FirstOrDefault();
            settings.SshPrivateKey = Cryptography.DecryptContent(settings.SshPrivateKey);
            settings.SshPrivateKeyPassword = settings.IsSshPasswordProtected ? Cryptography.DecryptContent(settings.SshPrivateKeyPassword) : null;
            settings.FromEmailPassword = Cryptography.DecryptContent(settings.FromEmailPassword);
            return settings;
        }

        private DataTable GetSettingTable()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MarketplaceAdminDb"].ConnectionString;
            DataSet ds = new DataSet();

            using (var connection = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT top 1 FtpHostAddress,FtpPort,FtpUser,SshPrivateKey,IsSshPasswordProtected,SshPrivateKeyPassword,FtpRemotePath,FromEmail,FromEmailPassword,ToEmails,SmtpClient,SmtpPort FROM [dbo].[ConfigurationSettings]";
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, connection);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                sqlAdapter.Fill(ds);
            }
            return ds.Tables[0];
        }

        private void SendEmail(string messageBody, Model.ConfigurationSettings settings)
        {

            string recepientList = settings.ToEmails;
            MailAddress toEmail = new MailAddress(recepientList.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)[0]);
            MailAddress fromEmail = new MailAddress(settings.FromEmail,"TCC Marketplace FTP Update");                  
            MailMessage message = new MailMessage (fromEmail, toEmail);
            foreach (var toAddress in recepientList.Split(new[] {',',';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(toAddress);
            }
            message.Subject = "Marketplace extract dump";
            message.Body = "<p>The following files has been successfully uploaded to FTP. <br /> <br />" + messageBody+"</p>";
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(settings.SmtpClient);
            client.Credentials = new NetworkCredential(fromEmail.Address, settings.FromEmailPassword);
            client.Port = Convert.ToInt32(settings.SmtpPort);
            client.EnableSsl = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string UploadToFTP(Model.ConfigurationSettings settings)
        {
            try
            {
                string SSH_Private_Key_File = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["SSH_Private_Key_File"];
                File.WriteAllText(SSH_Private_Key_File, settings.SshPrivateKey);

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = settings.FtpHostAddress,
                    UserName = settings.FtpUser,
                    PortNumber = settings.FtpPort,
                    SshPrivateKeyPath = SSH_Private_Key_File,
                    GiveUpSecurityAndAcceptAnySshHostKey = true
                };

                if (settings.IsSshPasswordProtected)
                    sessionOptions.SshPrivateKeyPassphrase = settings.SshPrivateKeyPassword;

                List<string> uploadedFiles = new List<string>();
                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    var directoryInfo = new DirectoryInfo(RootPath);
                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(RootPath+ @"\*.txt", settings.FtpRemotePath, false, transferOptions);
                    
                    // Throw on any error
                    transferResult.Check();
                    int count = 0;
                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {                        
                        uploadedFiles.Add(string.Format("{0}. {1}", ++count, Path.GetFileName(transfer.FileName)));
                    }
                    session.Close();
                    string uploadedFileNames = string.Join("<br />", uploadedFiles);
                    
                    return uploadedFileNames;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void BuildDelimittedFiles()
        {
            if (Directory.Exists(RootPath) == false)
            {
                Directory.CreateDirectory(RootPath);
            }
            else
            {
                var directoryInfo = new DirectoryInfo(RootPath);

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
            }

            foreach (DataTable datatable in DataSet.Tables)
            {

                var filePath = RootPath + "\\" + datatable.TableName.Replace("YYYYMMDD",DateTime.Now.ToString("yyyyMMdd"));
                datatable.WriteToCsvFile(filePath, Delimiter);
            }
        }
        //  Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Marketplace.Admin; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        // Data Source=mktplcsql.database.windows.net;Initial Catalog=mktplcqa;Integrated Security=False;User ID=tccmktplc@mktplcsql;Password=TheNumber8ty0ne;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
        private void ProcessQuery(Query query)
        {
            string marketplaceAdminConString = ConfigurationManager.ConnectionStrings["MarketplaceAdminDb"].ConnectionString;
            
            //using (var con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=true;initial catalog=Marketplace.Admin"))
            using (var con = new SqlConnection(marketplaceAdminConString))
            {
                var da = new SqlDataAdapter(query.Sql, con);
                var dt = new DataTable(query.FileName);
                da.Fill(dt);
                DataSet.Tables.Add(dt);
            }
        }

        private T DeserializeObject<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var tr = new StringReader(xml))
            {
                return (T)serializer.Deserialize(tr);
            }
        }
    }
}
