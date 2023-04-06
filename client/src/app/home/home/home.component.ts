import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import {
  BookingRequestDTO,
  BookingsService,
} from 'src/app/core/api/tennis-service';
import { TokenHandlerService } from 'src/app/core/services/token-handler.service';
import { AddBookingComponent } from 'src/app/shared/components/add-booking/add-booking.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  public hours: number[] = Array.from({ length: 15 }, (_, i) => i + 7);
  public daysOfWeek: string[] = [
    'Mon',
    'Tue',
    'Wed',
    'Thu',
    'Fri',
    'Sat',
    'Sun',
  ];
  public selectedDay: number = 1;
  public bookings: any[] = [];
  public selectedWeek: number = this.getCurrentWeekNumber();
  public bookingForm = {
    week: this.selectedWeek,
    dayOfWeek: 1,
    hour: 7,
  };

  constructor(
    private tokenHandler: TokenHandlerService,
    private bookingService: BookingsService,
    private modalService: NgbModal
  ) {}
  ngOnInit(): void {
    this.selectedWeek = this.getCurrentWeekNumber();
    this.getAllBookings();
  }
  findBooking(dayOfWeek: number, hour: number, week: number): any {
    
    return this.bookings.find(
      (booking) =>
        booking?.dayOfWeek === dayOfWeek &&
        booking?.hour === hour &&
        booking?.week === week
    );

  }
  getCurrentWeekNumber(): number {
    const now = new Date();
    const startOfYear = new Date(now.getFullYear(), 0, 1);
    const weekNumber = Math.ceil(
      ((now.getTime() - startOfYear.getTime()) / 86400000 +
        startOfYear.getDay() +
        1) /
        7
    );
    return weekNumber;
  }

  public isBooked(dayOfWeek: number, hour: number, week: number): any {
    const booking = this.bookings.find(
      (booking) =>
        booking && booking.dayOfWeek === dayOfWeek &&
        booking.hour === hour &&
        booking.week === week
    );
    return !!booking;
  }
  onCellClick(dayOfWeek: number, hour: number): void {
    if (!this.isBooked(dayOfWeek, hour, this.selectedWeek)) {
      this.bookingForm.dayOfWeek = dayOfWeek;
      this.bookingForm.hour = hour;
    }
    this.openDialog();
  }

  onSubmit(): void {}
  getAllBookings(): void {
    this.bookingService.apiBookingAllGet().subscribe({
      next: (data) => {
        this.bookings = data;
      },
      error: (error) => {
        console.error(error);
      },
    });
  }
openDialog() {
    const modalRef = this.modalService.open(AddBookingComponent);
    modalRef.componentInstance.bookingForm = {
      dayOfWeek: this.bookingForm.dayOfWeek,
      hour: this.bookingForm.hour,
      week: this.bookingForm.week
    };
    modalRef.result.then((result) => {
      const booking: BookingRequestDTO = {
        dayOfWeek: result.dayOfWeek,
        hour: result.hour,
        week: result.week,
        userId: Number(this.tokenHandler.getUserId()) || undefined
      };
      this.bookingService.apiBookingAddPost(booking).subscribe({
        next: (data) => {
          this.getAllBookings();
          this.bookings.push(data);
        },
        error: (error) => {
          console.error(error);
        },
      });
    }).catch((reason) => {
    });
  }  
}