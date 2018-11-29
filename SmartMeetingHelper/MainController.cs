using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emgu.CV;
using SmartMeetingHelper.Helpers;
using SmartMeetingHelper.Logic;
using SmartMeetingHelper.Models;
using EventHandler = SmartMeetingHelper.Logic.EventHandler;

namespace SmartMeetingHelper
{
    public class MainController : EventHandler
    {
        public Capture Grabber;
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        private FaceRecognition _faceRecognition;
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

        public async Task<string> ConnectUser()
        {
            var graphClient = Authentication.GetAuthenticatedClient();
            var currentUserObject = await graphClient.Me.Request().GetAsync();
            return currentUserObject.DisplayName;
            
            //DisconnectButton.IsEnabled = true;
            //ConnectButton.IsEnabled = false;

        }
      
        public void FrameGrabber(object sender, EventArgs e)
        {
            _faceRecognition.FrameGrabber();
        }
        public void AddFace()
        {
            _faceRecognition.AddFace();
        }

        public void AddUserToDb(UserModel userModel)
        {
            _sqlHelper.AddUserToDb(userModel);
        }




    }
}