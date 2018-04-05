using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AmazonBot
{
    class Program
    {
        const string _URL = "https://www.amazon.com/PCS-58mm-Lens-Canon-Replaces/dp/B01HXQ968A/ref=br_msw_pdt-6?_encoding=UTF8&smid=ATVPDKIKX0DER&pf_rd_m=ATVPDKIKX0DER&pf_rd_s=&pf_rd_r=9YGM8K9QE1J8P26VWEAW&pf_rd_t=36701&pf_rd_p=abda0da5-c6a7-4039-a2ae-76a66e9c69ce&pf_rd_i=desktop";
        static void Main(string[] args)
        {
            Console.WriteLine(GetProductName(_URL));
            Console.WriteLine(GetQuantity(_URL));
            Console.ReadLine();
        }

        static string GetContent(string urlAdress)
        {
            Thread.Sleep(500);
            Uri url = new Uri(urlAdress);
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            string html = client.DownloadString(url);
            return html;
        }

        static string CleanElement(string element)
        {
            return element.Replace("\n", "");
        }

        static string GetQuantity(string url)
        {
            string htmlContent = GetContent(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlContent);

            var node = document.DocumentNode.SelectSingleNode("//*[@id='quantity']");

            //Buraya bir bak ufak bir düzenleme gerek ama sanırım genel olarak istedikleri şey bu benim anladığım
            return node == null ? "1" :
                node.ChildNodes
                .Where(x => x.Name == "option")
                .Max(x => int.Parse(x.Attributes["value"].Value)).ToString();
        }

        static string GetProductName(string url)
        {

            string htmlContent = GetContent(url);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlContent);

            var node = document.DocumentNode.SelectSingleNode("//*[@id='productTitle']");
            if (node == null)
                return GetProductName(_URL);

            return node.InnerText;

        }
    }
}
