using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using SmartMeetingHelper.Models;

namespace SmartMeetingHelper.Logic
{
    public class FaceRecognition
    {
        private readonly MainController _mainController;
        //Declararation of all variables, vectors and haarcascades
        public Image<Bgr, byte> CurrentFrame;
       
        private readonly HaarCascade _face;
        public HaarCascade eye;
        public Image<Gray, byte> Result, TrainedFace = null;
        public Image<Gray, byte> Gray = null;
        public List<Image<Gray, byte>> TrainingImages = new List<Image<Gray, byte>>();
        //public int ContTrain;
        private UserModel _recognizetUser = new UserModel();
    
        public FaceRecognition(MainController mainController)
        {
            _mainController = mainController;
            _face = new HaarCascade("haarcascade_frontalface_default.xml");
            //ToDO Add Eye and Smile  
        }

        public void FrameGrabber()
        {
            _mainController.UpdateAmountOfDetectedFaceLabel("0");
            //label3.Text = "0";
            //label4.Text = "";


            //Get the current frame form capture device
            CurrentFrame = _mainController.Grabber.QueryFrame()
                .Resize(320, 240, INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            Gray = CurrentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = Gray.DetectHaarCascade(
                _face,
                1.2,
                10,
                HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                Result = CurrentFrame.Copy(f.rect)
                    .Convert<Gray, byte>()
                    .Resize(100, 100, INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                CurrentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (TrainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(_mainController.UserModelsList.Count, 0.001);

                    //Eigen face recognizer
                    var recognizer = new EigenObjectRecognizer(
                        TrainingImages.ToArray(),
                        _mainController.UserModelsList,
                        3000,
                        ref termCrit);

                    _recognizetUser = recognizer.Recognize(Result);
                    if (_recognizetUser != null)
                    {
                        //Draw the label for each face detected and recognized

                        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
                        CurrentFrame.Draw(_recognizetUser.Name, ref font,
                            new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));
                    }
                }

                //NamePersons[t - 1] = lableName;
                //NamePersons.Add("");


                //Set the number of faces detected on the scene
                _mainController.UpdateAmountOfDetectedFaceLabel(facesDetected[0].Length.ToString());
                //label3.Text = facesDetected[0].Length.ToString();

                /*
                //Set the region of interest on the faces
                
                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */

            }

            //Names concatenation of persons recognized
            //for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            //{
            //    lableName = lableName + NamePersons[nnn] + ", ";
            //}
            //Show the faces procesed and recognized
            _mainController.UpdateImageBoxFrame( CurrentFrame);
            //label4.Text = _mainController.names;
            if (_recognizetUser != null)
            {
                _mainController.UpdateRecognizedNameLabel(_recognizetUser.Name);
                _mainController.UpdateRecognizedEmeilLabel(_recognizetUser.Email);
                _mainController.UpdateRecognizedlastVisitLabel(_recognizetUser.LastVisit);
                _recognizetUser = null;
            }
            else
            {
                _mainController.UpdateRecognizedNameLabel("---");
                _mainController.UpdateRecognizedEmeilLabel("---");
                _mainController.UpdateRecognizedlastVisitLabel("---");
            }
            //  names = "";
            //Clear the list(vector) of names
            //NamePersons.Clear();
           

        }

        public void AddFace()
        {
            try
            {
                //Trained face counter

                //Get a gray frame from capture device
                Gray = _mainController.Grabber.QueryGrayFrame().Resize(320, 240, INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = Gray.DetectHaarCascade(
                    _face,
                1.2,
                10,
                HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = CurrentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = Result.Resize(100, 100, INTER.CV_INTER_CUBIC);
                TrainingImages.Add(TrainedFace);
                //Show face added in gray scale
                _mainController.UpdateTrainedImageBox(TrainedFace);


                var user = new UserModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = _mainController.GetNameFromTextBox(),
                    Email = _mainController.GetEmeilFromEmeilTextBox(),
                    LastVisit = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss")
                };

                var photoName = user.Id + ".bmp";
                TrainingImages.ToArray()[TrainingImages.Count-1].Save(Application.StartupPath + "/TrainedFaces/"+ photoName);
                user.PhotoId = photoName;

                MessageBox.Show(_mainController.GetNameFromTextBox() + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _mainController.UserModelsList.Add(user);
                _mainController.AddUserToDb(user);

            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void LoadFaces()
        {
            try
            {
                ////Load of previus trainned faces and labels for each image
                foreach (var userModel in _mainController.UserModelsList)
                {
                    var photoPath = Application.StartupPath + "/TrainedFaces/" + userModel.PhotoId;
                    if (FileHelper.CheckIfFileExist(photoPath))
                    {
                        TrainingImages.Add(new Image<Gray, byte>(photoPath));
                    }
                    else
                    {
                        MessageBox.Show($"Missed photo user.\r\nId: {userModel.Id} \r\nname: {userModel.Name} \r\nemeil: {userModel.Email}", @"User photo missed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        _mainController.UserModelsList.Remove(userModel);
                    }
                    
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show(@"Nothing in database, please add at least a face(Simply train the prototype with the Add Face Button).", @"Trained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}