using System.Web;
using System.IO;
using System.Web.Mvc;
using Marketplace.Admin.Models;
using Marketplace.Admin.Core;
using System;
using Marketplace.Admin.Business;
using Marketplace.Admin.ViewModels;
using Marketplace.Admin.Filters;

namespace Marketplace.Admin.Controllers
{
    /// <summary>
    /// Controls Settings manipulation operations.
    /// </summary>
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ISettingsManager _settingsManager;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="settingsManager"></param>
        public SettingsController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        /// <summary>
        /// Gets the settings view.
        /// GET: Settings
        /// </summary>
        /// <returns>Settings view. </returns>
        public ActionResult Index()
        {
            Model.ConfigurationSettings settings = _settingsManager.GetSettings();
            if (settings != null)
            {
                var settingsView = new SettingsViewModel
                {
                    Id = settings.Id,
                    FtpDetails = new FtpDetailsViewModel
                    {
                        FtpHostAddress = settings.FtpHostAddress,
                        FtpPort = settings.FtpPort,
                        FtpUser = settings.FtpUser,
                        SshPrivateKey = Cryptography.DecryptContent(settings.SshPrivateKey),
                        IsSshPasswordProtected = settings.IsSshPasswordProtected,
                        SshPrivateKeyPassword = settings.SshPrivateKeyPassword,
                        FtpRemotePath = settings.FtpRemotePath
                    },
                    SmtpDetails = new SMTPDetailsViewModel
                    {
                        FromEmail = settings.FromEmail,
                        FromEmailPassword = settings.FromEmailPassword,
                        SmtpClient = settings.SmtpClient,
                        ToEmails = settings.ToEmails,
                        SmtpPort = settings.SmtpPort
                    }

                };
                return View(settingsView);
            }
            else return View(new SettingsViewModel());
        }

        /// <summary>
        /// Handles the settings update.
        /// </summary>
        /// <param name="settings"> SettingsViewModel </param>
        /// <param name="sshKeyFile"> SSH Key File</param>
        /// <returns>Settings view</returns>
        [HttpPost]
        [SecureDataActionFilter]
        public ActionResult Edit(SettingsViewModel settings, HttpPostedFileBase sshKeyFile)
        {
            string radio = settings.FtpDetails.ReadMode;
            string sshkey = null;
            if (string.Equals(radio, "Paste", System.StringComparison.OrdinalIgnoreCase))
            {
                sshkey = settings.FtpDetails.SshPrivateKey;
            }
            else if (string.Equals(radio, "ReadFromFile", System.StringComparison.OrdinalIgnoreCase))
            {
                if (sshKeyFile == null)
                {
                    ModelState.AddModelError(string.Empty, "Select SSH key file");
                }
                else {
                    sshkey = new StreamReader(sshKeyFile.InputStream).ReadToEnd();
                }
            }
            if (ModelState.IsValid )
            {
                var confSettings = new Model.ConfigurationSettings
                {
                    Id = settings.Id,
                    FtpHostAddress = settings.FtpDetails.FtpHostAddress,
                    FtpPort = settings.FtpDetails.FtpPort,
                    FtpUser = settings.FtpDetails.FtpUser,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedUser = User.Identity.Name,
                    IsSshPasswordProtected = settings.FtpDetails.IsSshPasswordProtected,
                    SshPrivateKeyPassword = settings.FtpDetails.SshPrivateKeyPassword == null ? null : Cryptography.EncryptContent(settings.FtpDetails.SshPrivateKeyPassword),
                    SshPrivateKey = sshkey == null ? null : Cryptography.EncryptContent(sshkey),
                    FtpRemotePath = settings.FtpDetails.FtpRemotePath,
                    FromEmail = settings.SmtpDetails.FromEmail,
                    FromEmailPassword = settings.SmtpDetails.FromEmailPassword == null ? null : Cryptography.EncryptContent(settings.SmtpDetails.FromEmailPassword),
                    SmtpClient = settings.SmtpDetails.SmtpClient,
                    SmtpPort = settings.SmtpDetails.SmtpPort,
                    ToEmails = settings.SmtpDetails.ToEmails
                };

                string message;

                if (confSettings.Id > 0)
                {
                    _settingsManager.UpdateSettings(confSettings);
                    message = $"Settings has been updated successfully!";
                }
                else
                {
                    confSettings.CreatedDate = DateTime.UtcNow;
                    _settingsManager.Create(confSettings);
                    message = $"Settings has been saved successfully!";
                }
                _settingsManager.SaveSettings();
                                
                TempData["ResponseMessage"] = message;

                return RedirectToAction("Index");

            }
            else
            {
                return View("Index", settings);
            }
        }

    }
}