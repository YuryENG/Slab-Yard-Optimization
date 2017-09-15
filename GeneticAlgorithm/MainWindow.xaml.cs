using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Evolving_Shakespeare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // set collection view source for XAML data binding
        CollectionViewSource populationListSource;

        public string target = "to be or not to be";
        public double mutationRate = 0.01;
        public int totalPopulation = 1500;
        public List<DNA> population = new List<DNA>();
        public List<DNA> maitingPool = new List<DNA>();

        //global random variable 
        // it is required because C# does not generate random numbers well in a short period of time
        // therefore i will use one global variable to generate NEXT randoms
        
        private static readonly Random random = new Random();
        //Create object to "lock"
        //A lock protects access to the variables for the total count and sum of all random numbers 
        //generated on all thread; later in the code when i generate random numbers you will see 
        //lock (syncLock)
        // {
        // random.Next(....)
        //}
        
        private static readonly object syncLock = new object();

        public int totalgenerations;

        public MainWindow()
        {
            InitializeComponent();

            totalgenerations = 0;
            textBoxTotalPopulation.Text = totalPopulation.ToString();
            textBoxMutationRate.Text = Convert.ToString(mutationRate * 100) + "%";
            populationListSource = ((CollectionViewSource)(this.FindResource("populationListSource")));
                        
            //initialize population 
            for (int i = 0; i < totalPopulation; i++)
            {
                population.Add(new DNA(target));
            }
            //Display list in the grid view            
            populationListSource.Source = population;

            
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
           
            bool flag = false;

            do
            {
                totalgenerations++;

                // build mating pool            
                maitingPool.Clear();
                for (int i = 0; i < population.Count(); i++)
                {
                    int n = Convert.ToInt16((population[i].Fitness() * 100)) + 1;
                    for (int j = 0; j < n; j++)
                    {
                        maitingPool.Add(population[i]);
                    }
                }

                // reproduction
                
                for (int i = 0; i < population.Count(); i++)
                {
                    int a;
                    int b;
                    lock (syncLock)
                    {
                        // pick random a from population
                        a = random.Next(population.Count());                        
                        // pick raondm b from mating pull
                        b = random.Next(maitingPool.Count());                        
                    }
                    // peform crossover
                    population[a].Crossover(maitingPool[b]);
                    // perform muation
                    population[a].Mutation(mutationRate);
                    // update fitnes score after crossover and mutation 
                    // if it isnt done then it wont be diplayed properly
                    population[a].Fitness();
                }
                // check if the target phrase is in the population
                for (int i = 0; i < population.Count(); i++)
                {
                    if (population[i].DNAcode == target)
                    {
                        //System.Windows.MessageBox.Show("Target Phrase is in the population");
                        flag = true;
                    }
                }


               
            }
            // proceed with iteration untill target phrase or 10000 itertion, to avoid endless loop
            while (flag == false && totalgenerations <10000);

            //Update user interface
            IterationCount.Text = totalgenerations.ToString();
            populationListSource.View.Refresh();
            var best_score = population.Max(x => x.FitnesScore);
            var bestphrase = population.First(o => o.FitnesScore == best_score);
            TextBlockBestPhrase.Text = bestphrase.DNAcode;
        }
    }
}
