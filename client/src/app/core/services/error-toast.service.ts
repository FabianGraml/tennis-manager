import { Injectable } from '@angular/core';
import { ApplicationRef, ComponentFactoryResolver, Injector, ComponentRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { ToastComponent } from 'src/app/shared/components/toast/toast.component';

@Injectable({
  providedIn: 'root'
})
export class ErrorToastService {

  private toastComponentRef!: ComponentRef<ToastComponent>;

  constructor(
    private appRef: ApplicationRef,
    private componentFactoryResolver: ComponentFactoryResolver,
    private injector: Injector
  ) {
    this.createToastComponent();
  }

  private createToastComponent(): void {
    const factory = this.componentFactoryResolver.resolveComponentFactory(ToastComponent);
    this.toastComponentRef = factory.create(this.injector);
    this.appRef.attachView(this.toastComponentRef.location.nativeElement);
    document.body.appendChild(this.toastComponentRef.location.nativeElement);
  }

  handleError(error: HttpErrorResponse): any {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error ${error.status}: ${error.message}`;
    }
    this.showError(errorMessage);
    return throwError(error);
  }

  showError(message: string, delay: number = 5000): void {
    this.toastComponentRef.instance.message = message;
    this.toastComponentRef.instance.toastClass = 'bg-danger text-light';
    this.toastComponentRef.instance.showWithTimeout(delay);
  }
}