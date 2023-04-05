import { Component, OnInit } from '@angular/core';
import { BookingRequestDTO, BookingsService } from 'src/app/core/api/tennis-service';
import { TokenHandlerService } from 'src/app/core/services/token-handler.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddBookingComponent } from 'src/app/shared/components/add-booking/add-booking.component';
@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  public userBookings: any[] = [];
  public daysOfWeek: string[] = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];

  constructor(
    private tokenHandler: TokenHandlerService,
    private bookingService: BookingsService,
    private modalService: NgbModal
  ) { }
  ngOnInit(): void {
    this.getUserBookings();
  }
  getUserBookings(): void {
    const userId = Number(this.tokenHandler.getUserId());
    this.bookingService.apiBookingPersonGet(userId).subscribe({
      next: (data) => {
        this.userBookings = data;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }
  
  editBooking(booking: any): void {
    const modalRef = this.modalService.open(AddBookingComponent);
    modalRef.componentInstance.bookingForm = {
      dayOfWeek: booking.dayOfWeek,
      hour: booking.hour,
      week: booking.week
    };

    modalRef.result
      .then((result) => {
        const updatedBooking: BookingRequestDTO = {
          dayOfWeek: result.dayOfWeek,
          hour: result.hour,
          week: result.week,
          userId: Number(this.tokenHandler.getUserId()) || undefined,
        };
        this.bookingService.apiBookingUpdatePut(booking.id, updatedBooking).subscribe({
          next: (data) => {
            this.getUserBookings();
          },
          error: (error) => {
            console.error(error);
          },
        });
      })
      .catch((reason) => {
        console.log('***Modal closed***');
      });
  }
  deleteBooking(bookingId: number): void {
    this.bookingService.apiBookingRemoveDelete(bookingId).subscribe({
      next: () => {
        this.getUserBookings();
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

}
