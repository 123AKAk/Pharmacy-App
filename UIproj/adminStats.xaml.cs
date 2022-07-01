using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
using UIproj.myclass;

namespace UIproj
{
    /// <summary>
    /// Interaction logic for adminStats.xaml
    /// </summary>
    public partial class adminStats : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection LastHourSeries { get; set; }
        public SeriesCollection LastHourSeries1 { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        dtba connec = new dtba();
        MySqlCommand insertCommand, insertCommand2;
        MySqlDataAdapter daa;
        DataSet dss;
        MySqlCommandBuilder scmbl;

        public adminStats()
        {
            InitializeComponent();
        }

        //runing analysis sum of data based on data which data was stored
        DateTime d1, d2;
        string date1, date2; double todays;
        ArrayList datelistprofit = new ArrayList();
        ArrayList datelistloss = new ArrayList();
        private void Analyse(object sender, RoutedEventArgs e)
        {
            if (fromDate.Text == "")
            {
                MessageBox.Show("Start Date Cannot Be Empty");
            }
            else if (toDate.Text == "")
            {
                MessageBox.Show("End Date Cannot Be Empty");
            }
            else
            {
                //DateTime xmas = new DateTime(2009, 12, 25);
                //double daysUntilChristmas = xmas.Subtract(DateTime.Today).TotalDays;

                d1 = DateTime.ParseExact(fromDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                d2 = DateTime.ParseExact(toDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                todays = (d1 - d2).TotalDays;
                if (d1 > d2)
                {
                    MessageBox.Show("Start Date cannot be greater than End Date", "Warning:", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                // todays >= 0 && todays < -7
                else if (-6 < todays && todays <= 0)
                {
                    MessageBox.Show("Must be equivalent to 7 Days ", "Warning:", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else
                {
                    date1 = d1.ToString("yyyy-MM-dd");
                    date2 = d2.ToString("yyyy-MM-dd");

                    for (var adate = d1; adate <= d2; adate = adate.AddDays(1))
                    {
                        datelistprofit.Add(adate.ToString("yyyy-MM-dd"));
                        datelistloss.Add(adate.ToString("yyyy-MM-dd"));
                    }

                    CalculateChartprofit();
                    CalculateChartloss();
                    GetTotalData();
                    GetTotalLossData();
                }
            }
        }

        //reading profit money in each selected date
        Dictionary<string, int> names = new Dictionary<string, int>();
        Dictionary<string, string> adays = new Dictionary<string, string>();
        string dateselect;
        private void CalculateChartprofit()
        {
            for (int j = 0; j < datelistprofit.Count; j++)
            {
                dateselect = datelistprofit[j].ToString();

                names.Add(String.Format("name{0}", j.ToString()), j);
                adays.Add(String.Format("days{0}", j.ToString()), j.ToString());

                MySqlCommand ccommand;
                var cQuery = "SELECT SUM(Price) chartsumprofit FROM psales WHERE MDate='" + dateselect + "' AND Credit_State = 0";
                ccommand = new MySqlCommand(cQuery, connec.connectdb);
                try
                {
                    connec.connectdb.Open();
                    MySqlDataReader readerc = ccommand.ExecuteReader();
                    while (readerc.Read())
                    {
                        if(readerc["chartsumprofit"].ToString().Equals(""))
                        {
                            names["name" + j] = 0;
                            DateTime dateTimefinal = Convert.ToDateTime(datelistprofit[j].ToString());
                            adays["days" + j] = dateTimefinal.ToString("dddd, dd MMMM yyyy");
                            //MessageBox.Show("Stage a one"+ names["name" + j]);
                        }
                        else
                        {
                            names["name" + j] = Int32.Parse(readerc["chartsumprofit"].ToString());
                            DateTime dateTimefinal = Convert.ToDateTime(datelistprofit[j].ToString());
                            adays["days" + j] = dateTimefinal.ToString("dddd, dd MMMM yyyy");
                            //MessageBox.Show("Stage b one" + names["name" + j]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (PROFIT-Date)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                }
            }
            datelistprofit.Clear();
        }

        //reading lost money in each selected date
        Dictionary<string, int> anames = new Dictionary<string, int>();
        private void CalculateChartloss()
        {
            for (int k = 0; k < datelistloss.Count; k++)
            {
                dateselect = datelistloss[k].ToString();

                MySqlCommand ccommand;
                var cQuery = "SELECT SUM(Price) chartsumloss FROM psales WHERE MDate='" + dateselect + "' AND Credit_State = 1";
                ccommand = new MySqlCommand(cQuery, connec.connectdb);
                try
                {
                    connec.connectdb.Open();
                    MySqlDataReader readerc = ccommand.ExecuteReader();
                    while (readerc.Read())
                    {
                        if (readerc["chartsumloss"].ToString().Equals(""))
                        {
                            anames["name" + k] = 0;
                            //MessageBox.Show("Stage a Two" + anames["name" + k]);
                        }
                        else
                        {
                            anames["name" + k] = Int32.Parse(readerc["chartsumloss"].ToString());
                            //MessageBox.Show("Stage b Two" + anames["name" + k]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " (Loss-Date)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    connec.connectdb.Close();
                }
            }
            datelistloss.Clear();
            ShowChart();
        }

        //shows the data in the chart after calculation
        private void ShowChart()
        {
            try
            {
                //MessageBox.Show("" + names["name0"] + " | " + names["name1"] + " | " + names["name2"] + " | " + names["name3"] + " | " + names["name4"] + " | " + names["name5"] + " | " + names["name6"]);
                //MessageBox.Show("" + anames["name0"] + " | " + anames["name1"] + " | " + anames["name2"] + " | " + anames["name3"] + " | " + anames["name4"] + " | " + anames["name5"] + " | " + anames["name6"]);

                SeriesCollection = new SeriesCollection
                {
                    new StackedColumnSeries
                    {
                        Values = new ChartValues<double> {names["name0"], names["name1"], names["name2"], names["name3"], names["name4"],names["name5"], names["name6"]},
                        StackMode = StackMode.Values,
                        DataLabels = true
                    },
                     new StackedColumnSeries
                    {
                        Values = new ChartValues<double> {anames["name0"], anames["name1"], anames["name2"], anames["name3"], anames["name4"], anames["name5"], anames["name6"]},
                        StackMode = StackMode.Values,
                        DataLabels = true
                    }
                };
                LastHourSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        AreaLimit = 10,
                        Values = new ChartValues<ObservableValue>
                        {
                            new ObservableValue(3),new ObservableValue(1),new ObservableValue(9),new ObservableValue(4),new ObservableValue(5),
                            new ObservableValue(3),new ObservableValue(1),new ObservableValue(2),new ObservableValue(3),new ObservableValue(7),
                        }
                    }
                };
                LastHourSeries1 = new SeriesCollection
                {
                    new LineSeries
                    {
                        AreaLimit = -10,
                        Values = new ChartValues<ObservableValue>
                        {
                            new ObservableValue(13),new ObservableValue(11),new ObservableValue(9),new ObservableValue(14),new ObservableValue(5),
                            new ObservableValue(3),new ObservableValue(12),new ObservableValue(2),new ObservableValue(3),new ObservableValue(7),
                        }
                    }
                };
                //Labels = new[] { "Day 1", "Day 2", "Day 3", "Day 4", "Day 5","Day 6", "Day 7",};
                Labels = new[] { adays["days0"], adays["days1"], adays["days2"], adays["days3"], adays["days4"], adays["days5"], adays["days6"] };
                Formatter = value => value.ToString();
                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " (Chart)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                names.Clear();
                anames.Clear();
                adays.Clear();
            }
        }

        int madd, nomadd;
        private void GetTotalData()
        {
            DataDisplay.Items.Clear();
            MySqlCommand ccommand;
            var cQuery = "SELECT SUM(Price) realsum FROM psales WHERE Credit_State = '0' AND MDate >= '" + date1 + "' AND MDate <= '" + date2 + "'";
            ccommand = new MySqlCommand(cQuery, connec.connectdb);
            try
            {
                connec.connectdb.Open();
                MySqlDataReader readerc = ccommand.ExecuteReader();
                while (readerc.Read())
                {
                    madd = Int32.Parse(readerc["realsum"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connec.connectdb.Close();
            }
        }

        private void GetTotalLossData()
        {
            MySqlCommand ccommand;
            var cQuery = "SELECT SUM(Price) arealsum FROM psales WHERE Credit_State = '1' AND MDate >= '" + date1 + "' AND MDate <= '" + date2 + "'";
            ccommand = new MySqlCommand(cQuery, connec.connectdb);
            try
            {
                connec.connectdb.Open();
                MySqlDataReader readerc = ccommand.ExecuteReader();
                while (readerc.Read())
                {
                    nomadd = Int32.Parse(readerc["arealsum"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connec.connectdb.Close();

                //adds data to list view
                var row = new { totalProfit = madd.ToString(), totaloss = nomadd.ToString() };
                DataDisplay.Items.Add(row);
            }
        }

    }
}
