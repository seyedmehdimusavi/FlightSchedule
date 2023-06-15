
using FlightSchedule.model;
using FlightSchedule.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static FlightSchedule.model.FlightScheduleContext;

//public interface IFlightService
//{
//    public int calculateDepartureValues();
//}
//==========================================================================================================

public class FlightService
{
    private readonly FlightScheduleContext db = new FlightScheduleContext();

    public string getStatus(int flight_id, DateTime departure_time, int airline_id)
    {
        long? departureValue = db.flights.FirstOrDefault(t => t.flight_id == flight_id).departureValue;
        var ifNew = from f in db.flights
                    where f.airline_id == airline_id && (f.departureValue > (departureValue - 10110)) && (f.departureValue <= departureValue + 30)
                    select f;

        var ifDiscontinued = from f in db.flights
                             where f.airline_id == airline_id && (f.departureValue > (departureValue + 10110)) && (f.departureValue >= departureValue - 30)
                             select f;

        if (ifNew.Count() == 0) return "New flights";
        else if (ifDiscontinued.Count() == 0) return "Discontinued flights";
        else return "";
    }


    int departureTime2Int(DateTime d)
    {
        int day = d.Day + d.Month * 31 + (d.Year - 2000) * 372;
        int hour = d.Hour * 60 + d.Minute;
        return (day * 1440) + hour;
    }


    public int calculateDepartureValuesForFirstRun()
    {
        var flights = from f in db.flights select f;
        foreach (flight flight in flights)
        {
            flight.departureValue = departureTime2Int(flight.departure_time);
            db.Entry(flight).State = EntityState.Modified;
        }
        return db.SaveChanges();
    }


    public int string2DataTime(string date)
    {
        return departureTime2Int(new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(5, 2)), int.Parse(date.Substring(8, 2))));
    }

    public void findFlight(string start_date_str, string end_date_str, string agency_id_str)
    {
        int start_date = string2DataTime(start_date_str);
        int end_date = string2DataTime(end_date_str);
        int agency_id = int.Parse(agency_id_str);
        ResultModel[] res = (from f in db.flights
                             join r in db.routes on f.route_id equals r.route_id
                             join e in db.subscriptions on new { r.destination_city_id, r.origin_city_id } equals new { e.destination_city_id, e.origin_city_id }
                             where e.agency_id == agency_id && f.departureValue >= start_date && f.departureValue <= end_date
                             select new ResultModel
                             {
                                 flight_id = f.flight_id,
                                 origin_city_id = e.origin_city_id,
                                 destination_city_id = e.destination_city_id,
                                 departure_time = f.departure_time,
                                 arrival_time = f.arrival_time,
                                 airline_id = f.airline_id,
                                 //status = "" //getStatus(f.flight_id, f.departure_time, f.airline_id)
                             }).ToArray();

        Console.WriteLine("Number of Available Flights:{0}", res.Length);

        StringBuilder result = new StringBuilder();
        result.Append("flight_id,origin_city_id,destination_city_id,departure_time,arrival_time,airline_id,status\n");
        foreach (var t in res)
        {
            result.Append($"{t.flight_id},{t.origin_city_id},{t.destination_city_id},{t.departure_time},{t.arrival_time},{t.airline_id}, { getStatus(t.flight_id, t.departure_time, t.airline_id)}\n");
        }
        File.WriteAllText("results.csv", result.ToString());
    }
}



