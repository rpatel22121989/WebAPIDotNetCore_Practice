using System.Drawing;

namespace ExportApp.Models
{
    public class FormFieldModel
    {
        public FormFieldModel() { }
        public string? Name { get; set; }
        public object? Value { get; set; }
        public FieldValueControlType ValueControlType { get; set; }
        public int PageIndex { get; set; }
        public FormFieldValuePositionModel? ValuePosModel { get; set; }
        public FormFieldValuePositionModel? ValueXPosModel { get; set; }
        public FormFieldValuePositionModel? ValueYPosModel { get; set; }
        public string? FontName { get; set; }
        public float FontSize { get; set; }
        public FontStyle FontStyle { get; set; }
        public float FontWeight { get; set; }
        public FieldValueTextAlignment ValueTextAlignment { get; set; }
    }

    public class FormFieldValuePositionModel
    {
        public string? SearchText { get; set; }
        public SearchTextFilterType SearchTextFilterType { get; set; }
        public int SearchTextIndex { get; set; }
        public bool IncludeXPosOfSearchText { get; set; }
        public bool IncludeYPosOfSearchText { get; set; }
        public SearchTextDimensionOp SearchTextWidthOp { get; set; }
        public SearchTextDimensionOp SearchTextHeightOp { get; set; }
        public float AddedWidth { get; set; }
        public float SubtractedWidth { get; set; }
        public float AddedHeight { get; set; }
        public float SubtractedHeight { get; set; }
    }

    public enum SearchTextDimensionOp
    {
        None = 0,
        Add = 1,
        Subtract = 2
    }

    public enum SearchTextFilterType
    {
        StartsWith = 1,
        Contains = 2,
        EqualTo = 3
    }

    public enum FieldValueControlType
    {
        Text = 1,
        Checkbox = 2,
        Radiobutton = 3,
        Image = 4,
        Other = 5
    }

    public enum FieldValueTextAlignment
    {
        Left = 1,
        Right = 2,
        Center = 3,
        Justify = 4
    }

    public class FieldValueTextPos
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
