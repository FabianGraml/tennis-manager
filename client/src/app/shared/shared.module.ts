import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { FormsModule } from '@angular/forms';
import { AddBookingComponent } from './components/add-booking/add-booking.component';
import { ToastComponent } from './components/toast/toast.component';


@NgModule({
  declarations: [
    HeaderComponent,
    AddBookingComponent,
    ToastComponent
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    AddBookingComponent,
    HeaderComponent
  ]
})
export class SharedModule { }
