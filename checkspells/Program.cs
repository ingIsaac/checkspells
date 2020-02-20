using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace checkspells
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.init();
            Console.ReadKey();
        }

        public void init()
        {
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string xml_file = directory + "/spells.xml";
            XDocument d_xml = XDocument.Load(xml_file);
            string[] voc = new string[] { "Master Sorcerer", "Elder Druid", "Royal Paladin", "Elite Knight"};
            foreach(string v in voc)
            {
                Console.WriteLine("Spells for: " + v + "\n\n");
                foreach(var t in getSpellWordsFromXML(d_xml, v))
                {
                    Console.WriteLine(t);
                }
                Console.WriteLine("\n\n");
            }
        }

        public List<string> getSpellWordsFromXML(XDocument doc, string voc)
        {
            List<string> r = new List<string>();
            try
            {
                var instant = (from x in doc.Descendants("spells").Elements("instant").Elements("vocation") where x.Attribute("name").Value == voc select x.Parent);
                var conjure = (from x in doc.Descendants("spells").Elements("conjure").Elements("vocation") where x.Attribute("name").Value == voc select x.Parent);
                foreach (var t in instant.Concat(conjure).Where(x => int.Parse(x.Attribute("lvl").Value) <= 200).OrderBy(x => int.Parse(x.Attribute("lvl").Value)))
                {
                    r.Add("Spell: " + t.Attribute("name").Value + " - Level: " + t.Attribute("lvl").Value + " - Mana: " + t.Attribute("mana").Value);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            return r;
        }
    }
}
