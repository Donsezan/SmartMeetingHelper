using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using SmartMeetingHelper.Helpers;
using SmartMeetingHelper.Logic;
using SmartMeetingHelper.Models;
using Calendar = SmartMeetingHelper.Logic.Calendar;

namespace SmartMeetingHelper
{
    public class MainController
    {
        public Capture Grabber;
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        private FaceRecognition _faceRecognition;
        public delegate void TextDelegate(string message);
        public delegate void UserDelegate(UserModel userModel);
        public delegate void ImageBgrBoxFrameDelegate(Image<Bgr, byte> image);
        public delegate void ImageGrayBoxFrameDelegate(Image<Gray, byte> image);
        public event TextDelegate AmountOfDetectedFaceLabelEvent;
        public event ImageBgrBoxFrameDelegate ImageBoxFrameEvent;
        public event ImageGrayBoxFrameDelegate TrainedImageBoxEvent;
        public event UserDelegate UpdateUserLablesEvent;
        public event Action GetEmeilFromEmeilTextBoxEvent;
        public event Action GetNameFromTextBoxEvent;
        public string CurrentUserName;
        public string CurrentUserEmeil;
        public List<UserModel> UserModelsList = new List<UserModel>();

        public void InitParams()
        {
            FileHelper.CreateFolderTrainedFaces();
            FileHelper.CopyFiles();

            _faceRecognition = new FaceRecognition(this);
            if (!FileHelper.CheckIfDbExist())
            {
                _sqlHelper.CreateBase();
            }
            UserModelsList = _sqlHelper.GetAllDataFromDb();
            _faceRecognition.LoadFaces();
        }

        public string ConnectUser()
        {
            var graphClient = Authentication.GetAuthenticatedClient();
            var currentUserObject = graphClient.Me.Request().GetAsync();
            currentUserObject.Wait();
            //var startTime = DateTime.Now.ToString("o");
            //var endTime = DateTime.Now.AddHours(20).ToString("o");
            //var sd = Calendar.GetDayEventsAsync(startTime, endTime, "donsezan@outlook.com");
            ////sd.Wait();donsezan@outlook.com
            //var model = Calendar.GetEvent(sd.GetResult());
            return currentUserObject.Result.DisplayName;
            
            //DisconnectButton.IsEnabled = true;
            //ConnectButton.IsEnabled = false;

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

        public void FrameGrabber(object sender, EventArgs e)
        {
            _faceRecognition.FrameGrabber();
        }
        public void AddFace()
        {
            _faceRecognition.AddFace();
        }
        public string GetNameFromTextBox()
        {
            GetNameFromTextBoxEvent?.Invoke();
            return CurrentUserName;
        }

        public string GetEmeilFromEmeilTextBox()
        {
            GetEmeilFromEmeilTextBoxEvent?.Invoke();
            return CurrentUserEmeil;
        }

        public void AddUserToDb(UserModel userModel)
        {
            _sqlHelper.AddUserToDb(userModel);
        }
        
        public void UpdateUserInfo(UserModel user)
        {
            UpdateUserLablesEvent?.Invoke(user);
        }
    }
}