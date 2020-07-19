using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using TrainingTests.Helpers;
using TrainingTests.Models;
using TrainingTests.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainingTests.Controllers
{
    [Route("api/[controller]")]
    public class MeetingController : Controller
    {
        private readonly ILogger<MeetingController> _logger;
        private readonly MySqlContext _context;

        public MeetingController(ILogger<MeetingController> logger)
        {
            _logger = logger;
            //_context = new Repository.LocalRepository();
        }

        ///<summary>
        /// Download excel file with all table
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Excel
        /// </remarks>
        /// <returns>Excel file</returns>
        /// <response code="200">Excel file</response>
        /// <response code="400">Bad request</response>   
        [HttpGet("Excel")]
        public IActionResult Excel(string table)
        {
            _logger.LogInformation("Create excel file table = " + table);

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var meetupsWorksheet = workbook.Worksheets.Add("meetups");
                var speakersWorksheet = workbook.Worksheets.Add("speakers");

                var meetups = _context.Meetups.ToList();
                var speakers = _context.Speakers.ToList();
                var eventProfiles = _context.EventProfileUsers.ToList();

                meetupsWorksheet.Cell("A1").Value = "Пойдут на встречу";

                meetupsWorksheet.Cell("A2").Value = "Телефон";
                meetupsWorksheet.Cell("B2").Value = "Почта";
                meetupsWorksheet.Cell("C2").Value = "ФИО";
                meetupsWorksheet.Cell("D2").Value = "Дата регистрации";
                meetupsWorksheet.Cell("E2").Value = "Активация";
                meetupsWorksheet.Cell("F2").Value = "Место работы";
                meetupsWorksheet.Cell("G2").Value = "Должность";
                meetupsWorksheet.Cell("H2").Value = "Группа";

                for (int i = 0; i < meetups.Count; i++)
                {
                    var profile = eventProfiles.Find(a => a.Id.Equals(meetups[i].EventProfileUserId));
                    meetupsWorksheet.Cell("A" + (i + 3)).Value = profile.Phone;
                    meetupsWorksheet.Cell("B" + (i + 3)).Value = profile.Email;
                    meetupsWorksheet.Cell("C" + (i + 3)).Value = profile.Fullname;
                    meetupsWorksheet.Cell("D" + (i + 3)).Value = profile.Registration.ToShortDateString();
                    meetupsWorksheet.Cell("E" + (i + 3)).Value = profile.Activity;
                    meetupsWorksheet.Cell("F" + (i + 3)).Value = meetups[i].PositionWork;
                    meetupsWorksheet.Cell("G" + (i + 3)).Value = meetups[i].PlaceWork;
                    meetupsWorksheet.Cell("H" + (i + 3)).Value = meetups[i].Group;
                }

                speakersWorksheet.Cell("A1").Value = "Спикеры";

                speakersWorksheet.Cell("A2").Value = "Телефон";
                speakersWorksheet.Cell("B2").Value = "Почта";
                speakersWorksheet.Cell("C2").Value = "ФИО";
                speakersWorksheet.Cell("D2").Value = "Дата регистрации";
                speakersWorksheet.Cell("E2").Value = "Активация";
                speakersWorksheet.Cell("F2").Value = "Название статьи";
                speakersWorksheet.Cell("G2").Value = "О статье";
                speakersWorksheet.Cell("H2").Value = "Присутствие";

                for (int i = 0; i < speakers.Count; i++)
                {
                    var profile = eventProfiles.Find(a => a.Id.Equals(meetups[i].EventProfileUserId));
                    speakersWorksheet.Cell("A" + (i + 3)).Value = profile.Phone;
                    speakersWorksheet.Cell("B" + (i + 3)).Value = profile.Email;
                    speakersWorksheet.Cell("C" + (i + 3)).Value = profile.Fullname;
                    speakersWorksheet.Cell("D" + (i + 3)).Value = profile.Registration.ToShortDateString();
                    speakersWorksheet.Cell("E" + (i + 3)).Value = profile.Activity;
                    speakersWorksheet.Cell("F" + (i + 3)).Value = speakers[i].ArticleTitle;
                    speakersWorksheet.Cell("G" + (i + 3)).Value = speakers[i].ReportTitle;
                    speakersWorksheet.Cell("H" + (i + 3)).Value = speakers[i].Performance;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"{table}_excel_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }


        ///<summary>
        /// Upload docs file
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /UploadFile
        ///     {
        ///         file = "file"
        ///     }
        /// </remarks>
        /// <returns>Upload docs file</returns>
        /// <response code="200">Excel file</response>
        /// <response code="400">Bad request</response> 
        [HttpPost("UploadFile")]
        public IActionResult post([FromForm] IFormFile file)
        {
            if (file == null)
            {
                return StatusCode(400);
            }
            string filePath = $"SaveFiles/{file.FileName}_{DateTime.Now.Ticks.ToString()}";

            using (var stream = System.IO.File.Create(filePath))
            {
                file.CopyToAsync(stream);
            }
            return StatusCode(200);
        }

        ///<summary>
        /// Send email to user
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /SendEmail
        ///     {
        ///         text = "file",
        ///         email = "89126527199@yandex.ru"
        ///     }
        /// </remarks>
        /// <returns>Send email to user</returns>
        /// <response code="200">Excel file</response>
        /// <response code="400">Bad request</response> 
        [HttpGet("SendEmail")]
        public string SendEmail(string email, string text)
        {
            SendEmail sendEmail = new SendEmail();

            return sendEmail.SendRequest(email, text);
        }


        ///<summary>
        /// Add user for meeting
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /AddMeeting
        ///     {
        ///         email = "89126527199@yandex.ru"
        ///         text = "file",
        ///         phone = "81231321232312",
        ///         fullname = "FUll name",
        ///         regim = "speaker",
        ///         articleTitle = "title",
        ///         reportTitle = "report",
        ///         perfomance = "perfomance",
        ///         placeWork = "Universoty",
        ///         positionWork = "Boss",
        ///         group = "MD-15-1",
        ///     }
        /// </remarks>
        /// <returns>Add user for meeting</returns>
        /// <response code="200">Saved new user</response>
        /// <response code="400">Bad request</response> 
        [HttpPost("AddMeeting")]
        public IActionResult AddMeeting(string email, string text, string phone, string fullname,
            string regim, string articleTitle, string reportTitle, string perfomance, string placeWork,
            string positionWork, string group)
        {
            JObject jres = new JObject();
            try
            {
                //if (!_context.CheckEmailEvent(email))
                //{
                //    return StatusCode(400);
                //}
                EventProfileUser profileUser = new EventProfileUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    Phone = phone,
                    Fullname = fullname
                };
                jres.Add("profile", new JObject(profileUser));
                _context.EventProfileUsers.Add(profileUser);
                switch (regim)
                {
                    case "speaker":
                        var speaker = new Speaker()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EventProfileUserId = profileUser.Id,
                            ArticleTitle = articleTitle,
                            ReportTitle = reportTitle,
                        };
                        switch (perfomance)
                        {
                            case "report":
                                speaker.Performance = Performance.onlyReport;
                                break;
                            case "article":
                                speaker.Performance = Performance.onlyArticle;
                                break;
                            case "all":
                                speaker.Performance = Performance.all;
                                break;
                        }

                        jres.Add("speaker", new JObject(speaker));
                        _context.Speakers.Add(speaker);
                        break;
                    case "meetup":
                        Meetup meetup = new Meetup()
                        {
                            Id = Guid.NewGuid().ToString(),
                            EventProfileUserId = profileUser.Id,
                            PlaceWork = placeWork,
                            PositionWork = positionWork,
                            Group = group,
                        };

                        jres.Add("meetup", new JObject(meetup));
                        _context.Meetups.Add(meetup);
                        break;
                }
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }

            return Ok(jres);
        }
        ///<summary>
        /// Get all users for meeting
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetAll
        /// </remarks>
        /// <returns>Get all users</returns>
        /// <response code="200">Get 3 lists object with users</response>
        /// <response code="400">Bad request</response> 
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            JObject jObject = new JObject();

            try
            {
                //jObject["meetups"] = new JArray(_context.GetAllMeetups());
                //jObject["profiles"] = new JArray(_context.GetAllProfiles());
                //jObject["speakers"] = new JArray(_context.GetAllSpeakers());
            }
            catch (Exception e)
            {
                return StatusCode(400);
            }

            return Ok(jObject);
        }

        ///<summary>
        /// Get only 1 user
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Get
        ///     {
        ///         mode = "meetups" //meetups|profiles|speakers
        ///         id = "meeting-1"
        ///     }
        /// </remarks>
        /// <returns>Send email to user</returns>
        /// <response code="200">Excel file</response>
        /// <response code="400">Bad request</response> 
        [HttpGet("Get")]
        public IActionResult Get(string mode, string id)
        {
            //switch (mode)
            //{
            //    case "meetups":
            //        var meetup = _context.Meetups.Find(a => a.Id.Equals(id));
            //        return Ok(meetup);
            //    case "profiles":
            //        var profile = _context.EventProfileUsers.Find(a => a.Id.Equals(id));
            //        return Ok(profile);
            //    case "speakers":
            //        var speaker = _context.Speakers.Find(a => a.Id.Equals(id));
            //        return Ok(speaker);
            //    default:
            //        return Ok(400);
            //}
            return null;
        }

        ///<summary>
        /// User confirmation by email
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /Get
        ///     {
        ///         mode = "meetups" //meetups|profiles|speakers
        ///         id = "meeting-1"
        ///     }
        /// </remarks>
        /// <returns>Redirect main page</returns>
        /// <response code="200">User confirmation by email</response>
        /// <response code="400">Bad request</response> 
        [HttpGet("Authorization/{token}")]
        public RedirectResult Authorization(string token)
        {
            //EventProfileUser eventProfile = _context.GetEventProfileFromToken(token);

            //Response.Cookies.Append("meetup", eventProfile.TokenHeader);

            return Redirect("/meetup");
        }
        ///<summary>
        /// Creating a token for confirmation via email
        ///</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /CreateToken
        ///     {
        ///         email = "email@email.ru"
        ///     }
        /// </remarks>
        /// <returns>Sending message to email</returns>
        /// <response code="200">Sended message to email</response>
        /// <response code="400">Bad request</response> 
        [HttpPost("CreateToken")]
        public JObject CreateToken(string email)
        {
            // Отправить подтверждение на почту
            JObject jObject = new JObject();
            jObject.Add("status", true);
            jObject.Add("error", null);
            //EventProfileUser profileUser = _context.GetEventProfileFromEmail(email);
            //if (profileUser == null)
            //{
            //    jObject.Add("status", false);
            //    jObject.Add("error", "Such mail is already there");
            //    return jObject;
            //}

            //SendEmail(profileUser.Email, "Подвердите почту перейдя по этой ссылке: ");
            return jObject;
        }
    }
}
