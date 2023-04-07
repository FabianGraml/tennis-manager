import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { FormsModule } from '@angular/forms';
import { AddBookingComponent } from './components/add-booking/add-booking.component';
import { MaterialModule } from './material/material.module';


@NgModule({
  declarations: [
    HeaderComponent,
    AddBookingComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    MaterialModule,
  ],
  exports: [
    AddBookingComponent,
    HeaderComponent,
    MaterialModule,
  ]
})
export class SharedModule { }
