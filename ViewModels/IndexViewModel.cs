using Learn_web.Models;
using Learn_web.Models.SortPageFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public PageViewModel pageViewModel { get; set; }

        public SortViewModel SortViewModel { get; set; }

        public double? sum { get; set; }

        public WeatherResponse getWeather = Weather.GetWeather();

    }
}
