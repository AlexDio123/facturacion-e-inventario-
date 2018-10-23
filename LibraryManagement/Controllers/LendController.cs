using LibraryManagement.Data;
using LibraryManagement.Data.Interfaces;
using LibraryManagement.Reports;
using LibraryManagement.ViewModel;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class LendController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ApplicationDbContext _context;

        public LendController(IBookRepository bookRepository, ICustomerRepository customerRepository, ApplicationDbContext context)
        {
            _bookRepository = bookRepository;
            _customerRepository = customerRepository;
            //Haciendo injection de la clase dbcontext
            _context = context;
        }

        [Route("Lend")]
        public IActionResult List()
        {
            // aqui estoy cargando los libros disponibles
            var availableBooks = _bookRepository.FindWithAuthor(x => x.BorrowerId == 0);
            // recorro el collection
            if (availableBooks.Count() == 0)
            {
                return View("Empty");
            }
            else
            {
                return View(availableBooks);
            }
        }

        public IActionResult LendBook(int bookId)
        {
            //cargo el libro y los usuarios
            var lendVM = new LendViewModel()
            {
                Book = _bookRepository.GetById(bookId),
                Customers = _customerRepository.GetAll()
            };
            // Send data to the Lend view
            return View(lendVM);
        }
       

        [HttpPost]
        public IActionResult LendBook(LendViewModel lendViewModel)
        {
            // aqui actualizo la base de datos
            var book = _bookRepository.GetById(lendViewModel.Book.BookId);
            
            

            var customer = _customerRepository.GetById(lendViewModel.Book.BorrowerId);


            book.Borrower = customer;

            _bookRepository.Update(book);

            // redirijo al list view
            return RedirectToAction(nameof(ImprimirFactura), new { id=book.BookId}); //Despues de prestar un libro el va a imprimir la facutra, imprimir factura quiere un id, el id del libro que se presto así que se lo enviamos 
        }


        public IActionResult ImprimirFactura(int id)
        {

            LendReport LP = new LendReport(); //Hacemos una instancia de la clase que genera el pdf 

            //Ponermos el memory stream en una variable ms, le pueden poner cualquier otro nombre a la variable 
            // EL prepare report exige que le pasemos un book D:
            //Buscando el libro 
            //Include quiere decir que el va a incluir en la variable book, lo que yo le diga, en teste caso author, pero tambien necesitamos el libro y el cliente.
            var book = _context.Books.Include(x=>x.Author).Include(x=>x.Borrower).FirstOrDefault(x=>x.BookId==id);
           
            var ms=LP.PrepareReport(book); //Este metodo devuelve algo, devuelve un memorystream, un memorystream es algo que en su buufer puede guardar información, documentos, fotos, archivos, cosas así.

            var doc = ms.GetBuffer(); //Aquí almacenamos el documento, recuerda que ms es un memorystream que devuelve el metodo preparereport. Los memoryStream tienen un metodo get buffer que te permiten obtener lo que ellos tienen, es decir, el documento o lo que sea que guarden, buffer está en bytes, es decir getbuffer devuelve bytes
            

            return File(doc, "application/pdf"); //Le decimos que queremos que este action devueva un file, es decir un archivo, le pasamos primero el arcivho que queremos que dvuelva que lo tenemos en la variable doc, seguido de un content type, es decir, del tipo de documento en el que queremos que se muestre el archivo, en este caso. pdf see if I add more f
        }

        [HttpPost]
        public IActionResult EnviarCorreo (int id, string correo)
        {
            var book = _context.Books.Include(x => x.Author).Include(x => x.Borrower).FirstOrDefault(x => x.BookId == id);



            if (book.Borrower == null)
            {
                return NotFound();
            }

            LendReport LP = new LendReport();
            var ms = LP.PrepareReport(book); //Este metodo devuelve algo, devuelve un memorystream, un memorystream es algo que en su buufer puede guardar información, documentos, fotos, archivos, cosas así.

            var doc = ms.GetBuffer(); //Aquí almacenamos el documento, recuerda que ms es un memorystream que devuelve el metodo preparereport. Los memoryStream tienen un metodo get buffer que te permiten obtener lo que ellos tienen, es decir, el documento o lo que sea que guarden, buffer está en bytes, es decir getbuffer devuelve bytes

            //Preparando el reporte
           

            //Retornar no email

            ///Preparando mensaje
            ///
            //Destinatario, quien lo envia y el topic
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("CComics", "aquí debes de poner tu email"));
            message.To.Add(new MailboxAddress(correo));

            message.Subject = "Factura de compra en CCcomis";


            //Buscando el saludo xd

            var Hora = DateTime.Now.Hour;

            var Saludos = "";

            if (Hora >= 6 && Hora < 12)
            {
                Saludos = "Buenos días";
            }
            else
            {
                if (Hora >= 12 && Hora < 19)
                {
                    Saludos = "Buenas tardes";
                }
                else
                {
                    Saludos = "Buenas Noches";
                }
            }

            var builder = new BodyBuilder();

            // El cuerpo del mensaje
            builder.TextBody = Saludos+", estimado usuario.  ¿Qué tal te va? \n" +
                "Esta es la factura por el prestamo que ha recibido del libro \n" +
                " ¡Gracias por elegirnos!";

            // Agregarmos aquí el documento a vendiar
            MimeKit.ContentType ct = new MimeKit.ContentType("application", "pdf");
            builder.Attachments.Add("Factura", ms.GetBuffer(), ct);

            //Agregarlos al body todo lo que la clase BodyBuilder nos permitió crear
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("Tu correo", "Tu contrase?A");
                    client.Send(message);
                    client.Disconnect(true);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(List)); // si no encuentra un gmail ni nada, entonces el enviara dnuevo a la lista, pon tu correo y tu contrase?a en los campos que correspondan
                }

            }
            return RedirectToAction(nameof(List));

        }

    }
}
