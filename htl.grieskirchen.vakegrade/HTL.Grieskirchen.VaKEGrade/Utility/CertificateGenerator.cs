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
        static Font bigFont = FontFactory.GetFont("Arial", 20f, 1);
        static Font normalFont = FontFactory.GetFont("Arial", 14f);
        static Font smallFont = FontFactory.GetFont("Arial", 12f);

        public static MemoryStream GeneratePDF(Teacher teacher, SchoolClass schoolClass, string schoolYear) {
            
            Document document = new Document();
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);
            
            writer.CloseStream = false;
            document.Open();
            document.AddCreationDate();
            document.AddAuthor("VaKEGrade");
            document.AddTitle("Certificate");

            foreach (Pupil pupil in teacher.PrimaryClasses.First().Pupils.OrderBy(x => x.LastName).ToList())
            {
                CertificateGenerator.GenerateCertificate(pupil, schoolClass, schoolYear, ref document);
            }
            document.Close();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static MemoryStream GeneratePDF(Teacher teacher, SchoolClass schoolClass, List<Pupil> pupils, string schoolYear)
        {

            Document document = new Document();
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            writer.CloseStream = false;
            document.Open();
            document.AddCreationDate();
            document.AddAuthor("VaKEGrade");
            document.AddTitle("Certificate");

            pupils = pupils.OrderBy(x => x.LastName).ToList();
            foreach (Pupil pupil in pupils)
            {
                CertificateGenerator.GenerateCertificate(pupil, schoolClass, schoolYear, ref document);
            }
            document.Close();
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private static void GenerateCertificate(Pupil pupil, SchoolClass schoolClass, string schoolYear, ref Document document)
        {
            Paragraph paragraph;
            PdfPTable table;
            PdfPCell cell;



            paragraph = new Paragraph("HAUPTSCHULE 1 RIED IM INNKREIS", FontFactory.GetFont("Arial", 14f, 1));
            paragraph.Alignment = 1;
            document.Add(paragraph);

            table = new PdfPTable(1);
            cell = new PdfPCell(new Paragraph("Brucknerstraße 20, 4910 Ried im Innkreis", FontFactory.GetFont("Arial", 7f, 0)));
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthBottom = 0.5f;
            cell.BorderColorBottom = BaseColor.BLACK;
            cell.PaddingBottom = 5;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            table.WidthPercentage = 100f;
            document.Add(table);

            //paragraph = new Paragraph("Brucknerstraße 20, 4910 Ried im Innkreis", FontFactory.GetFont("Arial", 10f, 0));
            //paragraph.Alignment = 1;
            //paragraph.SpacingAfter = 12;
            //document.Add(paragraph);

            table = new PdfPTable(2);
            cell = new PdfPCell(new Paragraph("Alternative Form der Leistungsbeurteilung gemäß § 78 SchUG", FontFactory.GetFont("Arial", 7f, 0)));
            cell.BorderWidth = 0;
            cell.Padding = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);
            cell = new PdfPCell(new Paragraph("Schuljahr " + schoolYear, FontFactory.GetFont("Arial", 7f, 0)));
            cell.BorderWidth = 0;
            cell.Padding = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);
            table.WidthPercentage = 100f;
            table.SpacingAfter = 10;
            document.Add(table);

            paragraph = new Paragraph("Jahreszeugnis", FontFactory.GetFont("Arial", 24f, 0));
            paragraph.Alignment = 1;
            document.Add(paragraph);

            paragraph = new Paragraph("VaKE-Klasse", FontFactory.GetFont("Arial", 12f, 0));
            paragraph.Alignment = 1;
            paragraph.SpacingAfter = 8;
            document.Add(paragraph);

            paragraph = new Paragraph(pupil.LastName.ToUpper() + " " + pupil.FirstName, FontFactory.GetFont("Arial", 14f, 1));
            paragraph.Alignment = 1;
            paragraph.SpacingAfter = 8;
            document.Add(paragraph);

            paragraph = new Paragraph("geboren am " + pupil.Birthdate.ToShortDateString() + ", Religionsbekenntnis: " + pupil.Religion, FontFactory.GetFont("Arial", 11f, 0));
            paragraph.Alignment = 1;
            paragraph.SpacingAfter = 3;
            document.Add(paragraph);

            paragraph = new Paragraph((pupil.Gender == "m" ? "Schüler" : "Schülerin") + " der Klasse " + schoolClass.Level + schoolClass.Name + " (" + (schoolClass.Level + 4) + ".Schulstufe), Hauptschule", FontFactory.GetFont("Arial", 11f, 0));
            paragraph.Alignment = 1;
            paragraph.SpacingAfter = 3;
            document.Add(paragraph);

            paragraph = new Paragraph("Verhalten in der Schule: ", FontFactory.GetFont("Arial", 12f, 0));
            paragraph.Add(new Chunk("Sehr zufriedenstellend", FontFactory.GetFont("Arial", 11f, 1)));
            paragraph.Alignment = 0;
            paragraph.SpacingAfter = 10;
            document.Add(paragraph);

            document.Add(CertificateGenerator.GenerateImage(pupil));

            if (pupil.BindingSubjectAssignments.Count > 0)
            {
                List<Subject> bindingSubjects = (from vsa in pupil.BindingSubjectAssignments
                                                   select vsa.Subject).ToList();
                string subjects = "";
                for(int i = 0; i < bindingSubjects.Count; i++)
                {
                    subjects += (bindingSubjects.Count > 1 && i == bindingSubjects.Count-1 ? " und " : " ") + "\"" + bindingSubjects[i].Name + "\""+(i < bindingSubjects.Count-2 ? "," : "");
                }
                subjects.Remove(subjects.Length - 1,1);

                paragraph = new Paragraph((pupil.Gender == "m" ? "Er" : "Sie") + " hat an "+(bindingSubjects.Count > 1 ? "den" : "der")+" verbindlichen "+(bindingSubjects.Count > 1 ? "Übungen" : "Übung") + subjects + " teilgenommen.", FontFactory.GetFont("Arial", 9f, 0));
                paragraph.Alignment = 0;
                paragraph.SpacingBefore = 5;
                paragraph.SpacingAfter = 0;
                document.Add(paragraph);
            }

            if (pupil.VoluntarySubjectAssignements.Count > 0)
            {
                List<Subject> voluntarySubjects = (from vsa in pupil.VoluntarySubjectAssignements
                                                   select vsa.Subject).ToList();
                string subjects = "";
                for (int i = 0; i < voluntarySubjects.Count; i++)
                {
                    subjects += (voluntarySubjects.Count > 1 && i == voluntarySubjects.Count - 1 ? " und " : " ") + "\"" + voluntarySubjects[i].Name + "\"" + (i < voluntarySubjects.Count - 2 ? "," : "");
                }
                subjects.Remove(subjects.Length - 1);

                paragraph = new Paragraph((pupil.Gender == "m" ? "Er" : "Sie") + " hat an " + (voluntarySubjects.Count > 1 ? "den" : "der") + " freiwilligen " + (voluntarySubjects.Count > 1 ? "Übungen " : "Übung ") + subjects + " teilgenommen.", FontFactory.GetFont("Arial", 9f, 0));
                paragraph.Alignment = 0;
                paragraph.SpacingBefore = 5;
                paragraph.SpacingAfter = 0;
                document.Add(paragraph);
            }

            paragraph = new Paragraph((pupil.Gender == "m" ? "Er" : "Sie") + " ist gemäß §25 des SchUG zum Aufsteigen in die " + (schoolClass.Level + 1) + ".Klasse (" + (schoolClass.Level + 5) + ".Schulstufe) berechtigt.", FontFactory.GetFont("Arial", 9f, 0));
            paragraph.Alignment = 0;
            paragraph.SpacingBefore = 30;
            paragraph.SpacingAfter = 50;
            document.Add(paragraph);

            paragraph = new Paragraph("Ried im Innkreis, am " + DateTime.Now.ToShortDateString(), FontFactory.GetFont("Arial", 10f, 0));
            paragraph.Alignment = 0;
            paragraph.SpacingAfter = 30;
            document.Add(paragraph);

            paragraph = new Paragraph("R.S.", FontFactory.GetFont("Arial", 10f, 1));
            paragraph.Alignment = 1;
            paragraph.SpacingAfter = 25;
            document.Add(paragraph);

            table = new PdfPTable(2);
            cell = new PdfPCell(new Paragraph("__________________________", FontFactory.GetFont("Arial", 10f, 0)));
            cell.BorderWidth = 0;
            cell.Padding = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("__________________________", FontFactory.GetFont("Arial", 10f, 0)));
            cell.BorderWidth = 0;
            cell.Padding = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Leiter der Schule", FontFactory.GetFont("Arial", 7f, 0)));
            cell.PaddingTop = 3;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Klassenvorstand", FontFactory.GetFont("Arial", 7f, 0)));
            cell.PaddingTop = 3;
            cell.BorderWidth = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph("Max Mustermann", FontFactory.GetFont("Arial", 9f, 0)));
            cell.BorderWidth = 0;
            cell.Padding = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthBottom = 0.5f;
            cell.BorderColorBottom = BaseColor.BLACK;
            cell.PaddingBottom = 12;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cell);

            cell = new PdfPCell(new Paragraph(schoolClass.Teacher.FirstName + " " + schoolClass.Teacher.LastName, FontFactory.GetFont("Arial", 9f, 0)));
            cell.BorderWidth = 0;
            cell.Padding = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;
            cell.BorderWidthBottom = 0.5f;
            cell.BorderColorBottom = BaseColor.BLACK;
            cell.PaddingBottom = 12;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table.AddCell(cell);
            table.WidthPercentage = 100f;
            table.SpacingAfter = 10;
            document.Add(table);

            paragraph = new Paragraph("DVR: 0064351412052\nBeurteilungskriterien: Lernziele (LZ), Mitarbeit (MA)\nBeurteilungsstufen: Sehr gut (1), Gut (2), Befriedigend (3), Genügend (4), Nicht genügend (5), Nicht teilgenommen (0)\nVerhalten in der Schule: Sehr zufriedenstellend, Zufriedenstellend, Wenig zufriedenstellend, Nicht zufriedenstellend\n", FontFactory.GetFont("Arial", 7f, 0));
            paragraph.Add(new Chunk(pupil.LastName.ToUpper() + " " + pupil.FirstName, FontFactory.GetFont("Arial", 7f, 2)));
            paragraph.Alignment = 0;
            document.Add(paragraph);

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