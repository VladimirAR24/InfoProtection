using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;


namespace InfoProtection.Servises
{
    public class PdfService
    {
        public byte[] GeneratePdf(string algorithm, string originalText, string encryptedText, DateTime encryptionDate)
        {
            using (var stream = new MemoryStream())
            {
                // Создаем PDF-документ
                var pdfWriter = new PdfWriter(stream);
                var pdfDocument = new PdfDocument(pdfWriter);
                var document = new Document(pdfDocument);

                // Заголовок
                document.Add(new Paragraph("NE ZBIT PRIDUMAT SUDA TEXT").SetFontSize(16));

                // Основная информация
                document.Add(new Paragraph($"Algorithm: {algorithm}"));
                document.Add(new Paragraph($"Original text: {originalText}"));
                document.Add(new Paragraph($"Encrypted text: {encryptedText}"));
                document.Add(new Paragraph($"Date: {encryptionDate:dd.MM.yyyy HH:mm}"));

                // Закрытие документа
                document.Close();

                return stream.ToArray();
            }
        }
    }

}
