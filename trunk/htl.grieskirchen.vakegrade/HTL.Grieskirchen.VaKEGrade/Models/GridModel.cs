using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;
using HTL.Grieskirchen.VaKEGrade.Database;

namespace HTL.Grieskirchen.VaKEGrade.Models
{
    public class GridModel
    {
        
        JQGrid spfGrid;
        public JQGrid SpfGrid
        {
            get { return spfGrid; }
            set { spfGrid = value; }
        }

        JQGrid pupilGrid;
        public JQGrid PupilGrid
        {
            get { return pupilGrid; }
            set { pupilGrid = value; }
        }

        public GridModel()
        {
            #region PupilGrid
            List<System.Web.Mvc.SelectListItem> genderDropDown = new List<System.Web.Mvc.SelectListItem>();
            genderDropDown.Add(new System.Web.Mvc.SelectListItem() { Text = "Männlich", Value = "m" });
            genderDropDown.Add(new System.Web.Mvc.SelectListItem() { Text = "Weiblich", Value = "w" });
            pupilGrid = new JQGrid
            {
                Columns = new List<JQGridColumn>()
                                 {
                                     new JQGridColumn { DataField = "ID",
                                                        Visible = false,
                                                        PrimaryKey = true,
                                                        Editable = false,
                                                        Width = 50 },                                    
                                     new JQGridColumn { DataField = "LastName",                                                                HeaderText = "Nachname", 
                                                        Editable = true,
                                                        Width = 100 },
                                                        
                                     new JQGridColumn { DataField = "FirstName",                                                               HeaderText = "Vorname", 
                                                        Editable = true,
                                                        Width = 100 },

                                     new JQGridColumn { DataField = "Religion",                                                                HeaderText = "Religion", 
                                                        Editable = true,
                                                        Width = 100 },

                                     new JQGridColumn { DataField = "Birthdate",                                                               HeaderText = "Geburtsdatum",
                                                        Editable = true,
                                                        Width = 100, 
                                                        EditType = Trirand.Web.Mvc.EditType.DatePicker,
                                                        EditorControlID = "DatePicker",
                                                        DataFormatString = "{0:dd/MM/yyyy}" },
                                     new JQGridColumn { DataField = "Gender",                                                                  HeaderText = "Geschlecht", 
                                                        Editable = true,
                                                        EditType = Trirand.Web.Mvc.EditType.DropDown,
                                                        EditList = genderDropDown,
                                                        Width = 75 }                           
                                 },
                Width = Unit.Pixel(640),
                Height = Unit.Percentage(100),
            };
            pupilGrid.ToolBarSettings.ShowAddButton = true;
            pupilGrid.ToolBarSettings.ShowDeleteButton = true;
            pupilGrid.EditDialogSettings.CloseAfterEditing = true;
            pupilGrid.AddDialogSettings.CloseAfterAdding = true;
            pupilGrid.ToolBarSettings.ShowRefreshButton = true;
            #endregion
            #region SPFGrid
            List<System.Web.Mvc.SelectListItem> subjectDropDown = new List<System.Web.Mvc.SelectListItem>();
            foreach (Subject subject in VaKEGradeRepository.Instance.GetSubjects())
            {
                genderDropDown.Add(new System.Web.Mvc.SelectListItem() { Text = subject.Name, Value = subject.ID.ToString() });
            }

            spfGrid = new JQGrid
            {
                Columns = new List<JQGridColumn>()
                                 {
                                     new JQGridColumn { DataField = "ID",
                                                        Visible = false,
                                                        PrimaryKey = true,
                                                        Editable = false,
                                                        Width = 50 },                                    
                                     new JQGridColumn { DataField = "Name",                                                                HeaderText = "Fach", 
                                                        Editable = true,
                                                        EditType = Trirand.Web.Mvc.EditType.DropDown,
                                                        EditList = subjectDropDown,
                                                        Width = 100 },
                                                        
                                     new JQGridColumn { DataField = "Level",                                                               HeaderText = "Schulstufe", 
                                                        Editable = true,
                                                        Width = 100 },
                                 },
                Width = Unit.Pixel(640),
                Height = Unit.Percentage(100),
            };
            spfGrid.ToolBarSettings.ShowAddButton = true;
            spfGrid.ToolBarSettings.ShowDeleteButton = true;
            spfGrid.EditDialogSettings.CloseAfterEditing = true;
            spfGrid.AddDialogSettings.CloseAfterAdding = true;
            spfGrid.ToolBarSettings.ShowRefreshButton = true;
            #endregion
        }

    }
}