using System.Collections;
using System.Collections.Generic;
using Spire.Pdf;
using Spire.Pdf.Widget;
using ExportApp.Models;

namespace ExportApp
{
    public class PdfFormOperation
    {
        private PdfDocument? pdfDoc = null;
        public PdfFormOperation(string filePath)
        {
            this.LoadPdfFile(filePath);
        }

        public void LoadPdfFile(string filePath)
        {
            this.pdfDoc = new PdfDocument(filePath);
        }

        public void FillUpFormFields(List<PdfFormFieldWidgetControl> pdfFormFieldWidgetControls, string savedFilePath)
        {
            if (this.pdfDoc == null || this.pdfDoc.Form == null)
            {
                return;
            }
            PdfFormWidget pdfFormWidget = (PdfFormWidget)this.pdfDoc.Form;
            PdfFormFieldWidgetCollection pdfFormFieldWidgets = pdfFormWidget.FieldsWidget;
            List<string> fieldNames = pdfFormFieldWidgets.FieldNames;
            IList pdfFormFieldWidgetsList = pdfFormFieldWidgets.List;
            int widgetIndex = -1;
            PdfTextBoxFieldWidget? pdfTextBoxFieldWidget = null;
            PdfCheckBoxWidgetFieldWidget? pdfCheckBoxWidgetFieldWidget = null;
            PdfRadioButtonListFieldWidget? pdfRadioButtonListFieldWidget = null;
            PdfComboBoxWidgetFieldWidget? pdfComboBoxWidgetFieldWidget = null;
            if (pdfFormFieldWidgetsList != null && pdfFormFieldWidgetsList.Count > 0)
            {
                foreach (PdfFormFieldWidgetControl pdfFormFieldWidgetControl in pdfFormFieldWidgetControls)
                {
                    if (pdfFormFieldWidgetControl != null)
                    {
                        widgetIndex = fieldNames.IndexOf(pdfFormFieldWidgetControl.Name);
                        if (widgetIndex > -1)
                        {
                            object pdfFormFieldWidget = pdfFormFieldWidgetsList[widgetIndex];
                            if (pdfFormFieldWidget is PdfTextBoxFieldWidget)
                            {
                                pdfTextBoxFieldWidget = (PdfTextBoxFieldWidget)pdfFormFieldWidget;
                                pdfTextBoxFieldWidget.Text = Convert.ToString(pdfFormFieldWidgetControl.Value);
                            }
                            else if (pdfFormFieldWidget is PdfCheckBoxWidgetFieldWidget)
                            {
                                pdfCheckBoxWidgetFieldWidget = (PdfCheckBoxWidgetFieldWidget)pdfFormFieldWidget;
                                if (pdfCheckBoxWidgetFieldWidget.WidgetWidgetItems.Count > 1)
                                {
                                    pdfCheckBoxWidgetFieldWidget.DefaultIndex = pdfFormFieldWidgetControl.SelectedItemIndex;
                                    //for (int i = 0; i < pdfCheckBoxWidgetFieldWidget.WidgetWidgetItems.Count; i++)
                                    //{
                                    //    pdfCheckBoxWidgetFieldWidget.WidgetWidgetItems[i].Checked = false;
                                    //    if (i == pdfCheckBoxWidgetFieldWidget.DefaultIndex)
                                    //    {
                                    //        pdfCheckBoxWidgetFieldWidget.WidgetWidgetItems[i].Checked = true;
                                    //    }
                                    //}
                                }
                                pdfCheckBoxWidgetFieldWidget.Checked = Convert.ToBoolean(pdfFormFieldWidgetControl.Value);

                            }
                            else if (pdfFormFieldWidget is PdfRadioButtonListFieldWidget)
                            {
                                pdfRadioButtonListFieldWidget = (PdfRadioButtonListFieldWidget)pdfFormFieldWidget;
                                pdfRadioButtonListFieldWidget.SelectedValue = Convert.ToString(pdfFormFieldWidgetControl.Value);
                                foreach (PdfRadioButtonWidgetItem item in pdfRadioButtonListFieldWidget.WidgetWidgetItems)
                                {
                                    if (item.Value == Convert.ToString(pdfFormFieldWidgetControl.Value))
                                    {
                                        item.Checked = true;
                                        item.Selected = true;
                                    }
                                }
                            }
                            else if (pdfFormFieldWidget is PdfComboBoxWidgetFieldWidget)
                            {
                                pdfComboBoxWidgetFieldWidget = (PdfComboBoxWidgetFieldWidget)pdfFormFieldWidget;
                                pdfComboBoxWidgetFieldWidget.SelectedValue = Convert.ToString(pdfFormFieldWidgetControl.Value);
                            }
                        }
                    }
                }
            }
            this.pdfDoc.SaveToFile(savedFilePath);
        }
    }
}
