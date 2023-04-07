import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, NgZone } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorService {
  constructor(private snackbar: MatSnackBar, private ngZone: NgZone) {}

  handleError(error: any): void {
    this.ngZone.run(() => {
      setTimeout(() => {
        if (error instanceof HttpErrorResponse) {
          if (error.status !== 500) {
            const config = new MatSnackBarConfig();
            config.duration = 3500;
            config.panelClass = ['snackbar-error'];
            config.verticalPosition = 'bottom';
            config.horizontalPosition = 'center';
            this.snackbar.open(error.error.failure.message, 'Schließen', config);
          } else {
            const config = new MatSnackBarConfig();
            config.duration = 5000;
            config.panelClass = ['snackbar-error'];
            config.verticalPosition = 'bottom';
            config.horizontalPosition = 'center';
            this.snackbar.open(
              'Unhandleable error occured',
              'Schließen',
              config
            );
          }
        }
      }, 0);
    });
  }
}