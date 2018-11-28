namespace SmartMeetingHelper
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AddFaceButton = new System.Windows.Forms.Button();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EmeilTextBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.EmeilLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TrainedImageBox = new Emgu.CV.UI.ImageBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.recognizetlastVisitLable = new System.Windows.Forms.Label();
            this.RecognizetEmeilLabel = new System.Windows.Forms.Label();
            this.PresentInTheSceneLabel = new System.Windows.Forms.Label();
            this.RecognizedNameLabel = new System.Windows.Forms.Label();
            this.AmountOfDetectedFaceLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DetectAndRecognizeButton = new System.Windows.Forms.Button();
            this.imageBoxFrameGrabber = new Emgu.CV.UI.ImageBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrainedImageBox)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).BeginInit();
            this.SuspendLayout();
            // 
            // AddFaceButton
            // 
            this.AddFaceButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.AddFaceButton.Location = new System.Drawing.Point(76, 201);
            this.AddFaceButton.Name = "AddFaceButton";
            this.AddFaceButton.Size = new System.Drawing.Size(56, 40);
            this.AddFaceButton.TabIndex = 3;
            this.AddFaceButton.Text = " Add face";
            this.AddFaceButton.UseVisualStyleBackColor = true;
            this.AddFaceButton.Click += new System.EventHandler(this.AddFaceButton_Click);
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(55, 151);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(78, 20);
            this.NameTextBox.TabIndex = 7;
            this.NameTextBox.Text = "Sergio";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EmeilTextBox);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.EmeilLabel);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.NameTextBox);
            this.groupBox1.Controls.Add(this.TrainedImageBox);
            this.groupBox1.Controls.Add(this.AddFaceButton);
            this.groupBox1.Location = new System.Drawing.Point(342, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(140, 249);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Training: ";
            // 
            // EmeilTextBox
            // 
            this.EmeilTextBox.Location = new System.Drawing.Point(55, 176);
            this.EmeilTextBox.Name = "EmeilTextBox";
            this.EmeilTextBox.Size = new System.Drawing.Size(78, 20);
            this.EmeilTextBox.TabIndex = 10;
            this.EmeilTextBox.Text = "test@test.com";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 201);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(64, 39);
            this.button3.TabIndex = 10;
            this.button3.Text = "Get Name";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // EmeilLabel
            // 
            this.EmeilLabel.AutoSize = true;
            this.EmeilLabel.Location = new System.Drawing.Point(9, 179);
            this.EmeilLabel.Name = "EmeilLabel";
            this.EmeilLabel.Size = new System.Drawing.Size(38, 13);
            this.EmeilLabel.TabIndex = 9;
            this.EmeilLabel.Text = "Emeil: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 151);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name: ";
            // 
            // TrainedImageBox
            // 
            this.TrainedImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TrainedImageBox.Location = new System.Drawing.Point(11, 15);
            this.TrainedImageBox.Name = "TrainedImageBox";
            this.TrainedImageBox.Size = new System.Drawing.Size(120, 130);
            this.TrainedImageBox.TabIndex = 5;
            this.TrainedImageBox.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.recognizetlastVisitLable);
            this.groupBox2.Controls.Add(this.RecognizetEmeilLabel);
            this.groupBox2.Controls.Add(this.PresentInTheSceneLabel);
            this.groupBox2.Controls.Add(this.RecognizedNameLabel);
            this.groupBox2.Location = new System.Drawing.Point(488, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 247);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Results: ";
            // 
            // recognizetlastVisitLable
            // 
            this.recognizetlastVisitLable.AutoSize = true;
            this.recognizetlastVisitLable.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recognizetlastVisitLable.ForeColor = System.Drawing.Color.Blue;
            this.recognizetlastVisitLable.Location = new System.Drawing.Point(9, 114);
            this.recognizetlastVisitLable.Name = "recognizetlastVisitLable";
            this.recognizetlastVisitLable.Size = new System.Drawing.Size(73, 19);
            this.recognizetlastVisitLable.TabIndex = 19;
            this.recognizetlastVisitLable.Text = "Last Visit";
            // 
            // RecognizetEmeilLabel
            // 
            this.RecognizetEmeilLabel.AutoSize = true;
            this.RecognizetEmeilLabel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecognizetEmeilLabel.ForeColor = System.Drawing.Color.Blue;
            this.RecognizetEmeilLabel.Location = new System.Drawing.Point(9, 81);
            this.RecognizetEmeilLabel.Name = "RecognizetEmeilLabel";
            this.RecognizetEmeilLabel.Size = new System.Drawing.Size(47, 19);
            this.RecognizetEmeilLabel.TabIndex = 18;
            this.RecognizetEmeilLabel.Text = "Emeil";
            // 
            // PresentInTheSceneLabel
            // 
            this.PresentInTheSceneLabel.AutoSize = true;
            this.PresentInTheSceneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PresentInTheSceneLabel.ForeColor = System.Drawing.Color.Black;
            this.PresentInTheSceneLabel.Location = new System.Drawing.Point(9, 23);
            this.PresentInTheSceneLabel.Name = "PresentInTheSceneLabel";
            this.PresentInTheSceneLabel.Size = new System.Drawing.Size(197, 15);
            this.PresentInTheSceneLabel.TabIndex = 17;
            this.PresentInTheSceneLabel.Text = "Persons present in the scene:";
            // 
            // RecognizedNameLabel
            // 
            this.RecognizedNameLabel.AutoSize = true;
            this.RecognizedNameLabel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecognizedNameLabel.ForeColor = System.Drawing.Color.Blue;
            this.RecognizedNameLabel.Location = new System.Drawing.Point(9, 53);
            this.RecognizedNameLabel.Name = "RecognizedNameLabel";
            this.RecognizedNameLabel.Size = new System.Drawing.Size(61, 19);
            this.RecognizedNameLabel.TabIndex = 16;
            this.RecognizedNameLabel.Text = "Nobody";
            // 
            // AmountOfDetectedFaceLabel
            // 
            this.AmountOfDetectedFaceLabel.AutoSize = true;
            this.AmountOfDetectedFaceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AmountOfDetectedFaceLabel.ForeColor = System.Drawing.Color.Red;
            this.AmountOfDetectedFaceLabel.Location = new System.Drawing.Point(303, 236);
            this.AmountOfDetectedFaceLabel.Name = "AmountOfDetectedFaceLabel";
            this.AmountOfDetectedFaceLabel.Size = new System.Drawing.Size(16, 16);
            this.AmountOfDetectedFaceLabel.TabIndex = 15;
            this.AmountOfDetectedFaceLabel.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(116, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Number of faces detected: ";
            // 
            // DetectAndRecognizeButton
            // 
            this.DetectAndRecognizeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.DetectAndRecognizeButton.Location = new System.Drawing.Point(12, 228);
            this.DetectAndRecognizeButton.Name = "DetectAndRecognizeButton";
            this.DetectAndRecognizeButton.Size = new System.Drawing.Size(98, 31);
            this.DetectAndRecognizeButton.TabIndex = 2;
            this.DetectAndRecognizeButton.Text = "1. Detect and recognize";
            this.DetectAndRecognizeButton.UseVisualStyleBackColor = true;
            this.DetectAndRecognizeButton.Click += new System.EventHandler(this.DetectAndRecognizeButton_Click);
            // 
            // imageBoxFrameGrabber
            // 
            this.imageBoxFrameGrabber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBoxFrameGrabber.Location = new System.Drawing.Point(12, 12);
            this.imageBoxFrameGrabber.Name = "imageBoxFrameGrabber";
            this.imageBoxFrameGrabber.Size = new System.Drawing.Size(316, 216);
            this.imageBoxFrameGrabber.TabIndex = 4;
            this.imageBoxFrameGrabber.TabStop = false;
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 262);
            this.Controls.Add(this.AmountOfDetectedFaceLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.DetectAndRecognizeButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.imageBoxFrameGrabber);
            this.Name = "FrmPrincipal";
            this.Text = "SmartMeetingHelper";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrainedImageBox)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddFaceButton;
        private Emgu.CV.UI.ImageBox imageBoxFrameGrabber;
        private Emgu.CV.UI.ImageBox TrainedImageBox;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label PresentInTheSceneLabel;
        private System.Windows.Forms.Label RecognizedNameLabel;
        private System.Windows.Forms.Label AmountOfDetectedFaceLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DetectAndRecognizeButton;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox EmeilTextBox;
        private System.Windows.Forms.Label EmeilLabel;
        private System.Windows.Forms.Label recognizetlastVisitLable;
        private System.Windows.Forms.Label RecognizetEmeilLabel;
    }
}

