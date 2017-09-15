using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolving_Shakespeare
{
    public class DNA
    {
        char[] genes;
        public double FitnesScore { get; set; }
        string target;
        public string DNAcode { get; set; }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public string Target { get { return target; } set { target = value; } }

        // Create DNA Randomly
        public DNA(string pTarget)
        {
            target = pTarget;
            genes = new char[pTarget.Length];
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            for (int i=0; i<genes.Length; i++)
            {                                
                lock (syncLock)                
                    genes[i] = chars[random.Next(chars.Length)];                               
            }
            FitnesScore = Fitness();
            DNAcode = getPhrase();
        }

        //Calculate fitness
        public double Fitness()
        {
            int score = 0;
            for (int i =0; i <genes.Length; i++)
            {
                if (genes[i] == target[i])                
                    score++;                
            }
            // Its important to convert INT to Double, otherwise you will get Fitness score 0 all the time           
            FitnesScore = Math.Round( Convert.ToDouble(score) / Convert.ToDouble(target.Length), 2);
            return FitnesScore;
        }

        //Crossover
        public void Crossover(DNA pPartner)
        {
            // crossover point
            lock (syncLock)
            {
                int crossover_point = random.Next(target.Length);

                // replace genes with genes of the parent
                for (int i = crossover_point; i < target.Length; i++)                
                    genes[i] = pPartner.genes[i];                
            }
            DNAcode = getPhrase();           
        }

        public void Mutation(double pMutationRate)
        {
            lock (syncLock)
            {
                for (int i = 0; i < target.Length; i++)
                {
                    double dr = random.NextDouble();
                    if (dr < pMutationRate)
                    {
                        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
                        genes[i] = chars[random.Next(chars.Length)];
                        
                    }
                }
            }
            DNAcode = getPhrase();
        }

        //convert Char to String - Phenotype
        public string getPhrase()
        {
            return new string(genes); 
        }

    }
}
