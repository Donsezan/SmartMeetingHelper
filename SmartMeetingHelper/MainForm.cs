﻿using System;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Capture = Emgu.CV.Capture;

namespace SmartMeetingHelper
{
    public partial class FrmPrincipal : Form
    {
        private readonly MainController _mainController = new MainController();

        public FrmPrincipal()
        {
            InitializeComponent();
            _mainController.InitParams();
            _mainController.AmountOfDetectedFaceLabelEvent += AmountOfDetectedFaceLabelHandler;
            _mainController.RecognizedNameLabelEvent += RecognizedNameLabelHandler;
            _mainController.ImageBoxFrameEvent += imageBoxFrameHandler;
            _mainController.TrainedImageBoxEvent += TrainedImageBoxHandler;
            _mainController.GetNameFromTextBoxEvent += GetNameFromTextBoxHandler;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            label6.Text = _mainController.ConnectUser();
        }

        private void DetectAndRecognizeButton_Click(object sender, EventArgs e)
        {
            //Initialize the capture device
            _mainController.Grabber = new Capture();
            _mainController.Grabber.QueryFrame();
            //Initialize the FrameGraber event
            //Application.Idle += new EventHandler(FrameGrabber);
            Application.Idle += _mainController.FrameGrabber;
            DetectAndRecognizeButton.Enabled = false;
        }

        private void AddFaceButton_Click(object sender, System.EventArgs e)
        {
            _mainController.AddFace();
        }

        public void AmountOfDetectedFaceLabelHandler(string text)
        {
            AmountOfDetectedFaceLabel.Text = text;
        }

        public void RecognizedNameLabelHandler(string text)
        {
            RecognizedNameLabel.Text = text;
        }

        public void imageBoxFrameHandler(Image<Bgr, byte> image)
        {
            imageBoxFrameGrabber.Image = image;
        }

        public void TrainedImageBoxHandler(Image<Gray, byte> image)
        {
            TrainedImageBox.Image = image;
        }

        public void GetNameFromTextBoxHandler()
        {
            _mainController.CurrentName =  NameTextBox.Text;
        }
    }
}