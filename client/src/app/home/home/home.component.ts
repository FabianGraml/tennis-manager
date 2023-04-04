import { Component, OnInit } from '@angular/core';
import { BookingRequestDTO, BookingsService } from 'src/app/core/api/tennis-service';
import { TokenHandlerService } from 'src/app/core/services/token-handler.service';

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
  public bookings: any[] = [
    {
      id: 1,
      week: 11,
      dayOfWeek: 2,
      hour: 18,
      user: {
        id: 1,
        firstname: 'Fabian',
        lastname: 'Graml',
        email: 'f.graml@gmx.at',
      },
    },
  ];
  public selectedWeek: number = this.getCurrentWeekNumber();
  public bookingForm = {
    week: this.selectedWeek,
    dayOfWeek: 1,
    hour: 7,
  };

  constructor(private tokenHandler: TokenHandlerService, private bookingService: BookingsService) {
    
  }
  ngOnInit(): void {
    this.selectedWeek = this.getCurrentWeekNumber();
    this.getAllBookings();
  }
  findBooking(dayOfWeek: number, hour: number, week: number): any {
    return this.bookings.find((booking) => booking.dayOfWeek === dayOfWeek && booking.hour === hour && booking.week === week);
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

  isBooked(dayOfWeek: number, hour: number, week: number): any {
    return this.bookings.find((booking) => booking.dayOfWeek === dayOfWeek && booking.hour === hour && booking.week === week);
  }

  onCellClick(dayOfWeek: number, hour: number): void {
    if (!this.isBooked(dayOfWeek, hour, this.selectedWeek)) {
      this.bookingForm.dayOfWeek = dayOfWeek;
      this.bookingForm.hour = hour;
    }
  }

  onSubmit(): void {
    if (
      this.bookingForm.week === this.selectedWeek &&
      this.bookingForm.dayOfWeek < new Date().getDay()
    ) {
      console.error('Cannot book a past date in the current week');
      return;
    }
    const userIdString: string | null = this.tokenHandler.getUserId();
    const userId: number | undefined = userIdString ? parseInt(userIdString, 10) : undefined;    var bookingRequesDTO : BookingRequestDTO = {
      dayOfWeek: this.bookingForm.dayOfWeek,
      hour: this.bookingForm.hour,
      week: this.bookingForm.week,
      userId: userId
    }
    this.bookingService.apiBookingAddPost(bookingRequesDTO).subscribe({
      next: (data) => {
        console.log(data);
        this.getAllBookings();
      },
      error: (error) => {
        console.error(error);
      }
    });
  }
  getAllBookings(): void {
    this.bookingService.apiBookingAllGet().subscribe({
      next: (data) => {
        this.bookings = data;
        console.log(data);
      },
      error: (error) => {
        console.error(error);
      }
    });
  }
}
