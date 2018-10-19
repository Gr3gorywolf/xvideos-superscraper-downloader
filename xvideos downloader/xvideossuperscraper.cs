using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;

namespace xvideos_downloader
{
    public class xvideossuperscraper
    {


        /*
         note: how to use getdownloadlink
         videourl: is the url of  the video that was scraped from get videos or someone who was introduced by the user
         quality: if you want low resolution put a 0 on quality. If you want to get the hd /sd videos put 1 on quality
             */
        public string getdownloadlink(string videourl, int quality)
        {


            var doc2 = new HtmlAgilityPack.HtmlWeb();
            /////////////se busca la pagina de info de el video 
            var htmlDoc2 = doc2.LoadFromWebAsync(videourl).Result;
            var nodee = htmlDoc2.GetElementbyId("html5video_base");
            var elems = nodee.ChildNodes;
            string link = "";
            if (quality == 0)
                link = elems[1].ChildNodes[0].ChildNodes[0].Attributes["href"].Value;
            else
                link = elems[1].ChildNodes[1].ChildNodes[0].Attributes["href"].Value;


            Console.WriteLine("ok");



            return link;
        }

     /*
      
             how to use getvideos
             Pagecount:is the number of pages that will be scraped 
             querry:is used when you want to search video results that match with the written criteria
             page: is used to load the current page of that criteria
             if you leave querry with the "" it will automatically search results from the home
             .
             as result of the getvideos it will return yo an pagedata who contains the following
            pagedata {
                  navigationmax:is the max number of pages that you can use in the current search
                  videomodels[]: is an array of videomodels who contains some information of each videos that match 
                  with the criteria
            
                 }
             
             */

        public Modals.pagedata getvideos(int pagecount, string querry = "", int page = 0)
        {

            var videos = new List<Modals.videosmodels>();
         Modals.pagedata pagedataa = new Modals.pagedata();
            string baseurl = "";
            if (querry == "")
            {

                baseurl = "http://www.xvideos.com/";
            }
            else
            {
                baseurl = "http://www.xvideos.com/?k=" + querry.Replace(' ', '+');
            }

            for (int i = 0; i < pagecount; i++)
            {

                int pageno = i;
                if (page > 0)
                    pageno = page;
                var doc2 = new HtmlAgilityPack.HtmlWeb();
                HtmlAgilityPack.HtmlDocument htmlDoc2 = null;
                /////////////se busca la pagina de info de el video 
                if (querry != "")
                    htmlDoc2 = doc2.LoadFromWebAsync(baseurl + "&p=" + pageno).Result;
                else
                {
                    if (page > 0)
                        htmlDoc2 = doc2.LoadFromWebAsync(baseurl + "new/" + (pageno + 1) + "/").Result;
                    else
                        htmlDoc2 = doc2.LoadFromWebAsync(baseurl).Result;

                }
                var paginations = htmlDoc2.DocumentNode.SelectNodes("//*[contains(@class,'pagination')]");
                if (!paginations.Last().Attributes["class"].Value.Contains("pagination-with-settings"))
                {

                    var elemsx = paginations.Last().ChildNodes["ul"].ChildNodes;

                    var outfake = 0;
                    var numeros = elemsx.Where(aax => int.TryParse(aax.InnerText, out outfake));

                    pagedataa.navigationmax = int.Parse(numeros.Last().InnerText);
                }
                else
                {
                    pagedataa.navigationmax = 0;
                }
                var elems = htmlDoc2.DocumentNode.SelectNodes("//*[contains(@class,'thumb-block')]");
                foreach (var xd in elems)
                {
                    var elemento = new Modals.videosmodels();
                    elemento.link = "http://www.xvideos.com" + xd.Descendants().Where(aax => aax.Attributes["class"].Value == "thumb").First().ChildNodes["a"].Attributes["href"].Value;
                    if (!elemento.link.Contains("/pornstar-channels/") && !elemento.link.Contains("/model-channels/") && !elemento.link.Contains("/profiles/"))
                    {

                        var elemthumb = xd.Descendants().Where(aax => aax.Attributes["class"].Value == "thumb").First().ChildNodes["a"].ChildNodes["img"];
                        try
                        {
                            elemento.thumb = elemthumb.Attributes["data-src"].Value;
                        }
                        catch (Exception)
                        {
                            elemento.thumb = elemthumb.Attributes["src"].Value;
                        }

                        elemento.title = WebUtility.HtmlDecode(xd.ChildNodes[1].ChildNodes["p"].ChildNodes["a"].Attributes["title"].Value);
                        elemento.duration = xd.ChildNodes[1].ChildNodes[1].ChildNodes["span"].ChildNodes["span"].InnerText;



                        videos.Add(elemento);
                        Console.WriteLine(videos.Count - 1 + "===>" + elemento.title);
                    }
                }







            }




            pagedataa.videos = videos;
            return pagedataa;

        }




    }
}