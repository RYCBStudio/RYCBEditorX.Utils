using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RYCBEditorX.Utils;
public static class PypiHelper
{
    public static (Dictionary<string, List<string>>, string) ParseLinkWithContent(string html)
    {
        Dictionary<string, List<string>> res = [];
        var doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(html);
        var linkNodes = doc.DocumentNode.SelectNodes("//span[@class='package-snippet__name']");
        var names = new List<string>();
        var descs = new List<string>();
        var vers = new List<string>();
        if (linkNodes is null) { return (null, "0"); }
        foreach (var linkNode in linkNodes)
        {
            var link = linkNode.GetDirectInnerText();
            names.Add(link);

        }
        linkNodes = doc.DocumentNode.SelectNodes("//p[@class='package-snippet__description']");
        foreach (var linkNode in linkNodes)
        {
            var link = linkNode.GetDirectInnerText();
            descs.Add(link);
        }
        linkNodes = doc.DocumentNode.SelectNodes("//span[@class='package-snippet__version']");
        foreach (var linkNode in linkNodes)
        {
            var link = linkNode.GetDirectInnerText();
            vers.Add(link);
        }
        linkNodes = doc.DocumentNode.SelectNodes("//a[@class='button button-group__button']");
        var max = 0;
        if (linkNodes is not null)
        {
            foreach (var node in linkNodes)
            {
                var link = node.GetDirectInnerText();
                try
                {
                    if (max <= Convert.ToInt32(link))
                    {
                        max = Convert.ToInt32(link);
                    }
                }
                catch { }
            }
        }
        else
        {
            max = 1;
        }
        res.Add("name", names);
        res.Add("desc", descs);
        res.Add("ver", vers);
        return (res, max.ToString());
    }

    public static async Task<string> GetHtml(string url)
    {
        var res = "";
        var client = new HttpClient();
        var stream = await client.GetStreamAsync(url);
        var sr = new StreamReader(stream, encoding: Encoding.UTF8);
        res = sr.ReadToEnd();
        sr.Close();
        client.Dispose();
        return res;
    }

    public static List<Dictionary<string, string>> ConvertToDict(Dictionary<string, List<string>> res)
    {
        List<Dictionary<string, string>> ret = new(res["name"].Count);

        var names = res["name"];
        var descs = res["desc"];
        var vers = res["ver"];
        foreach (var val in names)
        {
            Dictionary<string, string> tmp = new()
                {
                    {
                        "name",
                        val
                    },
                    {
                        "desc",
                        descs[names.IndexOf(val)]
                    },
                    {
                        "ver",
                        vers[names.IndexOf(val)]
                    }
                };
            ret.Add(tmp);
        }

        return ret;
    }

    public static List<GeneralPackageInfo> ConvertToClass(Dictionary<string, List<string>> res)
    {
        if (res == null)
        {
            return [];
        }
        List<GeneralPackageInfo> ret = new(res["name"].Count);
        var i = 0;

        var names = res["name"];
        var descs = res["desc"];
        var vers = res["ver"];
        foreach (var val in names)
        {
            GeneralPackageInfo tmp = new()
            {
                Name = val,
                Description =
                    descs[names.IndexOf(val)],
                Version =
                    vers[names.IndexOf(val)],
            };
            ret.Add(tmp);
        }

        return ret;
    }
}