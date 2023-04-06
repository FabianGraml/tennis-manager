import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss']
})
export class ToastComponent implements OnInit {
  @Input() message: string;
  @Input() toastClass: string;
  show: boolean;

  constructor() {
    this.message = '';
    this.toastClass = '';
    this.show = false;
  }

  ngOnInit(): void {}

  showWithTimeout(delay: number): void {
    this.show = true;
    setTimeout(() => {
      this.hide();
    }, delay);
  }

  hide(): void {
    this.show = false;
  }
}
