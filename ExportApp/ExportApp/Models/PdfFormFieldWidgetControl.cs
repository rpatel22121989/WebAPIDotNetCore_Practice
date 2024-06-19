namespace ExportApp.Models
{
    public class PdfFormFieldWidgetControl
    {
        public PdfFormFieldWidgetControl() { }

        public string? Name { get; set; }
        public object? Value { get; set; }
        public int SelectedItemIndex { get; set; }
        public EPDFFormFieldWidgetControlType ControlType { get; set; }
    }

    public enum EPDFFormFieldWidgetControlType
    {
        Checkbox = 1,
        Radiobutton = 2,
        Textbox = 3,
        Combobox = 4
    }
}
