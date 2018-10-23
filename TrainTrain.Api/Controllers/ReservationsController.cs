﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrainTrain.Api.Models;
using TrainTrain.Infrastructure.AdapterIn;

namespace TrainTrain.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReservationsController : Controller
    {
        private readonly ReservationAdapter _reservationAdapter;

        public ReservationsController(ReservationAdapter reservationAdapter)
        {
            _reservationAdapter = reservationAdapter;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<string> Post([FromBody]ReservationRequestDto reservationRequest)
        {
            return await _reservationAdapter.Reserve(reservationRequest.train_id, reservationRequest.number_of_seats);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
