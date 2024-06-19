using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Spire.Xls;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Grid;
using Spire.Pdf.Fields;
using Spire.Pdf.HtmlConverter;
using Spire.Pdf.HtmlConverter.Qt;
using Spire.License;
using Spire.Pdf.Widget;
using Spire.Pdf.Utilities;
//using PdfSharp;
//using PdfSharp.Pdf;
//using PdfSharp.Pdf.IO;
using ExportApp.Models;
using Spire.Pdf.Texts;
using Spire.Pdf.General.Find;
using Microsoft.AspNetCore.Mvc;
using Spire.Doc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using ExportApp.Services.User;
using Microsoft.AspNetCore.Http;
// using Microsoft;

namespace ExportApp.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly ILogger<ExportController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserService _userService;
        // private readonly IUserService _userService1;
        private const string SessionUserId = "_Id";
        private const string SessionUserName = "_UserName";
        public ExportController(ILogger<ExportController> logger, IWebHostEnvironment hostingEnvironment, IUserService userService) // IUserService userService1
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            // _userService1 = userService1;
        }

        [HttpPost]
        [Route("SetSessionValues")]
        public IActionResult SetSessionValues() // ActionResult
        {
            HttpContext.Session.SetInt32(SessionUserId, 1);
            HttpContext.Session.SetString(SessionUserName, "Ritesh");
            return StatusCode((int)HttpStatusCode.OK, "Done");
        }

        [HttpGet]
        [Route("RetriewSessionValues")]
        public IActionResult GetSessionStateValues()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            string? userName = HttpContext.Session.GetString(SessionUserName);
            return StatusCode((int)HttpStatusCode.OK, new { userId, userName });
        }

        [HttpPost]
        [Route("SaveCustomerDetails")]
        public IActionResult SaveCustomerInformation(UserDetails userDetails) // ActionResult<UserResponse>
        {
            var userResponse = new UserResponse();
            try
            {
                int userId = _userService.SaveDetails(userDetails);
                userResponse.UserId = userId;
                userResponse.UserName = $"User_{userId}";
                userResponse.Message = "Customer Details saved successfully.";
                return StatusCode((int)HttpStatusCode.OK, userResponse);
            }
            catch (Exception ex)
            {
                userResponse.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.InternalServerError, userResponse);
            }
        }

        [HttpPost]
        [Route("/ExportDataToPDF")]
        public object ExportDataToPDF()
        {
            Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
            PdfPageBase pdfPage = pdf.Pages.Add();

            PdfTextBoxField pdfTextBoxField = new PdfTextBoxField(pdfPage, "first_name");
            pdfTextBoxField.Bounds = new RectangleF(0, 0, 50, 12);
            PdfTrueTypeFont textboxFont = new PdfTrueTypeFont(new Font("Arial", 12f, FontStyle.Regular));
            pdfTextBoxField.Font = textboxFont;
            pdfTextBoxField.Text = "Ritesh Karasanbhai Patel";
            pdf.Form.Fields.Add(pdfTextBoxField);

            PdfGrid pdfGrid = new PdfGrid();
            PdfGridRow pdfGridRow1 = pdfGrid.Rows.Add();
            PdfGridRow pdfGridRow2 = pdfGrid.Rows.Add();

            pdfGrid.Style.CellPadding.Top = 10f;
            pdfGrid.Style.CellPadding.Bottom = 10f;

            pdfGrid.Columns.Add(2);
            pdfGrid.Columns[0].Width = 250f;
            pdfGrid.Columns[1].Width = 250f;

            PdfStringFormat stringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            pdfGridRow1.Cells[0].Value = "Customer' Name";
            pdfGridRow1.Cells[0].StringFormat = stringFormat;
            pdfGridRow1.Cells[0].Style.BackgroundBrush = PdfBrushes.Gray;
            pdfGridRow1.Cells[1].Value = "Product(s)";
            pdfGridRow1.Cells[1].StringFormat = stringFormat;
            pdfGridRow1.Cells[1].Style.BackgroundBrush = PdfBrushes.Gray;
            pdfGridRow2.Cells[0].Value = "Ritesh Patel";
            pdfGridRow2.Cells[0].StringFormat = stringFormat;
            pdfGridRow2.Cells[0].Style.TextBrush = PdfBrushes.Blue;
            pdfGridRow2.Cells[0].Style.TextPen = PdfPens.Green;
            pdfGridRow2.Cells[1].Value = "Home & Care";
            pdfGridRow2.Cells[1].StringFormat = stringFormat;
            pdfGridRow2.Cells[1].Style.TextBrush = PdfBrushes.Blue;
            pdfGridRow2.Cells[1].Style.TextPen = PdfPens.Green;

            pdfGrid.Draw(pdfPage, new PointF(0f, 50f));
            pdf.SaveToFile(@"MyDocs/CustomerProdInfo.pdf");

            return new
            {
                Data = "Success"
            };
        }

        [HttpPost]
        [Route("FillUAEResidentailHomeLoanAppForm")]
        public object FillUAEResidentailHomeLoanAppForm()
        {
            System.IO.File.Copy(@"BlankDocs/uae-residential-home-loan-application-form.pdf", "MyDocs/uae-residential-home-loan-application-form.pdf", true);

            PdfFormFillupOperation pdfFormFillupOperation = new PdfFormFillupOperation("MyDocs/uae-residential-home-loan-application-form.pdf");

            List<FormFieldModel> formFieldModels = new List<FormFieldModel>();
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "John, Antonegelli",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Mortgage Advisor: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 0.8f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1001001234",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Account Number: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Joint",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "3 Months AED EIBOR",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Richard Drennan",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Please indicate who has referred you to us: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 8f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Mr.",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Title (e.g. Mr. / Mrs.)",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Mr.",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Title (e.g. Mr. / Mrs.)",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Ritesh Patel",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Name (as on Passport)",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Shailesh Baria",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Name (as on Passport)",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ritesh@zurumedia.com",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Email Address",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "shailesh@zurumedia.com",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Email Address",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "9173084438",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Mobile Number",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "9925769909",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Mobile Number",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });

            List<PdfTextFind> searchTextList1 = pdfFormFillupOperation.FindTextLocation("First Applicant", 0);
            List<PdfTextFind> searchTextList2 = pdfFormFillupOperation.FindTextLocation("Second Applicant", 0);
            float textboxWidth = searchTextList2[0].Position.X - searchTextList1[0].Position.X;
            textboxWidth -= 10; // subtract spacing 10 inside of textbox;
            string firstAppAddress = "Mr Ahmed Al Wasl,General WWEE General Manager AAW PO Box 12345 General Trading PO Box 12345 Dubai, UAE";
            string result = pdfFormFillupOperation.wrapText(firstAppAddress, textboxWidth, 3);

            formFieldModels.Add(new FormFieldModel()
            {
                Value = result,
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Physical Address",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true,
                    SubtractedHeight = 10.0f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = pdfFormFillupOperation.wrapText("Ahmed Al Wasl AAW General Trading PO Box 17384 Dubai, UAE", textboxWidth, 3),
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Physical Address",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true,
                    SubtractedHeight = 10.0f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Mr Ahmed Al Wasl, Manager\r\nAAW General Trading\r\nPO Box 12345 Dubai, UAE",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Home country address",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true,
                    SubtractedHeight = 15.0f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Ahmed Al Wasl\r\nAAW General Trading\r\nPO Box 17384 Dubai, UAE",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Home country address",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true,
                    SubtractedHeight = 15.0f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "9173084438",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Home country contact details",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true,
                    SubtractedHeight = 15.0f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "9925769909",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Home country contact details",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true,
                    SubtractedHeight = 15.0f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Employed",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Employed",
                    SearchTextIndex = 1,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Permanent",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Permanent",
                    SearchTextIndex = 1,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Private Job",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Occupation",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Private Job",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Occupation",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Zuru Media India Pvt. Ltd. Mr Ahmed Al\r\nWasl, Manager AAW General Trading\r\nPO Box 12345 Dubai, UAE.",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Present employer and address",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Zuru Media India Pvt. Ltd. Mr Ahmed Al\r\nWasl, Manager AAW General Trading\r\nPO Box 12345 Dubai, UAE.",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Present employer and address",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1st November, 2022",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Present employment started",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "15th July, 2022",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Present employment started",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "15%",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "What percentage of share capital do you own?",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "20%",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "What percentage of share capital do you own?",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Zuru Media India Pvt. Ltd.",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Name of the company",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Zuru Media India Pvt. Ltd.",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Name of the company",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1st January, 2022",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Date company established",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1st January, 2022",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Date company established",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "20,000 £",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Average net profit of last two years",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "20,000 £",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Average net profit of last two years",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "20,000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Gross basic income",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "24,000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Gross basic income",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1500",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Guaranteed allowances",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "2100",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Guaranteed allowances",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "5000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Average of last 2 years annual bonus",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "7000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Average of last 2 years annual bonus",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "0",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Other regular income",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "0",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Other regular income",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Nothing",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Source of other income",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Nothing",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 0,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Source of other income",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "No",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 0,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "No",
                    SearchTextIndex = 1,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "AED",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "State currency",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 0.8f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "3",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "No. of Dependents:",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "3",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "No. of Dependents:",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "5000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Rent",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "5000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Rent",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Home loan and medical insurance",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Home Loan and Related",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Home loan and medical insurance",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Home Loan and Related",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1500",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "School Fees",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1800",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "School Fees",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1500",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "School Fees",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1800",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "School Fees",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "4500",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Utilities eg. Electric, Water",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "5100",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Utilities eg. Electric, Water",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "800",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Telephone",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "950",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Telephone",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "7000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Living Expenses",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "7600",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Living Expenses",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "500",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Entertainment",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "625",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Entertainment",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1225",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Regular Savings",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1310",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Regular Savings",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "800",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "First Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Other Expenses",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "975",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = null,
                ValueXPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Second Applicant",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true
                },
                ValueYPosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Other Expenses",
                    SearchTextIndex = 0,
                    IncludeYPosOfSearchText = true
                }
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "FADB",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Company / Bank Name",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 15.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Credi Card",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Product: (Credit Card, Personal Loan,",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 25.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "10000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Credit Card Limit /",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 25.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "833.33",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Monthly",
                    SearchTextIndex = 1,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 25.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "12",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Number of Payments",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 25.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ABFIFT",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Company / Bank Name",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 30.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ABFIFT",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Company / Bank Name",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 30.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Personal Loan",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Product: (Credit Card, Personal Loan,",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 40.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "24000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Credit Card Limit /",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 40.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Monthly",
                    SearchTextIndex = 1,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 40.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "24",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Number of Payments",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 40.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ADCB",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Company / Bank Name",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 47.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Home Loan",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Product: (Credit Card, Personal Loan,",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 57.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "48000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Credit Card Limit /",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 57.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "2000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Monthly",
                    SearchTextIndex = 1,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 57.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "24",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Number of Payments",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 57.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "CBOD",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Company / Bank Name",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 62.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Vehicle Loan",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Product: (Credit Card, Personal Loan,",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 72.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "18000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Credit Card Limit /",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 72.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1500",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Monthly",
                    SearchTextIndex = 1,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 72.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "12",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Number of Payments",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    AddedHeight = 72.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Equity release loan",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "Tax benefits,Rent reduction",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Purpose* of Top Up / ERL loan proceeds",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 0.9f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "50000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "AED",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 0.9f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "12000",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "AED",
                    SearchTextIndex = 2,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 0.9f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "18",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Home Loan Repayment Period (in months)",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 10f,
                    SubtractedHeight = 0.9f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "01/01/2024",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Preferred repayment date (dd/mm/yyyy)",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 22f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "4500",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "this is applicable for Balance Transfer only) If yes, please state the amount: AED ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 15f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Savings",
                    SearchTextIndex = 1,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 7.40f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "89, Mr Ahmed Al Wasl, Manager, AAW General Trading, PO Box 12345 Dubai, UAE",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Property Address: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "30005 95279",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Makani Number: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 10f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Villa / Townhose",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 1,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Investment",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "ü",
                ValueControlType = FieldValueControlType.Checkbox,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Individual",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SubtractedWidth = 9.5f,
                    SubtractedHeight = 0.1f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "John Ali",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Name (as per Passport):",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "UAE",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Country of Residence: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "1606302505909",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Passport Number: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "UAE",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Country of Residence: ________________________________________________ Nationality: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "John Ali",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "Name: ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 5f,
                    SubtractedHeight = 1.2f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "\"HSBC Home Loan Property Insurance Guidelines\" paper.",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = "11. Additional Information",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextHeightOp = SearchTextDimensionOp.Add,
                    AddedHeight = 5.0f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "4608902505909",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = " GWIS Reference Number (Passport): ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 1.0f,
                    SubtractedHeight = 0.8f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });
            formFieldModels.Add(new FormFieldModel()
            {
                Value = "46089025078479",
                ValueControlType = FieldValueControlType.Text,
                PageIndex = 2,
                ValuePosModel = new FormFieldValuePositionModel()
                {
                    SearchText = " GWIS Reference Number (EID): ",
                    SearchTextIndex = 0,
                    IncludeXPosOfSearchText = true,
                    IncludeYPosOfSearchText = true,
                    SearchTextWidthOp = SearchTextDimensionOp.Add,
                    AddedWidth = 1.0f,
                    SubtractedHeight = 0.8f
                },
                ValueXPosModel = null,
                ValueYPosModel = null
            });


            pdfFormFillupOperation.FillUpFormFields(formFieldModels, @"MyDocs/uae-residential-home-loan-application-form.pdf");

            return new
            {
                Data = "Success"
            };
        }

        [HttpPost]
        [Route("FillENBDApplicationForm")]
        public object FillENBDApplicationForm()
        {
            System.IO.File.Copy(@"BlankDocs/ENBD_application_form.pdf", "MyDocs/ENBD_application_form.pdf", true);

            List<PdfFormFieldWidgetControl> pdfFormFieldWidgetControls = new List<PdfFormFieldWidgetControl>();
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text1", Value = "01-06-2018" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group3- sad-1644", Value = true, SelectedItemIndex = 1 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "relationship - frist page", Value = "Other" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "agreement - frist page", Value = "65784093452170" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "cif number - frist page", Value = "89345219" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group3- sad-12", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group3- sadaa", Value = true, SelectedItemIndex = 3 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "frist page -1", Value = "4281840903097" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "date of receipt", Value = "12-06-2018" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "code", Value = "2710909045797" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "loan nu-frist page", Value = "90306342818097" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "code-1", Value = "5797271090904" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "RBE", Value = "4281840903097" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "scheme", Value = "2" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "code-2", Value = "9320309045797" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group3", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text5 page 2 of 11-others -465s", Value = "-" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text6-page 2 -name", Value = "John" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text6021", Value = "Duet" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text6022", Value = "Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text7- page 2 of 11- nation", Value = "Emirates" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group4-ma", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "date - frist page -7512", Value = "07061982" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text8- page 2 of 11- pls of brith", Value = "Dubai" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group3-sing", Value = true, SelectedItemIndex = 1 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "educational q", Value = "I.T. Enginner" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-1", Value = "12345" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text41-under address", Value = "12345" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text50-under address", Value = "12345" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text34-under address", Value = "Trading General A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text42-under address", Value = "AAW Trading General" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text51-under address", Value = "Trading General B" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text35-under address", Value = "123" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text43-under address", Value = "456" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text52-under address", Value = "789" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text36-under address", Value = "Emirates Towers" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text44-under address", Value = "The Dubai Fountain" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text53-under address", Value = "Burj Khalifa" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text37-under address", Value = "Dubai" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text45-under address", Value = "Dubai" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text54-under address", Value = "Dubai" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text38-under address", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text46-under address", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-under address", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text39-under address", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text47-under address", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text56-under address", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text48-under address", Value = "25214" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text57-under address", Value = "25414" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text40-under address", Value = "ROR" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text49-under address", Value = "ROR" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text58-under address", Value = "ROR" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text41", Value = "6" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text45", Value = "1234567" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text5-dsd", Value = "6" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text46", Value = "1234567" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text47", Value = "+971" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text48", Value = "(4)" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text49", Value = "12-345-6789" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text5-dsd-1", Value = "+971" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text50", Value = "(4)" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text51", Value = "12-345-6789" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text5-dsd-3", Value = "+971" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text53", Value = "+6" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text54", Value = "12-345-6789" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-stfj", Value = "+971" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text550", Value = "+6" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text5600", Value = "12-345-6789" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text552", Value = "+971" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text550-2", Value = "+4" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text560", Value = "12-345-6789" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55", Value = "johnduet27@gmail.com" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text56", Value = "john27duet@gmail.com" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-1", Value = "1606302505909" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-2", Value = "14-08-2016" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-3", Value = "14-08-2026" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-4", Value = "Dubai" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-5", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-6", Value = "A1234567" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-7", Value = "28-12-2021" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-8", Value = "28-12-2025" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-9", Value = "Dubai" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-10", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-11", Value = "6488816636" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-12", Value = "784-1234-1234567-1" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-13", Value = "12-08-2020" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-14", Value = "12-08-2027" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-15", Value = "Dubai" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-16", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-17", Value = "Family Book" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-18", Value = "04-09-2018" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-19", Value = "04-09-2038" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-20", Value = "Dubai" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text55-20DDES", Value = "UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box124646", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box124648", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box124650", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box124647", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box124649", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "OTHERS 5T0", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "account -46", Value = "7384096741238407" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "account -47", Value = "0840723840" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "account -47de", Value = "4639" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "account -48", Value = "4123840703840967" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "BANK NAME", Value = "HSBC" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "BRANCH", Value = "ENBD Nr. Burj kalifa" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "ACCOUNT TYPE", Value = "Savings" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "ACCOUNTNU,SDFAS", Value = "4123840703840967" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group5 -page 2", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "OTHERS 5T", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group6 -page 2", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "CURRENT EMPO/", Value = "Zurumedia Pvt. Ltd." });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "DESIGNATION", Value = "Sr. I.T. Engineer" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "DEPT-SAA", Value = "I.T." });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "TIME", Value = "15062022" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "EMPO/", Value = "BEON-IT Software Service" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "name of UAE-SSS", Value = "Mr Ahmed Al Wasl, Manager AAW General Trading PO Box 12345 Dubai, UAE." });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "TIME-5", Value = "052021" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "YEARS -54-BW", Value = "1 year" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "NATURE OF BUS", Value = "I.T. Services" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "mobil-798", Value = "6845137638" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "YEARS -54", Value = "11" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "name of UAE", Value = "Micky Jagtiani" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "1", Value = "John Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "4", Value = "50" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "2", Value = "Mac Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "5DEEEEEW", Value = "30" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "3", Value = "Johny Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "5", Value = "20" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "if you are -12", Value = "35" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "name -4564sdfg", Value = "Mac Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "mobil-79800", Value = "7382836830" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "emirate-216+1+61", Value = "Abu Dhabi" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "emial64+646", Value = "mac536@gmail.com" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "name -2-32161", Value = "Johny Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "mobil", Value = "5873146857" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "emirated1644", Value = "Abu Dhabi" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "emial", Value = "johny.antonegelli72@gmail.com" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "icome details -1", Value = "35000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "icome details -2", Value = "20000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "icome details -3", Value = "10000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "icome details -4", Value = "5000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "icome details -5", Value = "70000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group170-st", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "others - st", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "salary cc", Value = "30" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "others - st", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "average net - turnover", Value = "5000000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "average net - profit", Value = "1000000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group171-st", Value = true, SelectedItemIndex = 2 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "if leasehold", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group172-st", Value = true, SelectedItemIndex = 1 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "if leasehold-4st", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group173-st", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "purpose  of loan", Value = "Lower interest rates" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group174-st", Value = true, SelectedItemIndex = 1 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "every day", Value = "01" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "ads", Value = "022024" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "loan arfvv- page 1", Value = "12" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "loan arfvv", Value = "3000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "loan ternor -8552", Value = "10" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "loan amount applied", Value = "40000000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group175-st", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group176-st", Value = true, SelectedItemIndex = 1 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "perioed", Value = "7" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "rate%", Value = "4" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "rate dgw", Value = "2.5" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "there after consumer", Value = "6.5" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "for the frist month", Value = "4" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "down payment", Value = "15.5" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "repayment of interest", Value = "4385098741238407" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from -1", Value = "02-06-2018" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "lan amount -01", Value = "4535000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "tenor-01", Value = "14" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "monthly insl-1", Value = "4450" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "outstanding -1", Value = "1500" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from -2", Value = "07-09-2018" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "lan amount -02", Value = "8545000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "tenor-02", Value = "16" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "monthly insl-2", Value = "2450" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "outstanding -2", Value = "1800" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from -3", Value = "01-09-2018" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "lan amount -03", Value = "1647000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "tenor-03", Value = "11" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "monthly insl-3", Value = "1950" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "outstanding -3", Value = "1100" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from -4", Value = "16-04-2018" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "lan amount -04", Value = "2247000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "tenor-04", Value = "27" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "monthly insl-4", Value = "3950" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "outstanding -4", Value = "2100" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_123", Value = "John Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234", Value = "Johny Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4852", Value = "637523" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4852DDESDDDD", Value = "237563" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4852d", Value = "0345" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4852dd", Value = "3100" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_1", Value = "200000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_2", Value = "2017" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_3", Value = "250000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_4", Value = "2016" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_5", Value = "1697400" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_6", Value = "Nakheel" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_7", Value = "16" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_8", Value = "89, Mr Ahmed Al Wasl, Manager, AAW General Trading, PO Box 12345 Dubai, UAE" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_9", Value = "Sheikh Saeed Al-Maktoum House." });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_10", Value = "111188" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_11", Value = "Dubai, Abu Dhabi" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_12", Value = "Nakheel" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_13", Value = "Maktoum House-2" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "from-4_1234_14", Value = "3" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group176-st_pag51", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_1", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_2", Value = "13" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_3", Value = "5" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_4", Value = "large" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_5", Value = "6200" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_6", Value = "8000" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_7", Value = "25" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_654DDD", Value = "5" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_7DDDESAA", Value = "Maccy Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_8", Value = "Mr Ahmed Al Wasl, Manager AAW General Trading PO Box 12345 Dubai, UAE." });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_9", Value = "John Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_10", Value = "Marry Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group176-st_pag5122", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Page12Field_12", Value = "6374608226043" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group176-st_pag51225", Value = true, SelectedItemIndex = 1 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group176-st_pag512256", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1251", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1252dss", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1252", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1254", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1255", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1256", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group176-st_pag5122567", Value = true, SelectedItemIndex = 0 });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1257", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1258", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d1259", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12510", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12511", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12512", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12513", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12514", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12515", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12516", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12517", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "109_d12518", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "date page 1223", Value = "04-01-2024" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text300", Value = "No remarks" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text3001", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text3002", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text3004", Value = "N/A" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "203", Value = "Rossy Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "3008", Value = "74568264" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "3008-7b", Value = "65587330" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "203-b", Value = "Monny Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "205", Value = "ID card" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "206", Value = "John Antonegelli" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "207", Value = "746895406540674213953" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "208", Value = "2 months" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "209", Value = "3660346481" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "210", Value = "No Description" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "211", Value = "6567389014" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "212", Value = "4598964553" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "204", Value = "09-04-2023" });

            PdfFormOperation pdfFormOperation = new PdfFormOperation("MyDocs/ENBD_application_form.pdf");
            pdfFormOperation.FillUpFormFields(pdfFormFieldWidgetControls, @"MyDocs/ENBD_application_form.pdf");

            //MemoryStream pdfDocMS = new MemoryStream();
            //pdf.SaveToStream(pdfDocMS); //// Save the PDF Document to the specified Stream.

            //// Create a temporary memory stream for the zip file
            //using var zipMS = new MemoryStream();

            //using (var archive = new ZipArchive(zipMS, ZipArchiveMode.Create, true))
            //{
            //    var zipArchiveEntry = archive.CreateEntry("ENBD_application_form.pdf", CompressionLevel.SmallestSize);
            //    using (var zipArchiveEntryMS = zipArchiveEntry.Open())
            //    {
            //        //byte[] bytes = pdfDocMS.ToArray();
            //        //zipArchiveEntryMS.Write(bytes, 0, bytes.Length);
            //        pdfDocMS.WriteTo(zipArchiveEntryMS);
            //    }
            //}

            //zipMS.Seek(0, SeekOrigin.Begin);

            //using (FileStream fs = System.IO.File.Create("MyDocs/ENBD_application_form.zip"))
            //{
            //    zipMS.CopyTo(fs);
            //}

            //ZipFile.CreateFromDirectory("/MyDocs/", "Archived/MyPDF.zip");
            //ZipFile.ExtractToDirectory("Archived/MyPDF.zip", "Extracted");

            return new
            {
                Data = "Success"
            };
        }

        [HttpPost]
        [Route("FillPdfFormExampleDataToPDF")]
        public object FillPdfFormExampleDataToPDF()
        {
            List<PdfFormFieldWidgetControl> pdfFormFieldWidgetControls = new List<PdfFormFieldWidgetControl>();

            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Given Name Text Box", Value = "Ritesh" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Family Name Text Box", Value = "Patel" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "House nr Text Box", Value = "H-100" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Address 2 Text Box", Value = "Same as Address1" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Postcode Text Box", Value = "390021" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Country Combo Box", Value = "Romania" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Height Formatted Field", Value = "168" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "City Text Box", Value = "Bucharest" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Driving License Check Box", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Favourite Colour List Box", Value = "Green" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Language 1 Check Box", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Language 2 Check Box", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Language 3 Check Box", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Language 4 Check Box", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Language 5 Check Box", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Gender List Box", Value = "Man" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Address 1 Text Box", Value = "C-57, Om Society, B/h Jalaram nagar, Nr. Mother School, Gotri, 390021" });

            PdfFormOperation pdfFormOperation = new PdfFormOperation("MyDocs/PdfFormExample.pdf");
            pdfFormOperation.FillUpFormFields(pdfFormFieldWidgetControls, @"MyDocs/PdfFormExample.pdf");

            return new
            {
                Data = "Success"
            };
        }

        [HttpPost]
        [Route("FillSampleDataToPDF")]
        public object FillSampleDataToPDF()
        {
            List<PdfFormFieldWidgetControl> pdfFormFieldWidgetControls = new List<PdfFormFieldWidgetControl>();
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Name", Value = "Ritesh K. Patel" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Address", Value = "C-57, Om Society, B/h Jalaram nagar, Nr. Mother School, Gotri, 390021" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Dropdown1", Value = "22" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Dropdown2", Value = "Dec" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Dropdown3", Value = "2022" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box4", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box1", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box3", Value = false });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Check Box2", Value = true });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text5", Value = "Research & Development" });
            // pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Button7", Value = "Save" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Text6", Value = "Research & Development" });
            pdfFormFieldWidgetControls.Add(new PdfFormFieldWidgetControl() { Name = "Group6", Value = "Choice1" });

            PdfFormOperation pdfFormOperation = new PdfFormOperation("MyDocs/sample_pdf.pdf");
            pdfFormOperation.FillUpFormFields(pdfFormFieldWidgetControls, @"MyDocs/sample_pdf.pdf");
            return new
            {
                Data = "Success"
            };
        }

        [HttpPost]
        [Route("ExportEmployeeDataToPDFUsingSpirFreee")]
        public object ExportEmployeeDataToPDFUsingSpirFreee()
        {
            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            workbook.LoadFromFile(@"EmployeeData.xlsx");
            Spire.Xls.Worksheet worksheet = workbook.Worksheets[0];
            DataTable dt = worksheet.ExportDataTable();

            Spire.Pdf.PdfDocument pdfDoc = new Spire.Pdf.PdfDocument();

            SizeF sizeF = new SizeF(1800f, 3368f);
            PdfPageBase pdfPage = pdfDoc.Pages.Add(sizeF, new PdfMargins() { All = 15 }, PdfPageRotateAngle.RotateAngle0); // PdfPageSize.A4, new PdfMargins() { Top = 0, Bottom = 0, Left = 0, Right = 0 }, PdfPageRotateAngle.RotateAngle0, PdfPageOrientation.Landscape

            PdfGrid pdfGrid = new PdfGrid();
            pdfGrid.Style.CellPadding.Top = 5f;
            pdfGrid.Style.CellPadding.Bottom = 5f;
            pdfGrid.Style.CellPadding.Left = 5f;
            pdfGrid.Style.CellPadding.Right = 5f;

            pdfGrid.Columns.Add(dt.Columns.Count);

            //foreach (PdfGridColumn pdfGridColumn in pdfGrid.Columns)
            //{
            //    pdfGridColumn.Width = 120f;
            //}

            PdfStringFormat stringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);

            PdfGridRow pdfGridRow;
            PdfGridCell pdfGridCell;
            string cellValue;
            PdfBrush pdfBrush;

            pdfGridRow = pdfGrid.Rows.Add();
            for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
            {
                pdfGridCell = pdfGridRow.Cells[iCol];
                pdfGridCell.Value = dt.Columns[iCol].ColumnName;
                pdfGridCell.StringFormat = stringFormat;
                pdfGridCell.Style.BackgroundBrush = PdfBrushes.Gray;
            }

            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
            {
                pdfGridRow = pdfGrid.Rows.Add();

                for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
                {
                    pdfGridCell = pdfGridRow.Cells[iCol];
                    cellValue = Convert.ToString(dt.Rows[iRow][iCol]);
                    if (dt.Columns[iCol].ColumnName == "Annual Salary")
                    {
                        long salary = Convert.ToInt64(cellValue.Replace(",", string.Empty).Replace("$", string.Empty));
                        if (salary <= 100000)
                        {
                            pdfBrush = PdfBrushes.Yellow;
                        }
                        else if (salary > 100000 && salary < 150000)
                        {
                            pdfBrush = PdfBrushes.Blue;
                        }
                        else
                        {
                            pdfBrush = PdfBrushes.Green;
                        }
                        pdfGridCell.Style.BackgroundBrush = pdfBrush;
                    }
                    pdfGridCell.Value = cellValue;
                    pdfGridCell.StringFormat = stringFormat;
                }
            }

            pdfGrid.Draw(pdfPage, new PointF(0f, 0f));
            // pdfDoc.SaveToFile("EmployeeData.pdf");
            MemoryStream memoryStream = new MemoryStream();
            pdfDoc.SaveToStream(memoryStream, Spire.Pdf.FileFormat.PDF);
            // Stream[] streams = pdfDoc.SaveToStream(Spire.Pdf.FileFormat.PDF);
            memoryStream.Position = 0;

            return File(memoryStream, "application/octet-stream");
        }

        [HttpPost]
        [Route("ExportInvoiceDetailsToPDFUsingQuestPDF")]
        public object ExportInvoiceDetailsToPDFUsingQuestPDF()
        {
            Settings.License = LicenseType.Community;
            InvoiceModel model = InvoiceDocumentDataSource.GetInvoiceDetails();
            InvoiceDocument document = new InvoiceDocument(model);
            document.GeneratePdf("invoice.pdf");
            return new
            {
                Data = "Success"
            };
        }

        [HttpPost]
        [Route("ConvertHtmlToPdf")]
        public object ConvertHtmlToPdf()
        {
            string htmlContent = System.IO.File.ReadAllText("MyDocs/LeadTypeHelp.html");
            string pdfFilePath = Path.Combine(_hostingEnvironment.ContentRootPath, "MyDocs\\LeadTypeHelp.pdf");
            string plugInPath = Path.Combine(_hostingEnvironment.ContentRootPath, "bin\\Debug\\net8.0\\plugins");
            HtmlConverter.PluginPath = plugInPath;
            HtmlConverter.Convert(htmlContent, pdfFilePath, false, 10 * 1000, new SizeF(595.4f, 842f), new PdfMargins(0), LoadHtmlType.SourceCode);
            return new
            {
                Data = "Success"
            };
        }

        [HttpPost]
        [Route("ConvertDocToPdf")]
        public object ConvertDocToPdf()
        {
            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile("MyDocs/Solar.html", Spire.Doc.FileFormat.Html, Spire.Doc.Documents.XHTMLValidationType.None);
            document.Sections[0].PageSetup.Margins.All = 10f;
            document.Sections[0].PageSetup.PageSize = new SizeF(595.4f, 842f);
            document.SaveToFile("MyDocs/Solar.pdf", Spire.Doc.FileFormat.PDF);
            return new
            {
                Data = "Success"
            };
        }

        [HttpPost]
        [Route("ExportEmployeeDataToPDFUsingQuestPDF")]
        public object ExportEmployeeDataToPDFUsingQuestPDF()
        {
            Settings.License = LicenseType.Community;
            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            workbook.LoadFromFile(@"EmployeeData.xlsx");
            Spire.Xls.Worksheet worksheet = workbook.Worksheets[0];

            DataTable dt = worksheet.ExportDataTable();
            DataTable dataTable = dt.Select("", "Gender desc").CopyToDataTable();

            EmployeeDocument document = new EmployeeDocument(dataTable);
            document.GeneratePdf("EmployeeData1.pdf");
            MemoryStream memoryStream = new MemoryStream();
            document.GeneratePdf(memoryStream);
            memoryStream.Position = 0;

            return File(memoryStream, "application/octet-stream");
        }
    }
}
