using HtmlAgilityPack;
using System;
using System.Net;
using System.Xml;

namespace WebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // URL de la página web a scrapear
            string url = "https://www.fravega.com/";

            // Crea un objeto WebClient para descargar el HTML de la página web
            WebClient webClient = new WebClient();
            string html = webClient.DownloadString(url);
            // Crea un objeto HtmlDocument para parsear el HTML
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            // Busca los elementos con la etiqueta "a" y los imprime
            int i = 0;
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]"))
            {
               // Console.WriteLine(link.Attributes["href"].Value);
               // i++;
            }
            Console.WriteLine(i.ToString());

            // Busca todos los nodos que contienen la palabra "OFERTAS POR TIEMPO LIMITADO"
            var nodosOfertas = document.DocumentNode.SelectNodes("//*[contains(text(), 'OFERTAS POR TIEMPO LIMITADO')]");

            // Imprime el contenido de cada nodo encontrado
            foreach (var nodo in nodosOfertas)
            {
                nodosOfertas.ToCsv()
                Console.WriteLine(nodo.InnerHtml);
            }
           
            Console.ReadLine();
        }
    }
}
