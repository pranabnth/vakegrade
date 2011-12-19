using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using HTL.Grieskirchen.VaKEGrade.Database;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Web.UI.WebControls;


namespace HTL.Grieskirchen.VaKEGrade.Utility
{
    public class ImageGenerator
    {

        public static iTextSharp.text.Image GenerateImage(Pupil pupil)
        {

           

            Chart chart = new Chart();
            chart.RenderType = RenderType.ImageTag;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            chart.Titles.Add(pupil.FirstName + " " + pupil.LastName);
            chart.Titles[0].Font = new System.Drawing.Font("Arial", 16f);

            List<Subject> subjects = (from grade in pupil.Grades
                                  select grade.SubjectArea.Subject).Distinct().ToList();
            
            foreach (Subject subject in subjects)
            {
                ChartArea chartArea = new ChartArea(subject.Name);
                chartArea.AxisX = new Axis();
                chartArea.AxisY = new Axis();
                chartArea.AxisY.Maximum = 5;
                chartArea.AxisY.Minimum = 0;
                chartArea.AxisX.TitleFont = new System.Drawing.Font("Arial", 12f);
                chartArea.AxisY.TitleFont = new System.Drawing.Font("Arial", 12f);
                chartArea.AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 10f);
                chartArea.AxisX.LabelStyle.Angle = -90;
                chartArea.BackColor = System.Drawing.Color.White;

                Series series = new Series("");
                series.ChartType = SeriesChartType.Column;
                series.XValueType = ChartValueType.String;
                series.YValueType = ChartValueType.Int32;

                chart.Series.Add("");
                foreach (Grade grade in pupil.Grades)
                {
                    series.Points.AddXY(grade.SubjectArea.Name, grade.Value);
                }

                chart.ChartAreas.Add(chartArea);
                chart.Series.Add(series);
            }
            chart.Width = Unit.Pixel(400);
            chart.Height = Unit.Pixel(200);
            MemoryStream stream = new MemoryStream();
            chart.SaveImage(stream);
            stream.Seek(0,SeekOrigin.Begin);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(stream);
            stream.Close();

            return img;
            
        }

    }
}