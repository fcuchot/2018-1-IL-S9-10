using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algo.Optim
{
    public class Guest
    {
        public Guest( Meeting m,
                      string name,
                      Airport guestLocation )
        {
            Name = name;
            Location = guestLocation;
            var db = m.FlightDatabase;
            ArrivalFlights = db.GetFlights( m.MaxBusTimeOnArrival, guestLocation, m.Location )
                                .Concat( db.GetFlights( m.MaxBusTimeOnArrival.AddDays(-1), guestLocation, m.Location ) )
                                .Where( f => f.ArrivalTime < m.MaxBusTimeOnArrival )
                                .OrderByDescending( f => f.ArrivalTime )
                                .ToArray();
            DepartureFlights = db.GetFlights( m.MinBusTimeOnDeparture, m.Location, guestLocation )
                                 .Concat( db.GetFlights( m.MinBusTimeOnDeparture.AddDays( 1 ), m.Location, guestLocation ) )
                                 .Where( f => f.DepartureTime > m.MinBusTimeOnDeparture )
                                 .OrderBy( f => f.DepartureTime )
                                 .ToArray();
        }

        public string Name { get; }

        public Airport Location { get; }

        /// <summary>
        /// No stop is flights required.
        /// (Unused, this is just a sample of constraint.)
        /// </summary>
        public bool NoStop { get; set; }

        public IReadOnlyList<SimpleFlight> ArrivalFlights { get; }

        public IReadOnlyList<SimpleFlight> DepartureFlights { get; }
    }

    public class Meeting
    {
        public Meeting( FlightDatabase db )
        {
            FlightDatabase = db;
            Location = Airport.FindByCode( "LHR" );
            MaxBusTimeOnArrival = new DateTime( 2010, 7, 27, 17, 0, 0 );
            MinBusTimeOnDeparture = new DateTime( 2010, 8, 3, 15, 0, 0 );
            Guests = new[]
            {
                new Guest( this, "Kaï", Airport.FindByCode( "BER" ) ),
                new Guest( this, "Erwan", Airport.FindByCode( "CDG" ) ),
                new Guest( this, "Robert", Airport.FindByCode( "MRS" ) ),
                new Guest( this, "Paul", Airport.FindByCode( "LYS" ) ),
                new Guest( this, "James", Airport.FindByCode( "MAN" ) ),
                new Guest( this, "Pedro", Airport.FindByCode( "BIO" ) ),
                new Guest( this, "John", Airport.FindByCode( "JFK" ) ),
                new Guest( this, "Abdel", Airport.FindByCode( "TUN" ) ),
                new Guest( this, "Isabella", Airport.FindByCode( "MXP" ) )
            };
        }

        public FlightDatabase FlightDatabase { get; }

        public Airport Location { get; }

        public DateTime MaxBusTimeOnArrival { get; }

        public DateTime MinBusTimeOnDeparture { get; }

        public IReadOnlyList<Guest> Guests { get; }
    }
}
