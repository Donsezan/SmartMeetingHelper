using System;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using SmartMeetingHelper.Models;

namespace SmartMeetingHelper.Logic
{
    public class EventHandler
    {
        public delegate void TextDelegate(string message);

        public delegate void UserDelegate(UserModel userModel);

        public delegate void UserCalendar(CalendarEventModel calendarEvent);

        public delegate void ImageBgrBoxFrameDelegate(Image<Bgr, byte> image);

        public delegate void ImageGrayBoxFrameDelegate(Image<Gray, byte> image);

        public event TextDelegate AmountOfDetectedFaceLabelEvent;
        public event TextDelegate TimeLeftCounterLabelEvent;
        public event ImageBgrBoxFrameDelegate ImageBoxFrameEvent;
        public event ImageGrayBoxFrameDelegate TrainedImageBoxEvent;
        public event UserDelegate UpdateUserLabelsEvent;
        public event UserCalendar UpdateEmailSectionLabelsEvent;
        public event Action GetEmailFromEmailTextBoxEvent;
        public event Action GetNameFromTextBoxEvent;
        public string CurrentUserName;
        public string CurrentUserEmail;

        public string GetNameFromTextBox()
        {
            GetNameFromTextBoxEvent?.Invoke();
            return CurrentUserName;
        }

        public string GetEmeilFromEmeilTextBox()
        {
            GetEmailFromEmailTextBoxEvent?.Invoke();
            return CurrentUserEmail;
        }

        public void UpdateTimeLeftCounterLabel(string timeLeft)
        {
            TimeLeftCounterLabelEvent?.Invoke(timeLeft);
        }


        public void UpdateAmountOfDetectedFaceLabel(string text)
        {
            AmountOfDetectedFaceLabelEvent?.Invoke(text);
        }

        public void UpdateImageBoxFrame(Image<Bgr, byte> image)
        {
            ImageBoxFrameEvent?.Invoke(image);
        }

        public void UpdateTrainedImageBox(Image<Gray, byte> image)
        {
            TrainedImageBoxEvent?.Invoke(image);
        }

        public void UpdateEmailSectionLabels(CalendarEventModel calendarEvent)
        {
            UpdateEmailSectionLabelsEvent?.Invoke(calendarEvent);
        }

        private bool _requestEmail;
        private UserModel _oldUser;
        private int _userCounter;
        private bool _shouldLoadNewEmail;

        private void CheckUser(UserModel user)
        {
            if (!_shouldLoadNewEmail)
            {
                if (_oldUser == user)
                {
                    _userCounter++;
                }
                else
                {
                    _userCounter = 0;
                }

                if (_userCounter > 30)
                {
                    _shouldLoadNewEmail = true;
                }
            }
        }

        private readonly CalendarEventModel _resultStub = new CalendarEventModel
        {
            Subject = "---",
            MeetingTime = "---"
        };

        public async void UpdateUserInfo(UserModel user)
        {
            _oldUser = user;
            CheckUser(user);
            if (!_requestEmail && _shouldLoadNewEmail)
            {
                if (user?.Id != null)
                {
                    CalendarEventModel result;
                    _requestEmail = true;
                    try
                    {
                        result = await Calendar.GetUserCalendar(user.Email);
                    }
                    catch
                    {
                        result = _resultStub;
                    }

                    UpdateEmailSectionLabels(result);
                    UpdateTimeLeftCounterLabel(GetleftTimeToevent(result.MeetingTime));
                    WaitFor();
                }
            }
            UpdateUserLabelsEvent?.Invoke(user);
            
        }
        
        private async void WaitFor()
        {
            await Task.Delay(5000);
            _requestEmail = false;
            _shouldLoadNewEmail = false;
            UpdateEmailSectionLabels(_resultStub);
            UpdateTimeLeftCounterLabel("---");
        }

        private string GetleftTimeToevent(string eventTime)
        {
            try
            {
                var dateStr = eventTime.Split('-')[0];
                dateStr = dateStr.Substring(0, dateStr.Length - 1);
                var dateTime = DateTime.ParseExact(dateStr, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None);
                var sd = dateTime.Subtract(DateTime.Now);
                return sd.ToString().Split('.')[0];
            }
            catch (Exception e)
            {
                return "---";
            }
          
        }

    }
}