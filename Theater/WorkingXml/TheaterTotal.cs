using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using Theater.Models.Theater;

namespace Theater.WorkingXml
{
    public static class TheaterTotal
    {
        
        public static string StringConnectrion = ConfigurationManager.ConnectionStrings["TheaterXml"].ConnectionString;

        /// <summary>
        /// Read all total information from xml-file about theater in static class
        /// </summary>
        public static void ReadFromXml()
        {
            XmlTextReader reader = null;

            reader = new XmlTextReader(StringConnectrion);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "Seat":
                            {
                                if (reader.GetAttribute("type").Equals("Parterre"))
                                {
                                    TheaterInformation.Parterre.Name = reader.GetAttribute("type");
                                    TheaterInformation.Parterre.CountSeats = Convert.ToInt32(reader.GetAttribute("count"));
                                }
                                else
                                {
                                    TheaterInformation.Balcony.Name = reader.GetAttribute("type");
                                    TheaterInformation.Balcony.CountSeats = Convert.ToInt32(reader.GetAttribute("count"));

                                }
                            }
                            break;
                        case "Play":
                            {
                                TheaterInformation.Prices.AddPrice(new PricePlay(reader.GetAttribute("name"),
                                                                                 Convert.ToDecimal(reader.GetAttribute("priceParterre")),
                                                                                 Convert.ToDecimal(reader.GetAttribute("priceBalcony"))
                                                                                 ));
                            }
                            break;
                    }
                }
            }            
        }


    }
}