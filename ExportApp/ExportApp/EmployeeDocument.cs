using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using HTMLToQPDF;

namespace ExportApp
{
    public class EmployeeDocument : IDocument
    {
        string[] listItems = ["Blanditiis nulla fugiat eligendi aliquam pariatur.",
            "Dolores architecto sit illo aspernatur aspernatur.",
            "Cupiditate ducimus totam ut veritatis doloribus illo laborum.",
            "Cum dicta mollitia fugit vel fuga.",
            "Sit porro voluptas exercitationem aperiam eligendi quis voluptatem tempore porro.",
            "Neque eum iusto itaque laudantium commodi voluptatem rem inventore suscipit.",
            "Sapiente quidem velit dolore eos ullam quidem soluta veritatis ad perspiciatis.",
            "Corrupti ratione perspiciatis dolorem facere explicabo quam explicabo."];

        DataTable employeeDataTable { get; }

        public EmployeeDocument(DataTable dtEmployee)
        {
            this.employeeDataTable = dtEmployee;
        }

        public void Compose(IDocumentContainer container)
        {          
            container.Page(page =>
            {
                page.Margin(10);
                PageSize pageSize = new PageSize(1800f, 3368f); // new PageSize(1190.7f, 900f); // PageSizes.A4; // 595f, 842f // new PageSize(1800f, 3368f);
                page.Size(pageSize);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent1);
                page.Footer().Element(ComposeFooter);
                // page.ContentFromRightToLeft();
            }).Page(page =>
            {
                page.Margin(15);
                PageSize pageSize = new PageSize(1800f, 3368f); // 595f, 842f
                page.Size(pageSize);
                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent2);
                page.Footer().Element(ComposeFooter);
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Background("#000000").Element(ComposeHeaderContent);
        }

        void ComposeHeaderContent(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().AlignMiddle().Height(80).Padding(15).Row(row =>
                {
                    row.Spacing(15);
                    row.ConstantItem(250).Height(50).AlignLeft().AlignMiddle().Element(ComposeHeaderLogo);
                    row.RelativeItem().Height(50).AlignRight().AlignMiddle().Element(ComponseHeaderTitle);
                });
            });
        }

        void ComposeHeaderLogo(IContainer container)
        {
            // byte[] imageData = File.ReadAllBytes("zuru-media.png");
            // container.Image(imageData);
            container.Row(row =>
            {
                row.Spacing(10);
                row.ConstantItem(40).Height(40).Image("Images/zurumedia-icon.png").WithCompressionQuality(ImageCompressionQuality.High);
                row.ConstantItem(195).Height(40).Image("Images/zurumedia-title.png").WithCompressionQuality(ImageCompressionQuality.High);
            });
        }

        void ComponseHeaderTitle(IContainer container)
        {
            container.AlignRight().Column(column =>
            {
                //column.Spacing(5);
                //column.Item().Text("Phone: 01489 667855").SemiBold().FontColor("#FFFFFF").FontSize(16);
                //column.Item().Text("Email: hello@zurugroup.com").SemiBold().FontColor("#FFFFFF").FontSize(16);

                column.Item().Row(row =>
                {
                    row.ConstantItem(70).AlignLeft().Text("Phone: ").SemiBold().FontColor("#FFFFFF").FontSize(16);
                    row.AutoItem().Text("01489 667855").SemiBold().FontColor("#FF008A").FontSize(16);
                });

                column.Item().Row(row =>
                {
                    row.ConstantItem(70).AlignLeft().Text("Email: ").SemiBold().FontColor("#FFFFFF").FontSize(16);
                    row.AutoItem().Text("hello@zurugroup.com").SemiBold().FontColor("#FF008A").FontSize(16);
                });
            });

            //container.AlignRight().Width(250).Table(table =>
            //{
            //    table.ColumnsDefinition(columns =>
            //    {
            //        columns.ConstantColumn(70);
            //        columns.RelativeColumn();
            //    });

            //    table.Cell().AlignLeft().Text("Phone: ").SemiBold().FontColor("#FFFFFF").FontSize(16);
            //    table.Cell().Text("01489 667855").SemiBold().FontColor("#FF008A").FontSize(16);
            //    table.Cell().AlignLeft().Text("Email: ").SemiBold().FontColor("#FFFFFF").FontSize(16);
            //    table.Cell().Text("hello@zurugroup.com").SemiBold().FontColor("#FF008A").FontSize(16);
            //});
        }

        void ComposeContent1(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                // column.Spacing(5);
                column.Item().Element(ComposeTheme);
                // column.Item().Element(ComposeParagraph);
                // column.Item().Element(ComposeList1);
            });
        }

        void ComposeContent2(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);
                column.Item().Element(ComposeTable);
            });
        }

        void ComposeTheme(IContainer container)
        {
            container.Layers(layers =>
            {
                layers.PrimaryLayer().Width(500).Height(500).TranslateX(-250).TranslateY(-35).Image("Images/pink-bordered-circle.png");

                layers.Layer().Width(500).Height(500).TranslateX(1550).TranslateY(800).Image("Images/pink-bordered-circle.png"); // .TranslateY(2640)

                layers.Layer().Width(1200).TranslateX(250).TranslateY(400).Background("#00FFFFFF").Padding(25).Element(ComposeParagraph);
            });
        }

        void ComposeParagraph(IContainer container)
        {
            container.AlignLeft().AlignTop().PaddingVertical(10).DefaultTextStyle(x => x.FontSize(30)).Text(text =>
            {
                text.Span("At the core of what we do are cross-functional experts in their various fields, when marketing, tech, sales and insight teams come together, great things happen. We work closely with clients to research and really understand your requirements - Surpassing conventional marketing and sales funnels to develop lasting and impactful experiences for your audience.\r\n\r\nBy breaking down traditional working silos and fostering a culture collaboration, we're able to push the boundaries of possibility and delivery truly outstanding results for clients.");
            });
        }

        void ComposeList1(IContainer container)
        {
            container.Column(column =>
             {
                 for (int i = 0; i < listItems.Length; i++)
                 {
                     column.Item().Row(row =>
                     {
                         row.Spacing(5);
                         // row.AutoItem().DefaultTextStyle(x => x.FontSize(16)).Text($"{i + 1}."); // text or image
                         row.AutoItem().DefaultTextStyle(x => x.FontSize(16)).Text(text =>
                         {
                             text.Span("•");
                         });
                         row.RelativeItem().DefaultTextStyle(x => x.FontSize(16)).Text(listItems[i]);
                     });
                 }
             });
        }

        void ComposeTable(IContainer container)
        {
            container.Component(new PDFDataTable(employeeDataTable));

            //container.Table(table =>
            //{
            //    table.ColumnsDefinition(columns =>
            //    {
            //        for (int iCol = 0; iCol < employeeDataTable.Columns.Count; iCol++)
            //        {
            //            columns.RelativeColumn();
            //        }
            //    });

            //    table.Header(header =>
            //    {
            //        header.Cell().RowSpan(2).ShowOnce().Element(container =>
            //        {
            //            return container.DefaultTextStyle(x => x.Bold().FontSize(16).FontColor("#FFFFFF")).Border(1).BorderColor("#F2F2F2").Background("#00B0D9").Height(100).PaddingHorizontal(5).PaddingVertical(5).AlignCenter().AlignMiddle();
            //        }).Text(employeeDataTable.Columns[0].ColumnName);

            //        header.Cell().ColumnSpan(6).ShowOnce().Element(HeaderCellStyle).Text("Primary Details");
            //        header.Cell().ColumnSpan(7).ShowOnce().Element(HeaderCellStyle).Text("Secondary Details");

            //        for (int iCol = 1; iCol < employeeDataTable.Columns.Count; iCol++)
            //        {
            //            header.Cell().ShowOnce().Element(HeaderCellStyle).Text(employeeDataTable.Columns[iCol].ColumnName);
            //        }
            //    });

            //    for (int iRow = 0; iRow < employeeDataTable.Rows.Count; iRow++)
            //    {
            //        for (int iCol = 0; iCol < employeeDataTable.Columns.Count; iCol++)
            //        {
            //            string cellValue = Convert.ToString(employeeDataTable.Rows[iRow][iCol]);
            //            string cellBgColor = "#00000000";
            //            if (employeeDataTable.Columns[iCol].ColumnName == "Annual Salary")
            //            {
            //                long salary = Convert.ToInt64(cellValue.Replace(",", string.Empty).Replace("$", string.Empty));

            //                if (salary <= 100000)
            //                {
            //                    cellBgColor = "#FFFFFF00";
            //                }
            //                else if (salary > 100000 && salary < 150000)
            //                {
            //                    cellBgColor = "#FF0000FF";
            //                }
            //                else
            //                {
            //                    cellBgColor = "#FF008000";
            //                }
            //            }
            //            IContainer container1 = table.Cell().Background(cellBgColor).Element(DataCellStyle);
            //            // IContainer container1 = table.Cell().Element(DataCellStyle);
            //            if (employeeDataTable.Columns[iCol].ColumnName == "EEID")
            //            {
            //                container1.Hyperlink(string.Format("https://www.google.com?EmpId={0}", cellValue)).Text(cellValue).FontColor(Colors.Blue.Accent2).Underline(true);
            //            }
            //            else
            //            {
            //                container1.Text(cellValue);
            //                //container1.Text(text =>
            //                // {
            //                //     text.Span(cellValue).BackgroundColor(cellBgColor);
            //                // });
            //            }
            //        }
            //    }
            //});
        }

        void ComposeFooter(IContainer container)
        {
            container.Background("#000000").Element(ComposeFooterContent);
        }

        void ComposeFooterContent(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().DefaultTextStyle(x => x.SemiBold().FontColor("#FFFFFF").FontSize(16)).AlignMiddle().Height(80).Padding(15).Row(row =>
                {
                    row.ConstantItem(725).AlignLeft().AlignMiddle().Row(row =>
                    {
                        row.RelativeItem().PaddingRight(10).Text("© 2023 Zuru Group Limited");
                        row.RelativeItem().PaddingRight(10).Text("Company number 09236407");
                        row.ConstantItem(55).Text("Part of ");
                        row.RelativeItem().Hyperlink("https://www.zurugroup.com/").Text("Zuru Group").FontColor("#FF008A");
                    });

                    row.RelativeItem().AlignRight().AlignMiddle().Text(text => // .AlignCenter()
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                        text.Span(" of ");
                        text.TotalPages();
                    });
                });
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
