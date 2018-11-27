using System;
using System.IO;
using System.Resources;
using SmartMeetingHelper.Properties;

namespace SmartMeetingHelper
{
    public static class FileHelper
    {
        public static void CopyFiles()
        {
            TryWriteByteFile("opencv_contrib220.dll", Resources.opencv_contrib220);
            TryWriteByteFile("opencv_core220.dll", Resources.opencv_core220);
            TryWriteByteFile("opencv_features2d220.dll", Resources.opencv_features2d220);
            TryWriteByteFile("opencv_ffmpeg220.dll", Resources.opencv_ffmpeg220);
            TryWriteByteFile("opencv_flann220.dll", Resources.opencv_flann220);
            TryWriteByteFile("opencv_gpu220.dll", Resources.opencv_gpu220);
            TryWriteByteFile("opencv_highgui220.dll", Resources.opencv_highgui220);
            TryWriteByteFile("opencv_imgproc220.dll", Resources.opencv_imgproc220);
            TryWriteByteFile("opencv_legacy220.dll", Resources.opencv_legacy220);
            TryWriteByteFile("opencv_ml220.dll", Resources.opencv_ml220);
            TryWriteByteFile("opencv_objdetect220.dll", Resources.opencv_objdetect220);
            TryWriteByteFile("opencv_video220.dll", Resources.opencv_video220);
            TryWriteByteFile("cv110.dll", Resources.cv110);
            TryWriteByteFile("cvaux110.dll", Resources.cvaux110);
            TryWriteByteFile("cvextern.dll", Resources.cvextern);
            TryWriteByteFile("cxcore110.dll", Resources.cxcore110);
            TryWriteByteFile("highgui110.dll", Resources.highgui110);
            TryWriteByteFile("opencv_calib3d220.dll", Resources.opencv_calib3d220);
            TryWriteStringFile("haarcascade_frontalface_default.xml", Resources.haarcascade_frontalface_default);
        }

        private static void TryWriteByteFile(string name,byte[] resource)
        {
            try
            {
                File.WriteAllBytes(name, resource);
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Exception: {e} at moment writing {name} file" );
               
            }
        }
        private static void TryWriteStringFile(string name, string resource)
        {
            try
            {
                File.WriteAllText(name, resource);
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Exception: {e} at moment writing {name} file");
                throw;
            }
        }

        public static void CreateFolderTrainedFaces()
        {
            const string folderName = "TrainedFaces";
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
        }

        public static bool CheckIfDbExist()
        {
            return File.Exists(Settings.DbName);
        }
        public static bool CheckIfFileExist(string file)
        {
            return File.Exists(file);
        }
    }
}