﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trirand.Web.Mvc;
using HTL.Grieskirchen.VaKEGrade.Models;
using HTL.Grieskirchen.VaKEGrade.Database;

namespace HTL.Grieskirchen.VaKEGrade.Controllers
{
    public class ClassTeacherController : Controller
    {
        //
        // GET: /ClassTeacher/

       
        public ActionResult Index()
        {
            
            if (Session["User"] != null)
            {
                Database.Teacher teacher = (Database.Teacher)Session["User"];
                Utility.GridData gData = new Utility.GridData();
                List<Database.SchoolClass> classes = Database.VaKEGradeRepository.Instance.GetClassesOfTeacher(teacher).ToList();
        //    ordersGrid.EditUrl = Url.Action("EditRowInline_EditRow");
                GeneratePupilGrid();
                return View(classes);
            }
            ViewData["error"] = "Bitte melden sie sich am System an";
            return Redirect("/Home/");
        }

        public JQGrid GeneratePupilGrid() {
            JQGrid pupilGrid = new VaKEGrade.Models.GridModel().PupilGrid;
            pupilGrid.DataUrl = Url.Action("RetrieveAllStudents");
            pupilGrid.EditUrl = Url.Action("EditStudent");
            pupilGrid.ClientSideEvents.RowSelect = "editRow";
            Session["PupilGModel"] = pupilGrid;
            return pupilGrid;
        }

        public bool IsAuthorized()
        {
            return Session["User"] != null && Session["Role"].ToString() == "ClassTeacher";
        }

        public JsonResult RetrieveAllStudents() {
            if (IsAuthorized()) {
                Teacher user = VaKEGradeRepository.Instance.GetTeacher(((Teacher)Session["User"]).ID);
                Session["User"] = user;
                Database.SchoolClass schoolClass = user.PrimaryClasses.First();
                return GeneratePupilGrid().DataBind(schoolClass.Pupils.AsQueryable());
                
            }
            return null;
        }

        public void EditStudent(Database.Pupil editedPupil)
        {
            // Get the grid and database (northwind) models
            var pupilModel = GeneratePupilGrid();

            // If we are in "Edit" mode, get the Order from database that matches the edited order
            // Check for "Edit" mode this way, we can also be in "Delete" or "Add" mode as well in this method
            if (pupilModel.AjaxCallBackMode == AjaxCallBackMode.EditRow)
            {
                // Get the data from and find the Order corresponding to the edited row
                Database.Pupil pupilToUpdate = Database.VaKEGradeRepository.Instance.GetPupil(editedPupil.ID);

                pupilToUpdate.FirstName = editedPupil.FirstName;
                pupilToUpdate.LastName = editedPupil.LastName;
                pupilToUpdate.Birthdate = editedPupil.Birthdate;
                pupilToUpdate.Religion = editedPupil.Religion;
                pupilToUpdate.Gender = editedPupil.Gender;
              
                Database.VaKEGradeRepository.Instance.Update();

                var pup = Database.VaKEGradeRepository.Instance.GetPupil(editedPupil.ID);
                // Update the Order information to match the edited Order data
                // In this demo we do not need to update the database since we are using Session
                // In your case you may need to actually hit the database
                
                // This will save the changes into the database. We have commented this since this is just an online demo
                // northWindModel.SubmitChanges();
            }
        }

        //// This is the default action for the View. Use it to setup your jqGrid Model.
        //public ActionResult EditRowInline()
        //{
        //    // Get the model (setup) of the grid defined in the /Models folder.
        //    var gridModel = new VaKEGrade.Models.GridModel();
        //    // This method sets common properties for the grid, different than the default in the Model
        //    EditRowInline_SetUpGrid(gridModel.PupilGrid);

        //    // Pass the custmomized grid model to the View
        //    return View(gridModel);
        //}

        //// This method is called when the grid requests data. You can choose any method to call
        //// by setting the JQGrid.DataUrl property. We are doing this in the EditRowInline_SetUpGrid method.
        //public JsonResult EditRowInline_DataRequested()
        //{
        //    // Get both the grid and data models. For data model, make sure IQueryable is implemented (most linq-2-* cases)
        //    // The data model in our case is an autogenerated linq2sql database based on Northwind.
        //    var gridModel = new VaKEGrade.Models.GridModel();
        //    var dataModel = GetOrders().AsQueryable();

        //    // This method sets common properties for the grid, different than the default in the Model
        //    EditRowInline_SetUpGrid(gridModel.OrdersGrid);

        //    // return the result of the DataBind method, passing the datasource as a parameter
        //    // jqGrid for ASP.NET MVC automatically takes care of paging, sorting, filtering/searching, etc
        //    return gridModel.OrdersGrid.DataBind(dataModel);
        //}

        //// The data gets passed to the controller as strongly typed objects of your data model, Order in our case
        //// This functionality is called Model Binders in ASP.NET MVC terms
        //public void EditRowInline_EditRow(Order editedOrder)
        //{
        //    // Get the grid and database (northwind) models
        //    var gridModel = new OrdersJqGridModel();
        //    var northWindModel = new NorthwindDataContext();

        //    // If we are in "Edit" mode, get the Order from database that matches the edited order
        //    // Check for "Edit" mode this way, we can also be in "Delete" or "Add" mode as well in this method
        //    if (gridModel.OrdersGrid.AjaxCallBackMode == AjaxCallBackMode.EditRow)
        //    {
        //        // Get the data from and find the Order corresponding to the edited row
        //        List gridData = GetOrders();
        //        Order orderToUpdate = gridData.Single(o => o.OrderID == editedOrder.OrderID);

        //        // Update the Order information to match the edited Order data
        //        // In this demo we do not need to update the database since we are using Session
        //        // In your case you may need to actually hit the database
        //        orderToUpdate.OrderDate = editedOrder.OrderDate;
        //        orderToUpdate.CustomerID = editedOrder.CustomerID;
        //        orderToUpdate.Freight = editedOrder.Freight;
        //        orderToUpdate.ShipName = editedOrder.ShipName;

        //        // This will save the changes into the database. We have commented this since this is just an online demo
        //        // northWindModel.SubmitChanges();
        //    }
        //}

        //public void EditRowInline_SetUpGrid(JQGrid ordersGrid)
        //{
        //    ordersGrid.DataUrl = Url.Action("EditRowInline_DataRequested");
        //    ordersGrid.EditUrl = Url.Action("EditRowInline_EditRow");
        //    ordersGrid.ClientSideEvents.RowSelect = "editRow";

        //    // setup the dropdown values for the CustomerID editing dropdown
        //    EditRowInline_SetUpCustomerIDColumn(ordersGrid);
        //}

        //private void EditRowInline_SetUpCustomerIDColumn(JQGrid ordersGrid)
        //{
        //    // setup the grid search criteria for the columns
        //    JQGridColumn customersColumn = ordersGrid.Columns.Find(c => c.DataField == "CustomerID");
        //    customersColumn.Editable = true;
        //    customersColumn.EditType = EditType.DropDown;

        //    // Populate the search dropdown only on initial request, in order to optimize performance
        //    if (ordersGrid.AjaxCallBackMode == AjaxCallBackMode.RequestData)
        //    {
        //        var northWindModel = new NorthwindDataContext();
        //        var editList = from customers in northWindModel.Customers
        //                       select new SelectListItem
        //                       {
        //                           Text = customers.CustomerID,
        //                           Value = customers.CustomerID
        //                       };

        //        customersColumn.EditList = editList.ToList();
        //    }
        //}


        //// This is a helper method fetching the data from Session
        //public List EditRowInline_GetOrders()
        //{
        //    List orders;
        //    if (Session["Orders"] == null)
        //    {
        //        var northWindModel = new NorthwindDataContext();
        //        orders = (from order in northWindModel.Orders
        //                  select order).ToList();
        //        Session["Orders"] = orders;
        //    }
        //    else
        //    {
        //        orders = Session["Orders"] as List;
        //    }

        //    return orders;
        //}

    }
}
