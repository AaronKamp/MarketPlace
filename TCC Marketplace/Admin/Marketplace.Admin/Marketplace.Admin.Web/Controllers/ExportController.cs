using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Marketplace.Admin.Business;

namespace Marketplace.Admin.Controllers
{
    /// <summary>
    /// Controls export job operations.
    /// </summary>
    [Authorize]
    public class ExportController : Controller
    {
        private readonly IExportManager _exportManager;

        /// <summary>
        /// Parameterized constructor to work with dependency injection
        /// </summary>
        /// <param name="exportManager"></param>
        public ExportController(IExportManager exportManager)
        {
            _exportManager = exportManager;
        }

        /// <summary>
        /// Gets the index page.
        /// GET: Export
        /// </summary>
        /// <returns>The export job page.</returns>
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

        /// <summary>
        /// Action to update frequency .
        /// </summary>
        /// <param name="frequencyId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateFrequency(byte frequencyId)
        {
            try
            {
                //if job schedule is successfully updated in Azure, update database 
                if (UpdateWebJobSchedule(frequencyId))
                {
                    var exportFrequency = _exportManager.GetExtractFrequency();

                    //if new record, create new entry
                    if (exportFrequency == null)
                    {
                        exportFrequency = new Model.ExtractFrequency
                        {
                            FrequencyId = frequencyId,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            UpdatedUser = User.Identity.Name
                        };

                        _exportManager.CreateExportFrequency(exportFrequency);
                    }
                    else
                    {
                        //update existing frequency
                        exportFrequency.FrequencyId = frequencyId;
                        exportFrequency.UpdatedDate = DateTime.UtcNow;
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
            catch (Exception)
            {
                //log exception
                var json = new
                {
                    success = false,
                    message = "Failed to update extract frequency."
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Updates web job schedule using Azure Web-job rest API.
        /// </summary>
        /// <param name="frequencyId"> Selected schedule frequency Id.</param>
        /// <returns>Boolean</returns>
        private bool UpdateWebJobSchedule(byte frequencyId)
        {
            try
            {
                var webJobSettingsApi = string.Format(ConfigurationManager.AppSettings["WebJobApi"],
                                                           ConfigurationManager.AppSettings["SiteName"],
                                                           ConfigurationManager.AppSettings["ExtractJob"],
                                                           "settings");

                var frequency = _exportManager.GetFrequency(frequencyId);

                var schedule = "{\"schedule\":\""+ frequency.CronExpression +"\"}";
                
                using (var httpClient = new HttpClient())
                {
                    HttpContent contentPost = new StringContent(schedule, Encoding.UTF8, "application/json");

                    //set the basic Authorization header for http request
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


        /// <summary>
        /// Kick off the extract job on Azure using Azure rest API
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RunExtract()
        {
            try
            {
                string message = string.Empty;
                bool success = false;

                var webJobRunApi = string.Format(ConfigurationManager.AppSettings["WebJobApi"],
                                                       ConfigurationManager.AppSettings["SiteName"],
                                                       ConfigurationManager.AppSettings["ExtractJob"],
                                                       "run"
                                                );

                using (var httpClient = new HttpClient())
                {
                    HttpContent contentPost = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                    
                    //set the basic Authorization header for http request
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
            catch (Exception)
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

        /// <summary>
        /// Sets basic authorization header.
        /// </summary>
        /// <param name="client"></param>
        private void SetBasicAuthHeader(HttpClient client)
        {
            string authInfo = ConfigurationManager.AppSettings["SiteUserName"] + ":" + ConfigurationManager.AppSettings["SiteUserPwd"];
            var byteArray = Encoding.ASCII.GetBytes(authInfo);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

    }
}