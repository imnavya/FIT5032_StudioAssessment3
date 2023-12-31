﻿using Microsoft.AspNet.Identity;
using OmniScanMRI.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;


namespace OmniScanMRI.WebApp.Controllers
{
    public class ScanImageController : Controller
    {
        // GET: ScanImage
        private OmniScanContext _context = new OmniScanContext();

        //POST: UploadScan to ScanDetails DB
        [HttpPost]
        public ActionResult UploadScan(UploadScanViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.UploadFile != null && model.UploadFile.ContentLength > 0)
                    {
                        
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(model.UploadFile.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("", "Only .jpg, .jpeg, .png, and .gif file types are allowed.");
                            return View(model);
                        }

                        
                        var fileName = $"{Guid.NewGuid()}{fileExtension}";
                        var path = Path.Combine(Server.MapPath("~/ScanUploads"), fileName);
                        
                        model.UploadFile.SaveAs(path);

                        var currentUserId = User.Identity.GetUserId();

                        var scanImage = new ScanDetails
                        {
                            Name = model.ScanName,
                            Path = $"~/ScanUploads/{fileName}",
                            DateTaken = DateTime.Now,
                            UserId = currentUserId
                        };


                        _context.ScanImages.Add(scanImage);
                        _context.SaveChanges();

                        return RedirectToAction("UploadSuccess", "ScanImage");
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please select a correct file.");
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            
                            System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage);

                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    ModelState.AddModelError("", "Error uploading file: " + ex.Message);
                }


            }
            model.Patients = _context.Patients
                .Select(p => new SelectListItem
                {
                    Value = p.UserId.ToString(),
                    Text = p.FirstName + " " + p.LastName
                }).ToList();
            
            return View(model);
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult UploadScan()
        {
            var viewUploadScanModel = new UploadScanViewModel();
            
            viewUploadScanModel.Patients = _context.Patients
                                        .Select(p => new SelectListItem
                                        {
                                            Value = p.UserId.ToString(),
                                            Text = p.FirstName + " " + p.LastName
                                        }).ToList();

            return View(viewUploadScanModel);
        }

        public JsonResult GetPatients(string searchTerm)
        {
            var patients = _context.Patients
                            .Where(p => p.LastName.Contains(searchTerm))
                            .Select(p => new { id = p.UserId, text = p.FirstName + " " + p.LastName })
                            .ToList();

            return Json(patients, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadSuccess()
        {
            return View();
        }
    }
}