using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
namespace ConsoleApplication3
{
    class Program
    {
        private Dictionary<char, char> jobs = new Dictionary<char, char>();  //to store key value pair of first => second job
        private ArrayList jobSequence = new ArrayList(); //array for keeping track of completed job
        private ArrayList duplicate = new ArrayList(); // array for keeping track of circulare dependency . During program execution 
                                                       // if this array contain any duplicate then there is circulare dependency
        ArrayList temp;
        private int addJob(char key)
        {
            /*
            This function a single job as input
            */

            //check if job has no dependency then add it to jobSequence
            if(jobs[key] == ' ') {
                jobSequence.Add(key);
                temp.Remove(key);
                return 1;
            }
            else
            {
                //if job is dependent on another job

                //if dependent job has been completed then add it to jobSequence
                if (jobSequence.Contains(jobs[key]))
                {
                    jobSequence.Add(key);
                    temp.Remove(key);
                    return 1;
                }
                else
                { //if dependent job still depend on another job

                    //check for circular dependency
                    if (duplicate.Contains(key))
                    {
                        return -1;
                    }
                    duplicate.Add(key); // add this to duplicate to keep track of circular dependency
                    key = jobs[key]; 
                    return addJob(key); // recursivily call addJob function to add dependent job
                }
            }
        }

        public String JobString(String jobString)
        {
            String[] jobArray = jobString.Split(new Char[] { ';' });//Split String on ;
           
            //Add job to dictionary
            foreach (String s in jobArray)
            {
                char first = s[0];
                char last = s[s.Length - 1];

                if (first == last)
                { // if first job is same as second
                    //Return error
                    jobs.Clear();
                    jobSequence.Clear();
                    duplicate.Clear();
                    return "Error! Job can't depand on itself";
                }
                //if is true if job don't have any dependent job
                if (last < 'a' || last > 'z')
                {
                    last = ' ';
                }

                jobs.Add(first, last);
            }
            //get list of all job
            temp = new ArrayList(jobs.Keys);
            //iterate till all job has not been completed
            while (temp.Count != 0)
            {
                duplicate.Clear();
                int result  = addJob((char)temp[0]);
                if(result == -1)
                {
                    jobs.Clear();
                    jobSequence.Clear();
                    duplicate.Clear();
                    return "Error! Circular dependency";
                }
            }
            //convert array list to string
            var builder = new StringBuilder();
            for (int i = 0; i < jobSequence.Count; ++i) {
                builder.Append(jobSequence[i]);
            }

            jobs.Clear();
            jobSequence.Clear();
            duplicate.Clear();
            return builder.ToString();


        }
        static void Main(string[] args)
        {
            Program program = new Program();
            
            Console.WriteLine("Enter the tasks input:");
            //Console.WriteLine(program.JobString("a =>;b => c;c =>")); //print sequence acb
            //Console.WriteLine(program.JobString("a =>;b => c;c => f;d => a;e => ;f =>b")); //print Circular job dependency
            //Console.WriteLine(program.JobString("g => ")); //print sequence g
            //Console.WriteLine(program.JobString("a =>;b => c;c => f;d => a;e => b;f => ")); //print Job Sequence afcbde
            //Console.WriteLine(program.JobString("a =>;b => c;c => c")); //Print Error for self dependency
            string fullinput = Console.ReadLine();
            Console.WriteLine(program.JobString(fullinput));

            Thread.Sleep(5000);
        }
    }
}
