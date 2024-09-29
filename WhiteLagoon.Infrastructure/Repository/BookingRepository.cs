﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Booking entity)
        {
            _context.Bookings.Update(entity);
        }

        public void UpdateStatus(int bookingId, string bookingStatus)
        {
            var bookingFromDb = _context.Bookings.FirstOrDefault(m => m.Id == bookingId);
            if (bookingFromDb is not null)
            {
                bookingFromDb.Status = bookingStatus;
                if (bookingStatus == SD.StatusCheckedIn)
                {
                    bookingFromDb.ActualCheckInDate = DateTime.Now;
                }
                if (bookingStatus == SD.StatusCompleted)
                {
                    bookingFromDb.ActualCheckOutDate = DateTime.Now;
                }
            }
        }

        public void UpdateStripPaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDb = _context.Bookings.FirstOrDefault(m => m.Id == bookingId);
            if (bookingFromDb is not null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    bookingFromDb.StripeSessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingFromDb.StripePaymentIntentId = paymentIntentId;
                    bookingFromDb.PaymentDate = DateTime.Now;
                    bookingFromDb.IsPaymentSuccessful = true;
                }
            }
        }
    }
}
