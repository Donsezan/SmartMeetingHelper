using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using File = System.IO.File;
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
            _mainController.Label3TextEvent += Label3TextEventHeandler;
            _mainController.Label4TextEvent += Label4TextEventHeandler;
            _mainController.imageBoxFrameEvent += imageBoxFrameHeandler;
        }
        
        private void button3_Click_1(object sender, EventArgs e)
        {
            label6.Text = _mainController.ConnectUser();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            //Initialize the capture device
            _mainController.grabber = new Capture();
            _mainController.grabber.QueryFrame();
            //Initialize the FrameGraber event
            //Application.Idle += new EventHandler(FrameGrabber);
            Application.Idle += _mainController.FaceRecognition.FrameGrabber;
            button1.Enabled = false;
        }


        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Trained face counter
                _mainController.ContTrain = _mainController.ContTrain + 1;

                //Get a gray frame from capture device
                _mainController.gray = _mainController.grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = _mainController.gray.DetectHaarCascade(
                    _mainController.face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    _mainController.TrainedFace = _mainController.currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                _mainController.TrainedFace = _mainController.result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                _mainController.trainingImages.Add(_mainController.TrainedFace);
                _mainController.labels.Add(textBox1.Text);

                //Show face added in gray scale
                imageBox1.Image = _mainController.TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", _mainController.trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < _mainController.trainingImages.ToArray().Length + 1; i++)
                {
                    _mainController.trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", _mainController.labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void Label3TextEventHeandler(string text)
        {
            label3.Text = text;
        }
        public void Label4TextEventHeandler(string text)
        {
            label4.Text = text;
        }

        public void imageBoxFrameHeandler(Image<Bgr, Byte> image)
        {
            imageBoxFrameGrabber.Image = image;
        }
    }
}