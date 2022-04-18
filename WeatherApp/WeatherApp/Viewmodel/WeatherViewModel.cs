using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WeatherApp.Viewmodel
{
    public class WeatherViewModel: INotifyPropertyChanged
    {
        private WeatherModel _selectedPlace;
        public string _filter;
        public ObservableCollection<WeatherModel> PlaceList { get; set; }

        public static List<WeatherModel> _allPlaceList = new List<WeatherModel>();

        public event PropertyChangedEventHandler PropertyChanged;
        public string SelectedPlaceTemp { get; set; }
        public string SelectedPlaceName { get; set; }
        public string SelectedPlaceImg = "Images/blank.png";

        public DetailsPage DetailsPage { get; set; }
        public Image weatherIcon;

        public WeatherViewModel(Image weatherIcon)
        {
            PlaceList = new ObservableCollection<WeatherModel>();
            getData();
            this.weatherIcon = weatherIcon;

        }
        public void getData()
        {   // have to use this id and key or it wont grab the data from the site
            //?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90
            //http://api.weatherunlocked.com/api/current/{POSTALCODE/LON,LAT}?app_id={APP_ID}&app_key={APP_KEY}  
            //7ca54e7f ID
            //c3a446ab3f5d1f0c51233b4ef07ffa90 key

            PlaceList.Clear();
            _allPlaceList.Clear();
            makeModel("http://api.weatherunlocked.com/api/current/uk.G3 8ND?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "Glasgow");
            makeModel("http://api.weatherunlocked.com/api/current/ca.B2W%202W5?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "Nova Scotia");
            makeModel("http://api.weatherunlocked.com/api/current/us.10001?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "New York");
            makeModel("http://api.weatherunlocked.com/api/current/51.50,-0.12?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "London");
            makeModel("http://api.weatherunlocked.com/api/current/50.84,4.35?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "Brussels");
            makeModel("http://api.weatherunlocked.com/api/current/35.67,139.65?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "Tokyo");
            makeModel("http://api.weatherunlocked.com/api/current/6.20,106.84?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "Jakarta");
            makeModel("http://api.weatherunlocked.com/api/current/43.65,49.38?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "Toronto");
            makeModel("http://api.weatherunlocked.com/api/current/39.90,116.40?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "Beijing");
            makeModel("http://api.weatherunlocked.com/api/current/22.90,43.17?app_id=7ca54e7f&app_key=c3a446ab3f5d1f0c51233b4ef07ffa90", "Rio de Janeiro");
        }
        public string imgGrabber(float temp)//if its warm use a sunny image, if its cold use a snowflake, and if neither use a cloud
        {
            if (temp <= 0) { return "Images/cold.png"; }
            else if (temp >= 18 ) { return "Images/sunny.png"; }
            else { return "Images/cloudy.png"; }
        }
        public async void makeModel(string url,string placeName)//makes a model and adds it to the lists
        {
            using (HttpClient client = new HttpClient())//http://api.weatherunlocked.com
            {
                var response = await client.GetStringAsync(url);//grabs data from url given
                var data = JsonConvert.DeserializeObject<RootObject>(response);//converts it into a readable object
                
                string img = imgGrabber(data.temp_c);//quickly grab what image we want to use
                WeatherModel current = new WeatherModel(data.lat, data.lon, placeName, data.temp_c, data.temp_f, data.humid_pct, data.wx_desc, img);//set up the model
                PlaceList.Add(current);//add it to the lists
                _allPlaceList.Add(current);
                
            }
        }
        public WeatherModel SelectedPlace//changes the view to hold information about the current place
        {
            get { return _selectedPlace; }
            set
            {
                try
                {
                    _selectedPlace = value;
                    DetailsPage = new DetailsPage();


                    if (value == null)//if nothing selected do nothing
                    {

                    }
                    else
                    {
                        SelectedPlaceTemp = String.Format("{0}° C", value.tempC);
                        SelectedPlaceName = value.Location;
                        SelectedPlaceImg = value.Icon;

                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedPlaceTemp"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedPlaceName"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedPlaceImg"));

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }

 
        public string Filter//used for searching also from namedays app
        {
            get { return _filter; }
            set
            {

                if (value == _filter) { return; }
                _filter = value;


                PerformFiltering();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
            }
        }
        public void PerformFiltering()//function from namedays example that provides the search function
        {
            try
            {

                if (_filter == null)
                {
                    _filter = "";
                }
                //If _filter has a value (ie. user entered something in Filter textbox)
                //Lower-case and trim string
                var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

                //Use LINQ query to get all placemodel names that match filter text, as a list
                var result =
                    _allPlaceList.Where(d => d.Location.ToLowerInvariant()
                    .Contains(lowerCaseFilter))
                    .ToList();

                //Get list of values in current filtered list that we want to remove
                //(ie. don't meet new filter criteria)
                var toRemove = PlaceList.Except(result).ToList();

                //Loop to remove items that fail filter
                foreach (var x in toRemove)
                {
                    PlaceList.Remove(x);
                }

                var resultCount = result.Count;
                // Add back in correct order.
                for (int i = 0; i < resultCount; i++)
                {
                    var resultItem = result[i];
                    if (i + 1 > PlaceList.Count || !PlaceList[i].Equals(resultItem))
                    {
                        PlaceList.Insert(i, resultItem);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
