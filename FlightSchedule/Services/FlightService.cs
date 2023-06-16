
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


    //public int updateAllStatus()
    //{
    //    var flights = db.flights.OrderBy(t => t.departureValue).OrderBy(i => i.airline_id).ToList();

    //    int last_airline_id = -1;
    //    int last_departurevalue = -1;

    //    for (int i = 1; i < flights.Count - 1; i++)
    //    {
    //        if (flights[i].airline_id == last_airline_id)
    //        {
    //            long before = flights[i].departureValue - flights[i - 1].departureValue;
    //            long after = flights[i + 1].departureValue - flights[i].departureValue;
    //            if (before >= 10110) flights[i].status = "New flights";
    //            else if (after >= 10110) flights[i].status = "Discontinued flights";
    //            else flights[i].status = "";
    //            db.Entry(flights[i]).State = EntityState.Modified;
    //        }
    //        else last_airline_id = flights[i].airline_id;
    //    }
    //    return db.SaveChanges();
    //}



    public bool updateStatus(ref flight[] flights)
    {
        int last_airline_id = -1;
        for (int i = 1; i < flights.Length - 1; i++)
        {
            if (flights[i].airline_id == last_airline_id)
            {
                long before = flights[i].departureValue - flights[i - 1].departureValue;
                long after = flights[i + 1].departureValue - flights[i].departureValue;
                if (before >= 10110) flights[i].status = "New flights";
                else if (after >= 10110) flights[i].status = "Discontinued flights";
                else flights[i].status = "";
            }
            else last_airline_id = flights[i].airline_id;
        }
        return true;
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
                             }).ToArray();

        Console.WriteLine("Number of Available Flights:{0}", res.Length);
        //Console.WriteLine("processing time about {0} minutes", (res.Length / 1000) * 0.3);

        flight[] searchArr = db.flights.Where(t => t.departureValue >= start_date - 10110 && t.departureValue <= end_date + 10110).OrderBy(t => t.departureValue).OrderBy(i => i.airline_id).ToArray();
        if (updateStatus(ref searchArr))
        {
            int k = 0;
            StringBuilder result = new StringBuilder();
            result.Append("flight_id,origin_city_id,destination_city_id,departure_time,arrival_time,airline_id,status\n");
            foreach (var t in res)
            {
                k++;
                if ((k % 100) == 0) { Console.Clear();  Console.Write((int)((k * 100 / res.Length)) + "% "); }
                result.Append($"{t.flight_id},{t.origin_city_id},{t.destination_city_id},{t.departure_time},{t.arrival_time},{t.airline_id},{ searchArr.FirstOrDefault(s => s.flight_id == t.flight_id).status}\n");
            }
            File.WriteAllText("results.csv", result.ToString());
        }
    }
}



