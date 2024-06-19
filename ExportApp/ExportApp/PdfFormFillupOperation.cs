using System.Drawing;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using Spire.Pdf.Conversion;
using ExportApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spire.Xls;
using System.Text;

namespace ExportApp
{
    public class PdfFormFillupOperation
    {
        private PdfDocument? pdfDoc = null;
        PdfFont defaultTextFont = new PdfFont(PdfFontFamily.Helvetica, 9.0f, PdfFontStyle.Regular);
        PdfTrueTypeFont checkMarkFont = new PdfTrueTypeFont(new Font("Wingdings", 12.0f, FontStyle.Regular, GraphicsUnit.Pixel));
        PdfTrueTypeFont radionMarkFont = new PdfTrueTypeFont(new Font("Wingdings", 14.0f, FontStyle.Regular, GraphicsUnit.Pixel));
        PdfSolidBrush defaultBrush = new PdfSolidBrush(Color.Black);
        PdfStringFormat format = new PdfStringFormat();
        public PdfFormFillupOperation()
        {

        }

        public PdfFormFillupOperation(string filePath)
        {
            this.LoadPdfFile(filePath);
        }

        public void LoadPdfFile(string filePath)
        {
            this.pdfDoc = new PdfDocument(filePath);
        }

        public void FillUpFormFields(List<FormFieldModel> formFields, string savedFilePath)
        {
            if (this.pdfDoc == null)
            {
                return;
            }
            foreach (FormFieldModel field in formFields)
            {
                this.FillUpFormField(field);
            }
            this.pdfDoc.SaveToFile(savedFilePath);
        }

        private void FillUpFormField(FormFieldModel field)
        {
            PdfPageBase? page = null;
            float fieldValueTextXPos = 0.0f;
            float fieldValueTextYPos = 0.0f;
            PdfTextAlignment fielValueTextAlignment = PdfTextAlignment.Center;
            PdfFontBase? contentFont = null;
            string? searchTextValue = string.Empty;
            SizeF searchTextValueSize;
            RectangleF layoutRect;
            PdfTextFindCollection? textFindCollection = null;
            FieldValueTextPos? fieldValueTextPos = null;

            if (this.pdfDoc == null || field == null)
            {
                return;
            }

            if (field.PageIndex > this.pdfDoc.Pages.Count - 1)
            {
                return;
            }

            page = this.pdfDoc.Pages[field.PageIndex];
            textFindCollection = page.FindAllText();

            if (textFindCollection == null || textFindCollection.Finds.Length == 0)
            {
                return;
            }

            if (field.ValuePosModel != null)
            {
                fieldValueTextPos = getCoordinatesOfFormFieldValueText(field.ValuePosModel, textFindCollection);
                if (fieldValueTextPos != null)
                {
                    fieldValueTextXPos = fieldValueTextPos.X;
                    fieldValueTextYPos = fieldValueTextPos.Y;
                }
            }
            else
            {
                if (field.ValueXPosModel != null)
                {
                    fieldValueTextPos = getCoordinatesOfFormFieldValueText(field.ValueXPosModel, textFindCollection);
                    if (fieldValueTextPos != null)
                    {
                        fieldValueTextXPos = fieldValueTextPos.X;
                        fieldValueTextYPos = fieldValueTextPos.Y;
                    }
                }
                if (field.ValueYPosModel != null)
                {
                    fieldValueTextPos = getCoordinatesOfFormFieldValueText(field.ValueYPosModel, textFindCollection);
                    if (fieldValueTextPos != null)
                    {
                        fieldValueTextXPos += fieldValueTextPos.X;
                        fieldValueTextYPos += fieldValueTextPos.Y;
                    }
                }
            }

            searchTextValue = field.Value != null ? field.Value.ToString() : string.Empty;
            if (field.ValueTextAlignment == FieldValueTextAlignment.Left)
            {
                fielValueTextAlignment = PdfTextAlignment.Left;
            }
            else if (field.ValueTextAlignment == FieldValueTextAlignment.Right)
            {
                fielValueTextAlignment = PdfTextAlignment.Right;
            }
            else if (field.ValueTextAlignment == FieldValueTextAlignment.Center)
            {
                fielValueTextAlignment = PdfTextAlignment.Center;
            }
            else
            {
                fielValueTextAlignment = PdfTextAlignment.Justify;
            }
            contentFont = defaultTextFont;
            if (field.ValueControlType == FieldValueControlType.Checkbox)
            {
                contentFont = checkMarkFont;
            }
            else if (field.ValueControlType == FieldValueControlType.Radiobutton)
            {
                contentFont = radionMarkFont;
            }
            searchTextValueSize = contentFont.MeasureString(searchTextValue, new PdfStringFormat(fielValueTextAlignment));
            layoutRect = new RectangleF(new PointF(fieldValueTextXPos, fieldValueTextYPos), new SizeF(searchTextValueSize.Width, searchTextValueSize.Height));
            page.Canvas.DrawString(searchTextValue, contentFont, defaultBrush, layoutRect);
        }

        private FieldValueTextPos? getCoordinatesOfFormFieldValueText(FormFieldValuePositionModel valuePosModel, PdfTextFindCollection textFindCollection)
        {
            string searchText = string.Empty;
            List<PdfTextFind>? textFindList = null;
            PdfTextFind? textFind = null;
            SizeF searchTextSize;
            float fieldValueTextXPos = 0.0f;
            float fieldValueTextYPos = 0.0f;
            if (valuePosModel == null)
            {
                return null;
            }
            searchText = valuePosModel.SearchText;
            textFindList = textFindCollection.Finds.Where(f => f.SearchText == searchText || f.SearchText.StartsWith(searchText) || f.SearchText.Contains(searchText)).ToList();
            if (textFindList.Count == 0)
            {
                return null;
            }
            textFind = textFindList[valuePosModel.SearchTextIndex];
            if (valuePosModel.IncludeXPosOfSearchText)
            {
                fieldValueTextXPos += textFind.Position.X;
            }
            if (valuePosModel.IncludeYPosOfSearchText)
            {
                fieldValueTextYPos += textFind.Position.Y;
            }
            if (valuePosModel.SearchTextWidthOp != SearchTextDimensionOp.None || valuePosModel.SearchTextHeightOp != SearchTextDimensionOp.None)
            {
                searchTextSize = defaultTextFont.MeasureString(searchText, format);
                if (valuePosModel.SearchTextWidthOp != SearchTextDimensionOp.None)
                {
                    fieldValueTextXPos += valuePosModel.SearchTextWidthOp == SearchTextDimensionOp.Add ? searchTextSize.Width : (searchTextSize.Width * -1);
                }
                if (valuePosModel.SearchTextHeightOp != SearchTextDimensionOp.None)
                {
                    fieldValueTextYPos += valuePosModel.SearchTextHeightOp == SearchTextDimensionOp.Add ? searchTextSize.Height : (searchTextSize.Height * -1);
                }
            }
            if (valuePosModel.AddedWidth > 0)
            {
                fieldValueTextXPos += valuePosModel.AddedWidth;
            }
            if (valuePosModel.SubtractedWidth > 0)
            {
                fieldValueTextXPos -= valuePosModel.SubtractedWidth;
            }
            if (valuePosModel.AddedHeight > 0)
            {
                fieldValueTextYPos += valuePosModel.AddedHeight;
            }
            if (valuePosModel.SubtractedHeight > 0)
            {
                fieldValueTextYPos -= valuePosModel.SubtractedHeight;
            }
            return new FieldValueTextPos { X = fieldValueTextXPos, Y = fieldValueTextYPos };
        }

        public List<PdfTextFind> FindTextLocation(string searchText, int pageIndex)
        {
            PdfPageBase? page = this.pdfDoc.Pages[pageIndex];
            PdfTextFindCollection textFindCollection = page.FindAllText();
            return textFindCollection.Finds.Where(f => f.SearchText == searchText || f.SearchText.StartsWith(searchText) || f.SearchText.Contains(searchText)).ToList();
        }

        public SizeF FindTextSize(string searchText, PdfStringFormat format)
        {
            return defaultTextFont.MeasureString(searchText, format);
        }

        public string wrapText(string text, float textboxWidth, int maxNumOfLines, bool breakWord = true)
        {
            SizeF textSize, charSize;
            string resultText = string.Empty;
            StringBuilder sb = null;
            float tempLineWidth = 0;
            string tempStr = string.Empty;
            int lastIndexOfSpace = -1;
            List<string> subStrs = null;

            resultText = text;
            textSize = this.FindTextSize(text, new PdfStringFormat());

            if (textSize.Width > textboxWidth)
            {
                // double numberOfLines = Math.Ceiling(textSize.Width / textboxWidth);
                sb = new StringBuilder();
                tempLineWidth = 0;
                subStrs = new List<string>();

                for (int i = 0; i < text.Length; i++)
                {
                    sb.Append(text[i]);
                    charSize = this.FindTextSize(text[i].ToString(), new PdfStringFormat());
                    tempLineWidth += charSize.Width;
                    if (tempLineWidth > textboxWidth)
                    {
                        tempStr = sb.ToString();
                        if (!breakWord && tempStr[tempStr.Length - 1].ToString() != string.Empty && i < text.Length - 1)
                        {
                            lastIndexOfSpace = tempStr.LastIndexOf(' ');
                            if (lastIndexOfSpace > -1)
                            {
                                i -= (tempStr.Length - lastIndexOfSpace);
                                tempStr = tempStr.Substring(0, lastIndexOfSpace);
                            }
                        }
                        subStrs.Add(tempStr.Trim());
                        sb.Clear();
                        tempLineWidth = 0;
                        if (subStrs.Count == maxNumOfLines)
                        {
                            break;
                        }
                    }
                }
                if (sb.Length > 0 && subStrs.Count < maxNumOfLines)
                {
                    subStrs.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                resultText = string.Join("\n", subStrs);
            }
            return resultText;
        }
    }
}
