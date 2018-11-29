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



        public async void UpdateUserInfo(UserModel user)
        {
            _oldUser = user;
            CheckUser(user);
            if (!_requestEmail && _shouldLoadNewEmail)
            {
                if (user?.Id != null)
                {
                    var result = new CalendarEventModel();
                    _requestEmail = true;
                    try
                    {
                        result = await Calendar.GetUserCalendar(user.Email);
                    }
                    catch
                    {
                        result.Subject = "---";
                        result.MeetingTime = "---";
                    }

                    UpdateEmailSectionLabels(result);
                    WaitFor();
                }
            }

            UpdateUserLabelsEvent?.Invoke(user);
        }

        private async Task WaitFor()
        {
            await Task.Delay(3000);
            _requestEmail = false;
            _shouldLoadNewEmail = false;
        }
    }
}