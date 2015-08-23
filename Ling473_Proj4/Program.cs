using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ling473_Proj4
{
    class Program
    {
        static void Main (string[] args)
        {
            DictionaryNode root = new DictionaryNode();
            DictionaryNode child = new DictionaryNode();
            int i = 0;
            int maxTarget = 0;
            string payload;
            //Create a prie tree
            using (StreamReader sr = new StreamReader("targets"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.ToUpper();
                    maxTarget = Math.Max(maxTarget, line.Length);
                    for (int x = 0; x < line.Length; x++)
                    {
                        char c = line[x];
                        payload = (x == (line.Length - 1)) ? line : "";
                        if (i == 0)
                        {
                            child = GetChild(root, c, payload);
                            i++;
                        }
                        else
                        {
                            child = GetChild(child, c, payload);
                        }
                    }
                    i = 0;
                }
                line = null;
            }
            
            //Search through the prie tree
            string folderPath = @"C:\Users\gilopez\Documents\Visual Studio 2013\Projects\Ling473_Proj4\Ling473_Proj4\TestFolder";
            CheckDir(folderPath);
            Dictionary<String, String> Matches = new Dictionary<String, String>();
            string match = "";
            foreach (string file in Directory.EnumerateFiles(folderPath, "*"))
            {
                
                using (StreamReader reader = new StreamReader(file))
                {
                    StringBuilder sb = new StringBuilder(reader.ReadToEnd());
                    //for (int n = 0; n < sb.Length; n++)
                    //{
                    //    match = SearchTree(root, sb.ToString(n, maxTarget).ToUpper());
                    //    if (!String.IsNullOrEmpty(match))
                    //    {
                    //        Matches.Add(file, match);
                    //    }
                    //}                 
                }
            }


            //PrintTree(root);
            Console.WriteLine("Finished");
            Console.ReadLine();

        }

        //This function searches through a prie tree and returns the string if there was a match or returns empty String
        public static String SearchTree(DictionaryNode root,string testString)
        {
            string result="";
            //if we have payload we are at the leaf of the tree
            if (!String.IsNullOrEmpty(root.Payload))
            {
                return root.Payload;
            }
            // else we have to recurse by checking if current character exists in the tree and send the remaining characters for further recursion
            char c= testString[0];
            if(root.Children.ContainsKey(c))
            {
                result = SearchTree(root.Children[c],testString.Substring(1));
            }
            return result;
        }


        //Ref: https://stackoverflow.com/questions/1073038/c-should-i-throw-an-argumentexception-or-a-directorynotfoundexception/1073349#1073349
        static void CheckDir (string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Incorrect Path");
            }
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }
        }

        public static DictionaryNode GetChild (DictionaryNode root,char value,string payload)
        {
            if (!root.Children.ContainsKey(value))
                root.Children.Add(value, new DictionaryNode(payload));
            return root.Children[value];
        }

        public static void PrintTree (DictionaryNode root)
        {
            
            if(!String.IsNullOrEmpty(root.Payload))
            {
                Console.WriteLine();
            }
            foreach(KeyValuePair<char,DictionaryNode> entry in root.Children )
            {
                Console.Write(entry.Key + "-");
                PrintTree(entry.Value);
            }
            
            return;
        }
    }
}
