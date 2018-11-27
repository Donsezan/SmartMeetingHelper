using System;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using SmartMeetingHelper.Helpers;
using SmartMeetingHelper.Logic;
using SmartMeetingHelper.Models;

namespace SmartMeetingHelper
{
    public class MainController
    {
        public Capture Grabber;
        //private readonly FileHelper _fileHelper = new FileHelper();
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        private FaceRecognition _faceRecognition;
        public delegate void TextDelegate(string message);
        public delegate void ImageBgrBoxFrameDelegate(Image<Bgr, byte> image);
        public delegate void ImageGrayBoxFrameDelegate(Image<Gray, byte> image);
        public event TextDelegate AmountOfDetectedFaceLabelEvent;
        public event TextDelegate RecognizedNameLabelEvent;
        public event ImageBgrBoxFrameDelegate ImageBoxFrameEvent;
        public event ImageGrayBoxFrameDelegate TrainedImageBoxEvent;
        public event Action GetNameFromTextBoxEvent;
        public string CurrentName;
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
            return currentUserObject.Result.DisplayName;
            //DisconnectButton.IsEnabled = true;
            //ConnectButton.IsEnabled = false;

        }

        public void UpdateAmountOfDetectedFaceLabel(string text)
        {
            AmountOfDetectedFaceLabelEvent?.Invoke(text);
        }
        public void UpdateRecognizedNameLabel(string text)
        {
            RecognizedNameLabelEvent?.Invoke(text);
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
            return CurrentName;
        }
        public void AddUserToDb(UserModel userModel)
        {
            _sqlHelper.AddUserToDb(userModel);
        }
    }
}