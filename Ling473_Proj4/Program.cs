using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            int minTarget = int.MaxValue;
            string payload;
            int count_target = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            string TargetFile = (args.Length > 0) ? @args[0] :@"/opt/dropbox/15-16/473/project4/targets"; 
            //Create a prie tree
            using (StreamReader sr = new StreamReader(TargetFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.ToUpper();
                    count_target++;
                    maxTarget = Math.Max(maxTarget, line.Length);
                    minTarget = Math.Min(minTarget, line.Length);
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
            timer.Stop();
            Console.WriteLine("total number of targets is " + count_target);
            Console.WriteLine("timeElapsed for building a tree : "+timer.ElapsedMilliseconds);

            timer.Start();
            //Search through the prie tree
            string SequenceFolder = (args.Length>1) ? @args[1]: @"/opt/dropbox/15-16/473/project4/hg19-GRCh37";
            CheckDir(SequenceFolder);

            
            string match = "";
            Dictionary<String, List<String>> Matches = new Dictionary<string, List<string>>();
            foreach (string file in Directory.EnumerateFiles(SequenceFolder, "*"))
            {

                using (StreamReader reader = new StreamReader(file))
                {
                    StringBuilder sb = new StringBuilder(reader.ReadToEnd());

                    string fileName = Path.GetFileName(file); 
                    int sbLength = sb.Length;

                    for (int n = 0; n < sbLength; n++)
                    {
                        int displacement = Math.Min(sbLength - n, maxTarget);
                        //if length of remaining sequence is less than mintarget size we can leave
                        if(displacement<minTarget)
                            break;
                        match = SearchTree(root, sb.ToString(n, displacement));
                        if (!String.IsNullOrEmpty(match))
                        {
                            if (Matches.ContainsKey(match))
                                Matches[match].Add(n + "\t" + fileName);
                            else
                                Matches.Add(match, new List<String>(new String[] { n + "\t" + fileName }));
                        }
                    }                 
                }
            }
            
            timer.Stop();
            StreamWriter writer = new StreamWriter("output.txt");
            writer.WriteLine("timeElapsed for running through the tree : " + timer.ElapsedMilliseconds);
            foreach(KeyValuePair<String,List<String>> entry in Matches)
            {
                writer.WriteLine(entry.Key);
                foreach(String s in entry.Value)
                {
                    writer.WriteLine(s);
                }
            }
            writer.Close();
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
            Char c= Char.ToUpper(testString[0]);
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
