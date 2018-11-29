using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Graph;
using SmartMeetingHelper.Models;

namespace SmartMeetingHelper.Logic
{
    public static class Calendar
    {
        public static async Task<bool> SendReplyAllMessageAsync(string body, string subject, bool? isOrganizer)
        {
            bool emailSent = false;

            try
            {
                GraphServiceClient graphClient = Authentication.GetAuthenticatedClient();
                // Get meeting message from user's Sent Items folder or Inbox
                IMailFolderMessagesCollectionPage eventMessages;

                if (isOrganizer.GetValueOrDefault())
                {
                    eventMessages = await graphClient.Me.MailFolders.SentItems.Messages.Request().Filter("Subject eq '" + subject + "'").GetAsync();
                }
                else
                {
                    eventMessages = await graphClient.Me.MailFolders.Inbox.Messages.Request().Filter("Subject eq '" + subject + "'").GetAsync();
                }

                if (eventMessages.Count > 0)
                {
                    Message messageToReplyAll = eventMessages[0];

                    // Reply all to message
                    Message replyMessage = await graphClient.Me.Messages[messageToReplyAll.Id].CreateReplyAll().Request().PostAsync();

                    if (!String.IsNullOrEmpty(body))
                    {
                        ItemBody replyMessageBody = new ItemBody { ContentType = BodyType.Text, Content = body };
                        replyMessage.Body = replyMessageBody;
                        await graphClient.Me.Messages[replyMessage.Id].Request().UpdateAsync(replyMessage);
                    }
                    await graphClient.Me.Messages[replyMessage.Id].Send().Request().PostAsync();
                    emailSent = true;
                }

            }

            catch (ServiceException e)
            {
                Debug.WriteLine("Failed to send message" + e.Error.Message);
                emailSent = false;
            }

            return emailSent;
        }
        
        public static async Task<IUserCalendarViewCollectionPage> GetDayEventsAsync(string startDateTime, string endDateTime, string emeil)
        {
            IUserCalendarViewCollectionPage events = null;

            try
            {
                GraphServiceClient graphClient = Authentication.GetAuthenticatedClient();
                string localTimeZone = TimeZoneInfo.Local.Id;

                List<Option> options = new List<Option>();
                options.Add(new QueryOption("StartDateTime", startDateTime));
                options.Add(new QueryOption("EndDateTime", endDateTime));
                options.Add(new HeaderOption("Prefer", "outlook.timezone=\"" + localTimeZone + "\""));

                //events = await graphClient.Me.CalendarView.Request(options).OrderBy("start/DateTime").GetAsync();
                return await graphClient.Users[emeil].CalendarView.Request(options).OrderBy("start/DateTime").GetAsync();
            }

            catch (ServiceException e)
            {
                Debug.WriteLine("Get events failed: " + e.Error.Message);
                return null;
            }
        }

        public static CalendarEventModel GetEvent(IUserCalendarViewCollectionPage events)
        {
            if (events.Count > 0)
            {
                var eventModel = new CalendarEventModel
                {
                    Subject = events[0].Subject,
                    MeetingTime = events[0].Start.DateTime.Substring(0, 16) + " - " +
                                  events[0].End.DateTime.Substring(0, 16),
                    Participants = events[0].Attendees
                };
                return eventModel;
            }
            return null;
        }
        public static async Task<CalendarEventModel> GetUserCalendar(string email)
        {
            var startTime = DateTime.Now.ToString("s");
            var endTime = DateTime.Now.AddDays(1).AddTicks(-1).ToString("s");
            var sd = await Calendar.GetDayEventsAsync(startTime, endTime, email);
            var model = Calendar.GetEvent(sd);
            return model;
        }
    }
}