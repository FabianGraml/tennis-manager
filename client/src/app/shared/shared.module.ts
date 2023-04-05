import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { AddBookingComponent } from './components/add-booking/add-booking.component';



@NgModule({
  declarations: [
  
    HeaderComponent,
       AddBookingComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    HeaderComponent
  ]
})
export class SharedModule { }
