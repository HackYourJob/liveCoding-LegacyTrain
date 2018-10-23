﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TrainTrain
{
    public class WebTicketManager
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;

        public WebTicketManager():this(new TrainDataService(UriTrainDataService), new BookingReferenceService(UriBookingReferenceService))
        {
        }

        public WebTicketManager(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }


        public async Task<string> Reserve(string trainId, int seatsRequestedCount)
        {
            var train = await _trainDataService.GetTrain(trainId);

            if (!train.CanReserve(seatsRequestedCount))
            {
                return InvalidReservation(trainId);
            }

            var bookingReference = await _bookingReferenceService.GetBookingReference();

            var result = train.TryToBook(seatsRequestedCount, bookingReference, trainId);
            
            if (result is SeatsBookedFailedBecauseNotEnoughAvailableSeats)
            {
                return InvalidReservation(trainId);
            }

            var reservation = result as SeatsBooked;
            await _trainDataService.Reserve(trainId, bookingReference, reservation.SeatIds.ToList());

            return
                ValidReservation(trainId, bookingReference, reservation.SeatIds.ToList());

        }
      

        private string ValidReservation(string trainId, string bookingReference, List<Seat> availableSeats)
        {
            return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"{bookingReference}\", \"seats\": {dumpSeats(availableSeats)}}}";
        }

        private static string InvalidReservation(string trainId)
        {
            return $"{{\"train_id\": \"{trainId}\", \"booking_reference\": \"\", \"seats\": []}}";
        }

        private string dumpSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");

            var firstTime = true;
            foreach (var seat in seats)
            {
                if (!firstTime)
                {
                    sb.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                sb.Append(string.Format("\"{0}{1}\"", seat.SeatNumber, seat.CoachName));
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}