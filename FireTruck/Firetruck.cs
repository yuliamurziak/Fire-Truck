using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FireTruck
{
    class Firetruck : INotifyPropertyChanged
    {
        private const int SIZE_OF_ARR = 22;
        private int _cases = 0;
        private int _pathCount = 0;
        private string _routes;
        private static int _firstCorner;
        private static int _secondCorner;
        private static int _fireDestination; // the street closest to the fire
        private static int[,] _arrayOfAdjacentCorners = new int[SIZE_OF_ARR, SIZE_OF_ARR]; // array of adjacent street corners
        private static int[] _arrOfVisitedNodes = new int[SIZE_OF_ARR]; //firestations which has been visited ( not to repeat)
        private static int[] _arrOfPaths = new int[SIZE_OF_ARR];// firestations

        public string Routes
        {
            get { return _routes; }
            set
            {
                _routes = value;
                NotifyChanged();
            }
        }

        public void ReadFile()
        {
            string line;

            StreamReader file = new StreamReader(@"input.txt");
            while ((line = file.ReadLine()) != null)
            {
                string[] t = line.Split(' '); // an array for substrings of the whole read text
                if (t.Length == 1) // if there is one line
                {
                    _fireDestination = int.Parse(line);
                    ResetRoutes();
                }
                else // if there are our two corners which define the route
                {
                    _firstCorner = int.Parse(t[0]);
                    _secondCorner = int.Parse(t[1]);
                    if (_firstCorner != 0 && _secondCorner != 0)
                    {
                        _arrayOfAdjacentCorners[_firstCorner, _secondCorner] = _arrayOfAdjacentCorners[_secondCorner, _firstCorner] = 1;  // we show
                                                                                                                                          //that we can move in both directions withn one route
                    }
                    else // if there is 1 zero of  corners   or if there are 2 zeros (means finish)
                    {
                        Routes += "CASE " + (++_cases) + Environment.NewLine;
                        
                        _pathCount = 0;
                        _arrOfVisitedNodes[1] = 1; // initializing that our first node is Firestation1 tgtgx
                        CalculateRoutes(1, 0); // 1 & 0 ??
                        Routes += "There are " + _pathCount + " routes from the firestation 1 to streetcorner " + _fireDestination + Environment.NewLine+Environment.NewLine;
                       
                    }
                }
            }
            file.Close();

        }

        public void Reset()
        {
            Routes = " ";
        }
        private void ResetRoutes()
        {
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 22; j++)
                {
                    _arrayOfAdjacentCorners[i, j] = 0; // we reset the routes array for next cases
                }
            }
        }

        private void CalculateRoutes(int node, int dep)
        {
            int i;
            if (node == _fireDestination) // if the node is equal to 1(from start)
            {
                _pathCount++;
                Routes += "1";

                for (i = 0; i < dep; i++)
                {
                    Routes += " " + (_arrOfPaths[i]).ToString();
                    
                }
                Routes += Environment.NewLine;
                return;
            }
            for (i = 1; i <= 21; i++)
            {
                if (_arrayOfAdjacentCorners[node, i] != 0 && _arrOfVisitedNodes[i] == 0)//?
                {
                    if (Check(i) != 0)
                    {
                        continue;
                    }
                    _arrOfVisitedNodes[i] = 1;
                    _arrOfPaths[dep] = i;
                    CalculateRoutes(i, dep + 1);
                    _arrOfVisitedNodes[i] = 0; // to reset paths for tje next routes
                }
            }
        }

        private int Check(int param)
        {
            int[] temp_arr = new int[SIZE_OF_ARR];
            int var = 0;
            int i, j;
            int[] used = new int[SIZE_OF_ARR];
            int temporal_node;
            temp_arr[var] = param;

            for (i = 0; i <= var; i++)
            {
                temporal_node = temp_arr[i];
                if (temporal_node == _fireDestination)
                {
                    return 0;
                }
                for (j = 1; j <= 21; j++)
                {
                    if (_arrOfVisitedNodes[j] == 0 && _arrayOfAdjacentCorners[temporal_node, j] != 0 && used[j] == 0)
                    {
                        used[j] = 1;
                        temp_arr[++var] = j;
                    }
                }
            }
            return 1;
        }

        #region Notify
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion Notify
    }
}
