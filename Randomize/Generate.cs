using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KursDataBase.Randomize
{
    public class Generate
    {
        City[] cities;
        Country[] countries;

        public class City
        {
            public int city_id;
            public int country_id;
            public String name;
        }
        public class Country
        {
            public string Name;
            public int id;
        }

        public Generate()
        {
            DeserialaizeCities();
            DeserializeCounties();
        }

        private void DeserializeCounties()
        {
            var arrSerializer = new XmlSerializer(typeof(Country[]));

            using (var reader = new StreamReader("Data/Countries.xml"))
            {
                countries = (Country[])arrSerializer.Deserialize(reader);
            }
        }
        private void DeserialaizeCities()
        {
            var arrSerializer = new XmlSerializer(typeof(City[]));

            using (var reader = new StreamReader("Data/Cities.xml"))
            {
                cities = (City[])arrSerializer.Deserialize(reader);
            }
        }
        public City GetRandomCity()
        {
            return cities[new Random().Next(0, cities.Length)];
        }
        public Country GetCountry(City city)
        {
            return Array.Find(countries, (Country x) => x.id == city.country_id);
        }
    }
}
