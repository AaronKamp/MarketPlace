﻿using System.Web;
using System.IO;
using System.Web.Mvc;
using Marketplace.Admin.Models;
using Marketplace.Admin.Core;
using System.Linq;
using System;
using Marketplace.Admin.Business;
using Marketplace.Admin.ViewModels;

namespace Marketplace.Admin.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ISettingsManager _settingsManager;

        public SettingsController(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }
        // GET: Settings
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

        [HttpPost]
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
                    UpdatedDate = DateTime.Now,
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

                if (confSettings.Id > 0)
                {
                    _settingsManager.UpdateSettings(confSettings);
                }
                else
                {
                    confSettings.CreatedDate = DateTime.Now;
                    _settingsManager.Create(confSettings);
                }
                _settingsManager.SaveSettings();
                return RedirectToAction("Index");

            }
            else
            {
                return View("Index", settings);
            }
        }

    }
}