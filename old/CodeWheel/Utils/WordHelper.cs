using CodeWheel.Infrastructure.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.XWPF.Model;
using NPOI.XWPF.Extractor;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using System.IO;

namespace CodeWheel.Utils
{
    public class WordHelper
    {
        /// <summary>
        /// 导出word文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static bool ExportDBWordFile(string savePath, List<TableMeta> tables)
        {
            XWPFDocument doc = new XWPFDocument();
            doc.Document.body.sectPr = new CT_SectPr();
            //doc.Document.body.sectPr.pgSz.h = 16838;
            doc.Document.body.sectPr.pgSz.w = 11906;
            doc.Document.body.sectPr.pgMar.left = 1000;
            doc.Document.body.sectPr.pgMar.right = 1000;
            doc.Document.body.sectPr.pgMar.top = "150";
            doc.Document.body.sectPr.pgMar.bottom = "150";


            //标题
            XWPFParagraph p = doc.CreateParagraph();
            p.Alignment = ParagraphAlignment.CENTER;
            XWPFRun r = p.CreateRun();
            r.SetText("数据库文档");
            r.FontSize = 16;
            r.IsBold = true;


            for (int i = 0; i < tables.Count; i++)
            {
                Int32 index = i + 1;
                TableMeta current = tables[i];
                SetTableWord(doc, current, index);
            }


            try
            {
                using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    doc.Write(fs);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置数据文档的表
        /// </summary>
        /// <param name="document">文档</param>
        /// <param name="table">当前表</param>
        /// <param name="no">当前表编号,从1开始</param>
        public static void SetTableWord(XWPFDocument document, TableMeta table, Int32 no)
        {
            //表名 
            XWPFParagraph p = document.CreateParagraph();
            p.Alignment = ParagraphAlignment.LEFT;
            XWPFRun r = p.CreateRun();
            r.SetText($"{no}.{table.TableName}");
            r.FontSize = 14;
            r.IsBold = true;

            if (!string.IsNullOrEmpty(table.Comment))
            {
                //表注释
                p = document.CreateParagraph();
                p.Alignment = ParagraphAlignment.LEFT;
                r = p.CreateRun();
                r.SetText(table.Comment);
                r.FontSize = 14;
                r.IsBold = true;
            }
            

            //表格
            XWPFTable grid=document.CreateTable(table.Columns.Count+1,5);
            
            

            grid.Width = 2500;
            grid.SetColumnWidth(0, 256 * 2);
            grid.SetColumnWidth(1, 256 * 2);
            grid.SetColumnWidth(2, 256 * 1);
            grid.SetColumnWidth(3, 256 * 1);
            grid.SetColumnWidth(4, 256 * 4);

            

            //设置表头
            XWPFTableRow row = grid.GetRow(0);
            row.GetCell(0).SetParagraph(SetCellText(document, grid, "字段名"));
            row.GetCell(1).SetParagraph(SetCellText(document, grid, "类型"));
            row.GetCell(2).SetParagraph(SetCellText(document, grid, "是否主键"));
            row.GetCell(3).SetParagraph(SetCellText(document, grid, "可为空"));
            row.GetCell(4).SetParagraph(SetCellText(document, grid, "说明"));

            for (int i = 0; i < table.Columns.Count; i++)
            {
                ColumnMeta col = table.Columns[i];
                row = grid.GetRow(i + 1);
                row.GetCell(0).SetParagraph(SetCellText(document, grid, col.ColumnName));
                row.GetCell(1).SetParagraph(SetCellText(document, grid, col.FieldTypeName));
                row.GetCell(2).SetParagraph(SetCellText(document, grid, col.IsKey ? "是" : "否"));
                row.GetCell(3).SetParagraph(SetCellText(document, grid, col.AllowDBNull ? "是" : "否"));
                row.GetCell(4).SetParagraph(SetCellText(document, grid, string.IsNullOrEmpty(col.Comment)?string.Empty:col.Comment));
            }
        }

        //设置字体样式
        public static XWPFParagraph SetCellText(XWPFDocument doc, XWPFTable table, string text)
        {
            //table中的文字格式设置  
            XWPFParagraph cell = new XWPFParagraph(new CT_P(),table.Body);
            cell.Alignment = ParagraphAlignment.CENTER;//字体居中  
            cell.VerticalAlignment = TextAlignment.CENTER;//字体居中  

            XWPFRun r = cell.CreateRun();
            r.SetText(text);
            r.FontSize = 12;
            r.FontFamily = "华文楷体";
            r.IsBold = true;

            //r1c1.SetTextPosition(20);//设置高度  
            return cell;
        }
    }
}
