using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using HTL.Grieskirchen.VaKEGrade.Database;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;


namespace HTL.Grieskirchen.VaKEGrade.Utility
{
    public class CertificateGenerator
    {
        static Font bigFont = FontFactory.GetFont("Calibri", 20f, 1);
        static Font normalFont = FontFactory.GetFont("Calibri", 14f);
        static Font smallFont = FontFactory.GetFont("Calibri", 12f);

        public static MemoryStream GeneratePDF(Teacher teacher, SchoolClass schoolClass, string schoolYear) {
            
            Document document = new Document();
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            writer.CloseStream = false;
            document.Open();
            document.AddCreationDate();
            document.AddAuthor("VaKEGrade");
            document.AddTitle("Certificate");

            foreach (Pupil pupil in teacher.PrimaryClasses.First().Pupils)
            {
                CertificateGenerator.GenerateCertificate(pupil,schoolYear, ref document);
            }
            document.Close();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private static void GenerateCertificate(Pupil pupil, string schoolYear, ref Document document) {
            Paragraph paragraph = new Paragraph("Zeugnis", bigFont);
            paragraph.Alignment = 1;
            document.Add(paragraph);
            paragraph = new Paragraph("Schuljahr "+schoolYear, smallFont);
            paragraph.Alignment = 1;
            document.Add(paragraph);
            document.Add(new Paragraph(" "));
            paragraph = new Paragraph(pupil.FirstName + " " + pupil.LastName, normalFont);
            paragraph.Alignment = 1;
            document.Add(paragraph);
            paragraph = new Paragraph(pupil.Religion, smallFont);
            paragraph.Alignment = 1;
            document.Add(paragraph);


            document.Add(CertificateGenerator.GenerateImage(pupil));
            document.NewPage();
        }

        private static iTextSharp.text.Image GenerateImage(Pupil pupil)
        {

           

            Chart chart = new Chart();
            chart.RenderType = RenderType.ImageTag;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Font.Size = 20;
           
            //chart.Titles.Add(pupil.FirstName + " " + pupil.LastName);
            //chart.Titles[0].Font = new System.Drawing.Font("Arial", 16f);

            List<Subject> subjects = VaKEGradeRepository.Instance.GetSubjectsOfPupil(pupil).ToList();
            
            foreach (Subject subject in subjects)
            {
                ChartArea chartArea = new ChartArea(subject.Name);
                chartArea.AxisX = new Axis();
                chartArea.AxisY = new Axis();
                chartArea.AxisY.Maximum = 5;
                chartArea.AxisY.Minimum = 0;
                chartArea.AxisY.CustomLabels.Add(new CustomLabel(0, 1, "5", 0, LabelMarkStyle.None));
                chartArea.AxisY.CustomLabels.Add(new CustomLabel(1, 2, "4", 0, LabelMarkStyle.None));
                chartArea.AxisY.CustomLabels.Add(new CustomLabel(2, 3, "3", 0, LabelMarkStyle.None));
                chartArea.AxisY.CustomLabels.Add(new CustomLabel(3, 4, "2", 0, LabelMarkStyle.None));
                chartArea.AxisY.CustomLabels.Add(new CustomLabel(4, 5, "1", 0, LabelMarkStyle.None));
                chartArea.AxisY.CustomLabels.Add(new CustomLabel(0, 5, subject.Name, 1, LabelMarkStyle.Box));
                //chartArea.AxisX.TitleFont = new System.Drawing.Font("Arial", 18f);
                //chartArea.AxisY.TitleFont = new System.Drawing.Font("Arial", 18f);
                //chartArea.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 18f);
                //chartArea.AxisX.LabelStyle.Angle = -90;
                chartArea.BackColor = System.Drawing.Color.White;

                Series series = new Series("");
                series.ChartType = SeriesChartType.Column;
                series.XValueType = ChartValueType.String;
                series.YValueType = ChartValueType.Int32;
                series.ChartArea = chartArea.Name;

                chart.Series.Add("");
                foreach (Grade grade in VaKEGradeRepository.Instance.GetGradesOfPupil(pupil,subject))
                {
                    series.Points.AddXY(grade.SubjectArea.Name, 6-grade.Value);
                }

                chart.ChartAreas.Add(chartArea);
                chart.Series.Add(series);
            }
            
            
            chart.Width = Unit.Pixel(1000);
            chart.Height = Unit.Pixel(250*subjects.Count);
            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream, ChartImageFormat.Tiff);
            stream.Seek(0,SeekOrigin.Begin);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(stream);
            stream.Close();
            
            img.ScaleAbsolute(500, 125 * subjects.Count);
            return img;
            
        }

    }
}