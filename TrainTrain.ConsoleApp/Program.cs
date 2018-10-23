﻿using System;

namespace TrainTrain.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var train = args[0];
            var seats = int.Parse(args[1]);

            var manager = new WebTicketReservation();

            var jsonResult = manager.Execute(train, seats);

            Console.WriteLine(jsonResult.Result);

            Console.WriteLine("Type <enter> to exit.");
            Console.ReadLine();
        }
    }
}