using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;

namespace HTL.Grieskirchen.VaKEGrade.Models
{
    public class GridModel
    {
        JQGrid pupilGrid;

        public JQGrid PupilGrid
        {
            get { return pupilGrid; }
            set { pupilGrid = value; }
        }
        
        public GridModel() {
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
                                                        DataFormatString = "{0:dd/MM/yyyy}" },
                                     new JQGridColumn { DataField = "Gender",                                                                  HeaderText = "Geschlecht", 
                                                        Editable = true,
                                                        Width = 75 }                           
                                 },
                Width = Unit.Pixel(640),
                Height = Unit.Percentage(100),
            };

            pupilGrid.ToolBarSettings.ShowRefreshButton = true;    
        }
    }
}