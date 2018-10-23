using iTextSharp.text;
using iTextSharp.text.pdf;
using LibraryManagement.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Reports
{
    public class LendReport
    {
        #region Declaracion  
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfTable = new PdfPTable(6); //Aquí tambien en el constructor le pasamos cuantas columnas va a tener 
        PdfPCell _pdfCell;
        MemoryStream _memoryStream = new MemoryStream(); //El memorystream puede almacenar información, sea fotos, docs, etc. 
        #endregion

        public MemoryStream PrepareReport(Book book) //Esto devolvera un memory estream que almacena el documento pdf que haremos. Le pasamos un book para que el se lo pase al cuerpo 
        {
            
            #region
            _document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document.SetPageSize(PageSize.A4);
            _document.SetMargins(20f, 20f, 20f, 20f);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1); //Esto es font de tipo de letra, tama?oy todo eso 
            PdfWriter.GetInstance(_document, _memoryStream);
            _document.Open();
            _pdfTable.SetWidths(new float[] { 10f, 20f, 20f, 20f, 20f, 20f }); //Aquí le decimos que tama?o le estamoas dando a cada columna, termina en f porque es float 
            #endregion

            this.ReportHeader(); //Este metodo para hacer la cabereca de l documento que tambien va a pertenecer a la tabla pero no va tener borde
            this.ReportBody(book); //Esto cmo su nombre lo indica va a hacer el cuerpo. estos metodos no obligatorio llamarase así, pueden ponerle como quieran 
            _pdfTable.HeaderRows = 6;
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream;
        }

        private void ReportHeader()
        {

            //Encabecado que dice factura 
            _fontStyle = FontFactory.GetFont("Tahoma", 25f, 1);
            _pdfCell = new PdfPCell(new Phrase("Factura", _fontStyle)); //Aquí decimos el encabezado y también  el esltilo 
            _pdfCell.Colspan = 6; //Esto es para decirle cuantas columnas tiene de ancho, le ponemos total porque serán todas ya que es el encabezado
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT; //La posición de lafrase, sea centrado, a la derecha, izquieda, justificado etc 
            _pdfCell.Border = 0; //Aquí decimos que no tenga borde porque no queremos borde para el encabezado 
            _pdfCell.BackgroundColor = BaseColor.WHITE; //Color de fondo 
            _pdfCell.ExtraParagraphSpace = 0; //Esto es el espacio entre linea xd
            _pdfTable.AddCell(_pdfCell); //Aquí agregamos la celda que hicimos a la tabla 
            _pdfTable.CompleteRow(); //Cuando decimos completerow entonces inicia con otra fila, es decir, continua creando celdas mas abajo porque arriba queremos en grandote que diga factura y luego siga mas abajo 

            //Espaciado para dejar un espacio de 18 de tama?o y escribir más abajo 
            _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _pdfCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfCell.Colspan = 6; //Ponemos que cubra todas las columnas 
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow(); // Y completamos la fila para que siga más abajo y de un espacio en tre "Factura"y lo que vaya más abajo

            //Nombre de compañía
            _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _pdfCell = new PdfPCell(new Phrase("McBooks", _fontStyle));
            _pdfCell.Colspan = 6;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

            //De nuevo un espaciado 
            //Espaciado para dejar un espacio de 18 de tama?o y escribir más abajo 
            _fontStyle = FontFactory.GetFont("Tahoma", 18f, 1);
            _pdfCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfCell.Colspan = 6; //Ponemos que cubra todas las columnas 
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

            //Dirección
            _fontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _pdfCell = new PdfPCell(new Phrase("Av. México, 802. Santo Domingo Este", _fontStyle)); //Esto es para la dirección del local xd
            _pdfCell.Colspan = 6;
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

            //De nuevo un espaciado 
            //Espaciado para dejar un espacio de 12 de tama?o y escribir más abajo 
            _fontStyle = FontFactory.GetFont("Tahoma", 12f, 1);
            _pdfCell = new PdfPCell(new Phrase(" ", _fontStyle));
            _pdfCell.Colspan = 6; //Ponemos que cubra todas las columnas 
            _pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
            _pdfCell.Border = 0;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();


        }

        private void ReportBody(Book book) //Le entramos un libro porque el libro nos dara toda la información que queremos en la tabla como el cliente, el titulo y el autor miren 
        {
            //Aquí  vamos ya a crear el cuerpo del documento 

            #region Encabezado de tabla 
            _fontStyle = FontFactory.GetFont("Tahoma", 12f, 1); //Aquí hacemos el font que todos van a usar 
            _pdfCell = new PdfPCell(new Phrase("Título de libro", _fontStyle));
            _pdfCell.Colspan = 2; //Aquí decimos cuantas columnas va a abarcar 
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.BLUE;
            _pdfCell.Border = 1;
            _pdfTable.AddCell(_pdfCell);


            _pdfCell = new PdfPCell(new Phrase("Autor", _fontStyle));
            _pdfCell.Colspan = 2; //Aquí decimos cuantas columnas va a abarcar 
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.BLUE;
            _pdfCell.Border = 1;
            _pdfTable.AddCell(_pdfCell);



            _pdfCell = new PdfPCell(new Phrase("Cliente", _fontStyle));
            _pdfCell.Colspan = 2; //Aquí decimos cuantas columnas va a abarcar 
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER; //Aquí la alineación del texto
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE; //Aquí tambien pero en relacion a la vertial 
            _pdfCell.BackgroundColor = BaseColor.BLUE;
            _pdfCell.Border = 1; //Ya si le ponemos border 1 porque si queremos borde
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();//Cada vez que queremos saltar a otra linea usamos eso. Ya como hicimos los titulos de la tabla entonces queremos poner la información por lo tanto usamos completerow
            #endregion


            //Aquí empezamos  tomar información de la base de datos 


            #region Cuerpo de la tabla
            _fontStyle = FontFactory.GetFont("Tahoma", 12f, 0); //Aquí iniciamos una instancia de font con el estilo que queremos, el tipo le letra, el tama?o y el ultimo numero que ven el 0 dice que no es negrita, un 1 significa que si 

            //Aquí el titulo del libro 
            _pdfCell = new PdfPCell(new Phrase(book.Title, _fontStyle)); //Aquí siempre ponemos la información que queremos mostrar y el estilo
            _pdfCell.Colspan = 2;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.Border = 1;
            _pdfCell.BorderColor = BaseColor.BLACK;
            _pdfTable.AddCell(_pdfCell);

            //Aquí el autor del libro 

            _pdfCell = new PdfPCell(new Phrase(book.Author.Name, _fontStyle));
            _pdfCell.Colspan = 2;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.Border = 1;
            _pdfCell.BorderColor = BaseColor.BLACK;
            _pdfTable.AddCell(_pdfCell);


            //Aquí el cliente que está tomando el libro prestado why?
            _pdfCell = new PdfPCell(new Phrase(book.Borrower.Name, _fontStyle));
            _pdfCell.Colspan = 2;
            _pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfCell.BackgroundColor = BaseColor.WHITE;
            _pdfCell.Border = 1;
            _pdfCell.BorderColor = BaseColor.BLACK;
            _pdfTable.AddCell(_pdfCell);
            _pdfTable.CompleteRow();

         

            #endregion
        }


    }
    }
