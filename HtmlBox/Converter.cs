using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using HTML;
using HtmlAgilityPack;
using System;
using System.Diagnostics;

namespace HtmlBox
{
    public static class Converter
    {
        public static Control ConvertToUI(string htmlPath)
        {
            var doc = new HtmlDocument();
            doc.Load(htmlPath);

            var body = ConvertNodesToUI(doc.DocumentNode);

            if (body == null)
                body = new StackPanel();

            return body;
        }

        public static Control? ConvertNodesToUI(HtmlNode node)
        {
            string atrbs = string.Empty;
            foreach (var atrb in node.Attributes)
                atrbs += '{' + atrb.Name + ':' + atrb.Value + "}, ";
            Debug.WriteLine($"Name:{node.Name} Att:{atrbs} text:{node.InnerText}");

            Control? parent;

            if (node.NodeType == HtmlNodeType.Document)
                parent = new StackPanel();
            else
                parent = ConverNodeToUI(node);

            foreach (var childNode in node.ChildNodes)
            {
                var child = ConvertNodesToUI(childNode);
                if (parent != null && child != null)
                {
                    if (parent is IPanel panel)
                        panel.Children.Add(child);
                }
            }

            return parent;
        }

        public static Control? ConverNodeToUI(HtmlNode node)
        {
            Control? control = null;

            switch (node.Name)
            {
                // == head ==

                case "html": // <html>
                    break;

                case "meta": // <meta>
                    break;

                case "title": // <title>Page title</title>
                    break;
                
                // == Body ==

                case "body": // <body>
                    break;

                case "a": // <a href = ""></a>
                    control = new TextBlock()
                    {
                        Foreground = Brushes.Blue,
                    };
                    var href = node.Attributes["href"];
                    if (href != null)
                        control.Tapped += (s, e) => Process.Start(href.Value);
                    break;

                /*case "abbr": // <abbr>
                    break;*/

                case "b": // <b>Bold</b>
                    control = new TextBlock()
                    {
                        FontWeight = FontWeight.Bold
                    };
                    break;

                case "button": // <button>ClickMe</button>
                    control = new Button()
                    {
                        Content = node.InnerText
                    };
                    break;

                case "div": // <div>
                    control = new StackPanel();
                    break;

                case "i": // <i>Italic</i>
                    control = new TextBlock()
                    {
                        FontStyle = FontStyle.Italic
                    };
                    break;

                case "img": // <img src="" alt="" />
                    var image = new Image();
                    var srcAtrb = node.Attributes["src"];

                    if (srcAtrb != null)
                        Downloader.LoadImage(ref image, srcAtrb.Value);

                    control = image;
                    break;

                case "p": // <p>
                    control = new TextBlock()
                    {
                        Text = node.InnerText
                    };
                    break;

                default:
                    if (node.Name.StartsWith("h") && node.Name.Length == 2)
                    {

                    }

                    break;
            }

            return control;
        }

        /*public static Control ConvertToUI(string htmlFile)
        {
            StackPanel rootPanel = new();
            ParseHTML parse = new()
            {
                Source = htmlFile
            };

            while (!parse.Eof())
            {
                char ch = parse.Parse();
                //Debug.WriteLine(ch);
                if (ch == 0)
                {
                    AttributeList tags = parse.GetTag();
                    foreach(var tag in tags)
                    {
                        if (tag is Attribute att)
                            Debug.WriteLine($"Name:{att.Name}, Value:{att.Value}");

                    }
                }
            }

            return rootPanel;
        }*/
    }
}
