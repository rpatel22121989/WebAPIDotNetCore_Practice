using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System.Data;

namespace ExportApp
{
    public class PDFDataTable: IComponent
    {
        private DataTable dataTable;

        public PDFDataTable(DataTable dt) 
        {
            dataTable = dt;
        }   

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    for (int iCol = 0; iCol < dataTable.Columns.Count; iCol++)
                    {
                        columns.RelativeColumn();
                    }
                });

                table.Header(header =>
                {
                    header.Cell().RowSpan(2).ShowOnce().Element(container =>
                    {
                        return container.DefaultTextStyle(x => x.Bold().FontSize(16).FontColor("#FFFFFF")).Border(1).BorderColor("#F2F2F2").Background("#00B0D9").Height(100).PaddingHorizontal(5).PaddingVertical(5).AlignCenter().AlignMiddle();
                    }).Text(dataTable.Columns[0].ColumnName);

                    header.Cell().ColumnSpan(6).ShowOnce().Element(HeaderCellStyle).Text("Primary Details");
                    header.Cell().ColumnSpan(7).ShowOnce().Element(HeaderCellStyle).Text("Secondary Details");

                    for (int iCol = 1; iCol < dataTable.Columns.Count; iCol++)
                    {
                        header.Cell().ShowOnce().Element(HeaderCellStyle).Text(dataTable.Columns[iCol].ColumnName);
                    }
                });

                for (int iRow = 0; iRow < dataTable.Rows.Count; iRow++)
                {
                    for (int iCol = 0; iCol < dataTable.Columns.Count; iCol++)
                    {
                        string cellValue = Convert.ToString(dataTable.Rows[iRow][iCol]);
                        string cellBgColor = "#00000000";
                        if (dataTable.Columns[iCol].ColumnName == "Annual Salary")
                        {
                            long salary = Convert.ToInt64(cellValue.Replace(",", string.Empty).Replace("$", string.Empty));

                            if (salary <= 100000)
                            {
                                cellBgColor = "#FFFFFF00";
                            }
                            else if (salary > 100000 && salary < 150000)
                            {
                                cellBgColor = "#FF0000FF";
                            }
                            else
                            {
                                cellBgColor = "#FF008000";
                            }
                        }
                        IContainer container1 = table.Cell().Background(cellBgColor).Element(DataCellStyle);
                        // IContainer container1 = table.Cell().Element(DataCellStyle);
                        if (dataTable.Columns[iCol].ColumnName == "EEID")
                        {
                            container1.Hyperlink(string.Format("https://www.google.com?EmpId={0}", cellValue)).Text(cellValue).FontColor(Colors.Blue.Accent2).Underline(true);
                        }
                        else
                        {
                            container1.Text(cellValue);
                            //container1.Text(text =>
                            // {
                            //     text.Span(cellValue).BackgroundColor(cellBgColor);
                            // });
                        }
                    }
                }
            });
        }

        IContainer HeaderCellStyle(IContainer container)
        {
            return container.DefaultTextStyle(x => x.Bold().FontSize(16).FontColor("#FFFFFF")).Border(1).BorderColor("#F2F2F2").Background("#00B0D9").Height(50).PaddingHorizontal(5).PaddingVertical(5).AlignCenter().AlignMiddle();
        }

        IContainer DataCellStyle(IContainer container)
        {
            return container.DefaultTextStyle(x => x.FontSize(14).FontColor("#6E7B7E")).Border(1).BorderColor("#F2F2F2").Background("#FFFFFF").PaddingHorizontal(5).PaddingVertical(5).AlignCenter().AlignMiddle();
        }
    }
}
