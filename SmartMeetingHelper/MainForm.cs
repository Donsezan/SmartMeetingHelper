using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using System.Diagnostics;
using Emgu.CV.CvEnum;
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
            Application.Idle += new EventHandler(FrameGrabber);
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


        void FrameGrabber(object sender, EventArgs e)
        {
            label3.Text = "0";
            //label4.Text = "";
            _mainController.NamePersons.Add("");


            //Get the current frame form capture device
            _mainController.currentFrame = _mainController.grabber.QueryFrame()
                .Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            _mainController.gray = _mainController.currentFrame.Convert<Gray, Byte>();

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
                _mainController.t = _mainController.t + 1;
                _mainController.result = _mainController.currentFrame.Copy(f.rect)
                    .Convert<Gray, byte>()
                    .Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                _mainController.currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (_mainController.trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(_mainController.ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                        _mainController.trainingImages.ToArray(),
                        _mainController.labels.ToArray(),
                        3000,
                        ref termCrit);

                    _mainController.name = recognizer.Recognize(_mainController.result);

                    //Draw the label for each face detected and recognized

                    MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
                    _mainController.currentFrame.Draw(_mainController.name, ref font,
                        new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));

                }

                _mainController.NamePersons[_mainController.t - 1] = _mainController.name;
                _mainController.NamePersons.Add("");


                //Set the number of faces detected on the scene
                label3.Text = facesDetected[0].Length.ToString();

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
            _mainController.t = 0;

            //Names concatenation of persons recognized
            for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                _mainController.names = _mainController.names + _mainController.NamePersons[nnn] + ", ";
            }
            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = _mainController.currentFrame;
            label4.Text = _mainController.names;
            _mainController.names = "";
            //Clear the list(vector) of names
            _mainController.NamePersons.Clear();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("Donate.html");
        }

    }
}