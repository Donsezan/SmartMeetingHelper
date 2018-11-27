﻿using System;
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
        public Image<Bgr, Byte> currentFrame;
       
        private readonly HaarCascade _face;
        public HaarCascade eye;
        public Image<Gray, byte> result, TrainedFace = null;
        public Image<Gray, byte> gray = null;
        public List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        //public List<string> labels = new List<string>();
        public List<string> NamePersons = new List<string>();
        public int ContTrain, t;
        public string name, names = null;
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
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = _mainController.Grabber.QueryFrame()
                .Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                _face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect)
                    .Convert<Gray, byte>()
                    .Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                        trainingImages.ToArray(),
                        _mainController.UserModelsList,
                        3000,
                        ref termCrit);

                    name = recognizer.Recognize(result).Name;

                    //Draw the label for each face detected and recognized

                    MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
                    currentFrame.Draw(name, ref font,
                        new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                NamePersons[t - 1] = name;
                NamePersons.Add("");


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
            t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
            }
            //Show the faces procesed and recognized
            _mainController.UpdateImageBoxFrame( currentFrame);
            //label4.Text = _mainController.names;
            _mainController.UpdateRecognizedNameLabel(names);
            names = "";
            //Clear the list(vector) of names
            NamePersons.Clear();

        }

        public void AddFace()
        {
            try
            {
                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = _mainController.Grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                    _face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                //Show face added in gray scale
                _mainController.UpdateTrainedImageBox(TrainedFace);


                var user = new UserModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = _mainController.GetNameFromTextBox(),
                    Email = "Test@test.com",
                    LastVisit = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss")
                };
                var photoName = user.Id + ".bmp";
                trainingImages.ToArray()[0].Save(Application.StartupPath + "/TrainedFaces/"+ photoName);
                user.PhotoId = photoName;

                ////Write the number of triained faces in a file text for further load
                //File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                ////Write the labels of triained faces in a file text for further load
                //for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                //{
                //    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                //    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                //}

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
                        trainingImages.Add(new Image<Gray, byte>(photoPath));
                        ContTrain++;
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