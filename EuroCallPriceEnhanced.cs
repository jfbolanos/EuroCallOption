using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///Must add references for the below before running the code. These were used in order to create the plot of various underlying asset paths.
using System.Windows.Forms.DataVisualization;
using System.Drawing;
using System.Windows.Forms;


namespace EuroCallOption
{
    /// The following is the class architecture for the Discretization Class. The class will contain functions for potentially many different discretizations.
    /// For this problem, the Euler discretization function was created. 
    class Discretization
    {
        

        /// Default constructor to create object
        public Discretization()
        {

        }


        /// Euler discretization. Given a starting price X(t), mu, sigma, delta and Z (the random gaussian variable), the next underlying price (X(t+1)) will be output.
        public double Euler(double X0, double mu, double sigma, double delta, double Z)
        {
            ///Euler Discretization Scheme:
            ///X(t+delta) = X(t) + mu*X(t)*delta + sigma*X(t)*sqrt(delta)*Z; where Z ~ N (0,1)
            
            return X0 + mu * X0 * delta + sigma * X0 * Math.Sqrt(delta) * Z;
        }


    }
    class EuroCallPrice
    {
        /// Main Code.
        static void Main(string[] args)
        {
            ///Defining variables
            double[] UnderlyingPrice;
            double[] CallPrice;
            Random r = new Random(); ///this is the Class used to generate random variables
            Discretization disc = new Discretization(); ///class that was coded above used to call the Euler function later in the code
            System.Windows.Forms.DataVisualization.Charting.Chart chart = new System.Windows.Forms.DataVisualization.Charting.Chart(); ///class to set up Plot of underlying price
            chart.ChartAreas.Add("ChartArea1"); ///set up the initial plot
            
            ///The following lines ask the user for the inputs needed to price the option and run the simulation
            ///Additionally, the inputs provided by the user are assigned the appropriate variables.
            Console.WriteLine("Please Enter The Initial Asset Price:");
            double X0 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Please Enter The Strike Price of the Option:");
            double K = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Please Enter The Time To Expiration:");
            double T = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Please Enter mu:");
            double mu = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Please Enter sigma:");
            double sigma = Convert.ToDouble(Console.ReadLine());

           /// Console.WriteLine("Please enter The Number of Simulations to Run:");
            ///int N = Convert.ToInt32(Console.ReadLine());
            int N = 5000; ///Number of simulations
            
            int m = 200; ///Number of increments between t = 0 and t = T
            double delta = T / m; ///size of the increment
                                  
            CallPrice = new double[N]; ///now allocating the size of array based on the number of simulations
            UnderlyingPrice = new double[m + 1]; //now allocating the size of the array based on the number of increments
            
            double U1; ///Represents uniform random variable between 0 and 1
            double U2; ///Also represents uniform random variable between 0 and 1
            double Z; ///Will represent the Normal Random Variable with mean 0 and variance 1
            
            double sum1 = 0; //variable used to keep track of the sum in order to find the average of all the Call Prices for each of the simulations
            double sum2 = 0; //variable used to keep track of the sum in order to ultimately find the Standard Error of the simulation

            double OptionPrice; //Final European Call Option Price calculated
            double StandardError; //Standard error of the simulation


            for (int i = 0; i < N; i++) ///Each run through this loop represents 1 simulation
            {
                ///Set up a new series that will be plotted on the graph
                chart.Series.Add(Convert.ToString(i)); 
                ///Set chart type to Line graph
                chart.Series[Convert.ToString(i)].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                ///Initialize the Underlying Price array based on the input given by the user
                UnderlyingPrice[0] = X0;

                ///Plot this initial t = 0 and Underlying price on the graph
                chart.Series[Convert.ToString(i)].Points.AddXY(0, UnderlyingPrice[0]);

                for (int j = 0; j < m; j++) ///Each run through this loop represents the next increment closer to t = T
                {

                    ///Because the Random() Class only allows the creation of uniform random variables between 0 and 1,
                    ///a transform must be done to create a normal random variable with mean 0 and variance 1.
                    ///The transform that was done is called the Box-Muller Transform
                    
                    U1 = r.NextDouble(); ///Get first uniform random variable
                    U2 = r.NextDouble(); ///Get second uniform random variable
                    Z = Math.Sqrt(-2 * Math.Log(U1)) * Math.Cos(2 * Math.PI * U2); ///Box-Muller Transform to generate Z ~ N(0,1)
                    

                    UnderlyingPrice[j + 1] = disc.Euler(UnderlyingPrice[j], mu, sigma, delta, Z); ///Generate the next X value based on the Euler Discretization

                    chart.Series[Convert.ToString(i)].Points.AddXY(delta*(j + 1), UnderlyingPrice[j + 1]); ///Plot the t value as well as X(t) value onto the graph for series i
                    

                }

                CallPrice[i] = Math.Max(0, UnderlyingPrice[m] - K); ///Value of the call option.

            }


            ///Find the sum of all the Call Prices that were simulated (used for the average)
            for (int a = 0; a < N; a++ )
            {
                sum1 = sum1 + CallPrice[a];

            }

            //Calculate the average which represents the option price
            OptionPrice = sum1 / N;

            ///Same procedure, but to calculate the Standard Error
            for (int b = 0; b < N; b++)
            {
                sum2 = sum2 + Math.Pow((CallPrice[b] - OptionPrice), 2);
            }

            ///SE = s / sqrt(N), where s is the sample standard deviation
            StandardError = Math.Sqrt(sum2 / (N - 1)) / Math.Sqrt(N);

            ///Print out the required output: Price of the European Call Option and the Standard Error of the simulation
            Console.WriteLine("The Price of the Option is: " + OptionPrice);
            Console.WriteLine("The Standard Error of the Simulation is: " + StandardError);

            ///Save the graph of the Asset Prices for each simulation in a png file that is saved in the same folder as the location of the .exe
            chart.SaveImage("StockPath.png", System.Drawing.Imaging.ImageFormat.Png);

            ///The console won't close until the user hits any key another time
            Console.ReadKey();
        }
    }
}
