using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Marketplace.Admin.Business;

namespace Marketplace.Admin.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        private readonly IExportManager _exportManager;

        public ExportController(IExportManager _exportManager)
        {
            this._exportManager = _exportManager;
        }

        // GET: Export
        public ActionResult Index()
        {
            int exportFrequencyId = 0;
            var exportFrequency = _exportManager.GetExtractFrequency();

            if (exportFrequency != null)
            {
                exportFrequencyId = exportFrequency.FrequencyId;
            }

            ViewBag.Frequncies = new SelectList(_exportManager.GetFrequencies(), "Id", "Name", exportFrequencyId);
            return View();
        }

        [HttpPost]
        public JsonResult UpdateFrequency(byte frequencyId)
        {
            try
            {
                if (UpdateWebJobSchedule(frequencyId))
                {
                    var exportFrequency = _exportManager.GetExtractFrequency();

                    if (exportFrequency == null)
                    {
                        exportFrequency = new Model.ExtractFrequency
                        {
                            FrequencyId = frequencyId,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now,
                            UpdatedUser = User.Identity.Name
                        };

                        _exportManager.CreateExportFrequency(exportFrequency);
                    }
                    else
                    {
                        exportFrequency.FrequencyId = frequencyId;
                        exportFrequency.UpdatedDate = DateTime.Now;
                        exportFrequency.UpdatedUser = User.Identity.Name;
                        _exportManager.UpdateExportFrequency(exportFrequency);
                    }

                    _exportManager.SaveExportFrequency();

                    var json = new
                    {
                        success = true,
                        message = "Export frequency has been updated successfully!"
                    };

                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var json = new
                    {
                        success = false,
                        message = "Failed to update web job settings"
                    };

                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var json = new
                {
                    success = false,
                    message = "Failed to update extract frequency."
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        private bool UpdateWebJobSchedule(byte frequencyId)
        {
            try
            {
                var webJobSettingsApi = string.Format(ConfigurationManager.AppSettings["WebJobApi"],
                                                           ConfigurationManager.AppSettings["SiteName"],
                                                           // Environment.ExpandEnvironmentVariables("%WEBSITE_SITE_NAME%"),
                                                           ConfigurationManager.AppSettings["ExtractJob"],
                                                           "settings");

                var frequency = _exportManager.GetFrequency(frequencyId);

                var schedule = "{\"schedule\":\""+ frequency.CronExpression +"\"}";
                
                using (var httpClient = new HttpClient())
                {
                    HttpContent contentPost = new StringContent(schedule, Encoding.UTF8, "application/json");
                    SetBasicAuthHeader(httpClient);
                    var response = httpClient.PutAsync(webJobSettingsApi, contentPost);
                    var responseContent = response.Result.Content.ReadAsStringAsync();

                    if (response.Result.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult RunExtract()
        {
            try
            {
                string message = string.Empty;
                bool success = false;

                var webJobRunApi = string.Format(ConfigurationManager.AppSettings["WebJobApi"],
                                                       ConfigurationManager.AppSettings["SiteName"],
                                                       // Environment.ExpandEnvironmentVariables("%WEBSITE_SITE_NAME%"),
                                                       ConfigurationManager.AppSettings["ExtractJob"],
                                                       "run"
                                                );

                using (var httpClient = new HttpClient())
                {
                    HttpContent contentPost = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                    SetBasicAuthHeader(httpClient);
                    var response = httpClient.PostAsync(webJobRunApi, contentPost);
                    var responseContent = response.Result.Content.ReadAsStringAsync();

                    if (response.Result.StatusCode == HttpStatusCode.Accepted)
                    {
                        success = true;
                        message = "Extract job initiated successfully!";
                    }
                    else
                    {
                        success = false;
                        message = responseContent.Result;
                    }

                }

                var json = new
                {
                    success = success,
                    message = message
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log exception
                var json = new
                {
                    success = false,
                    message = "Failed to execute extract job"
                };
                return Json(json, JsonRequestBehavior.AllowGet);

            }
        }

        private void SetBasicAuthHeader(HttpClient client)
        {
            string authInfo = ConfigurationManager.AppSettings["SiteUserName"] + ":" + ConfigurationManager.AppSettings["SiteUserPwd"];
            var byteArray = Encoding.ASCII.GetBytes(authInfo);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

    }
}