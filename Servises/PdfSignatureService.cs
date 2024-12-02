using System;
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Signatures;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using iText.Commons.Bouncycastle.Crypto;
using iText.Commons.Bouncycastle.Cert;
using iText.Kernel.Crypto;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using GroupDocs.Signature.Domain.Extensions;
using GroupDocs.Signature.Domain;
using GroupDocs.Signature.Options;
using GroupDocs.Signature;
using Org.BouncyCastle.Utilities.Zlib;

namespace InfoProtection.Servises;

public class PdfSignatureService
{
    /// <summary>
    /// Создаёт PDF-документ и подписывает его.
    /// </summary>
    /// <param name="content">Содержимое документа.</param>
    /// <param name="reason">Причина подписи.</param>
    /// <param name="location">Местоположение подписи.</param>
    /// <returns>Подписанный PDF в виде массива байт.</returns>
    public byte[] CreateAndSignPdf(string algorithm, string originalText, string encryptedText, DateTime encryptionDate)
    {
        using var memoryStream = new MemoryStream();

        // Создание PDF-документа
        var writer = new PdfWriter(memoryStream);
        var pdfDocument = new PdfDocument(writer);
        var document = new Document(pdfDocument);

        // Добавление содержимого
        document.Add(new Paragraph("Document")).SetFontSize(12);

        document.Add(new Paragraph($"Algorithm: {algorithm}"));
        document.Add(new Paragraph($"Original text: {originalText}"));
        document.Add(new Paragraph($"Encrypted Text: {encryptedText}"));
        document.Add(new Paragraph($"Date: {encryptionDate:dd.MM.yyyy HH:mm}"));

        document.Close(); // Закрыть документ перед подписью

        // Подпись PDF
        //return SignPdf(memoryStream.ToArray());
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Подписывает PDF-документ.
    /// </summary>
    /// <param name="pdfBytes">PDF-документ в виде массива байт.</param>
    /// <param name="reason">Причина подписи.</param>
    /// <param name="location">Местоположение подписи.</param>
    /// <returns>Подписанный PDF в виде массива байт.</returns>
    private byte[] SignPdf(byte[] pdfBytes)
    {
        // 1. Загружаем исходный PDF из массива байт
        using (var memoryStream = new MemoryStream(pdfBytes))
        {
            // Подготовка второго потока для записи
            using (var outputStream = new MemoryStream())
            {
                // 2. Создаем объект Signature для работы с документом
                Signature signature = new Signature(memoryStream);

                // 3. Настроим параметры подписи
                // Создание вариантов QR-кода с заранее заданным текстом
                //QrCodeSignOptions options = new QrCodeSignOptions("The document is approved by John Smith")
                //{
                //    // Настройте тип и положение кодировки QR-кода на странице.
                //    EncodeType = QrCodeTypes.QR,
                //    Left = 100,
                //    Top = 100
                //};
                DigitalSignOptions options = new DigitalSignOptions("C:\\Windows\\System32\\certificate.pfx")
                {
                    // Установите пароль сертификата
                    Password = "12345"
                };

                // 4. Подписываем документ
                var signedDocument = signature.Sign(outputStream, options);

                // 5. Возвращаем подписанный документ как массив байт
                
                return outputStream.ToArray();
            }
        }
    }
    //private byte[] EmbedSignatureInPdf(byte[] pdfBytes, byte[] signatureBytes, string reason, string location)
    //{
    //    // Загрузите документ для подписи
    //    using (Signature signature = new Signature("file_to_sign.pdf"))
    //    {
    //        // Создание вариантов QR-кода с заранее заданным текстом
    //        QrCodeSignOptions options = new QrCodeSignOptions("The document is approved by John Smith")
    //        {
    //            // Настройте тип и положение кодировки QR-кода на странице.
    //            EncodeType = QrCodeTypes.QR,
    //            Left = 100,
    //            Top = 100
    //        };
    //        // Подпишите документ и сохраните его как файл результатов.
    //        signature.Sign("file_with_QR.pdf", options);
    //    }



    //    return outputStream.ToArray(); // Возвращаем подписанный PDF как массив байт
    //}



    // Реализация собственного контейнера подписи
    private class CustomExternalSignatureContainer : IExternalSignatureContainer
    {
        private readonly byte[] _signature;

        public CustomExternalSignatureContainer(byte[] signature)
        {
            _signature = signature;
        }

        public void ModifySigningDictionary(PdfDictionary signDic)
        {
            // Добавляем информацию о методе подписи
            signDic.Put(PdfName.Filter, PdfName.Adobe_PPKLite);
            signDic.Put(PdfName.SubFilter, PdfName.Adbe_pkcs7_detached);
        }

        public byte[] Sign(Stream data)
        {
            // Возвращаем существующую подпись
            return _signature;
        }
    }


}
