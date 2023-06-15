// See https://aka.ms/new-console-template for more information


FlightService f = new FlightService();
//f.calculateDepartureValuesForFirstRun();

//if(args.Length >0)
//  f.findFlight(args[0], args[1], args[2]);

f.findFlight("2016-01-11","2016-10-15","5");
//f.findFlight("2018-01-11", "2018-01-15","1");

