using ExportApp.Models;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ExportApp
{
    public class InvoiceDocument : IDocument
    {
        public InvoiceModel Model { get; }

        public InvoiceDocument(InvoiceModel model)
        {
            Model = model;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
                {
                    page.Margin(50);
                    // page.Header().Height(100).Background(Colors.Grey.Lighten1);
                    page.Header().Element(ComposeHeader);
                    // page.Content().Background(Colors.Grey.Lighten3);
                    page.Content().Element(ComposeContent);
                    // page.Footer().Height(50).Background(Colors.Grey.Lighten1);
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            TextStyle titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"Invoice #{Model.InvoiceNumber}").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").SemiBold();
                        text.Span($"{Model.IssueDate:d}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Due date: ").SemiBold();
                        text.Span($"{Model.DueDate:d}");
                    });
                });

                row.ConstantItem(100).Height(50).Placeholder();
            });
        }

        void ComposeContent(IContainer container)
        {
            // container.PaddingVertical(40).Height(250).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text("Content").FontSize(16);
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);
                column.Item().Element(ComposeTable);

                decimal totalPrice = Model.Items.Sum(x => x.Price * x.Quantity);
                column.Item().AlignRight().Text($"Grand total: {totalPrice}$").FontSize(14);

                if (!string.IsNullOrWhiteSpace(Model.Comments))
                {
                    column.Item().PaddingTop(25).Element(ComposeComments);
                }
            });
        }

        void ComposeTable(IContainer container)
        {
            // container.Height(250).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle().Text("Table").FontSize(16);
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        columns.RelativeColumn();
                    }
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCellStyle).Text("#");
                    header.Cell().Element(HeaderCellStyle).Text("Product");
                    header.Cell().Element(HeaderCellStyle).AlignRight().Text("Unit price");
                    header.Cell().Element(HeaderCellStyle).AlignRight().Text("Quantity");
                    header.Cell().Element(HeaderCellStyle).AlignRight().Text("Total");
                });

                foreach (OrderItem item in Model.Items)
                {
                    table.Cell().Element(DataCellStyle).Text($"{Model.Items.IndexOf(item) + 1}");
                    table.Cell().Element(DataCellStyle).Text(item.Name);
                    table.Cell().Element(DataCellStyle).AlignRight().Text($"{item.Price}$");
                    table.Cell().Element(DataCellStyle).AlignRight().Text($"{item.Quantity}");
                    table.Cell().Element(DataCellStyle).AlignRight().Text($"{item.Price * item.Quantity}");
                }
            });
        }

        IContainer HeaderCellStyle(IContainer container)
        {
            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
        }

        IContainer DataCellStyle(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
        }

        void ComposeComments(IContainer container)
        {
            container.Background(Colors.Green.Lighten3).Padding(10).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text("Comments").FontSize(14);
                column.Item().Text(Model.Comments);
            });
        }
    }
}
