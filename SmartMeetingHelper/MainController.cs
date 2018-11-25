using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using SmartMeetingHelper.Helpers;
using SmartMeetingHelper.Logic;
using SmartMeetingHelper.Models;

namespace SmartMeetingHelper
{
    public class MainController
    {
        //Declararation of all variables, vectors and haarcascades
        public Image<Bgr, Byte> currentFrame;
        public Capture grabber;
        public HaarCascade face;
        public HaarCascade eye;
        public Image<Gray, byte> result, TrainedFace = null;
        public Image<Gray, byte> gray = null;
        public List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        public List<string> labels = new List<string>();
        public List<string> NamePersons = new List<string>();
        public int ContTrain, NumLabels, t;
        public string name, names = null;
        private readonly FileHelper _fileHelper = new FileHelper();
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        
        public void InitParams()
        {
            var user = new UserModel
            {
                Id = "dfs",
                Name = "Nik",
                Email = "tue@et.rr",
                PhotoId = "q113wf.jpg",
                LastVisit = "21.23.55"
            };
            _fileHelper.CreateFolderTrainedFaces();
            _fileHelper.copyFiles();
            _sqlHelper.CreateBase();
            _sqlHelper.AddUserToDb(user);
            var us = _sqlHelper.FoundInDbModel("dfs");

            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels+1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }
            
            }
            catch(Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public string ConnectUser()
        {
            var graphClient = AuthenticationHelper.GetAuthenticatedClient();
            var currentUserObject = graphClient.Me.Request().GetAsync();
            currentUserObject.Wait();
            return currentUserObject.Result.DisplayName;
            //DisconnectButton.IsEnabled = true;
            //ConnectButton.IsEnabled = false;

        }




    }
}