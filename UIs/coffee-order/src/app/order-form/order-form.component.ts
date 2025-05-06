import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { interval, Subscription } from 'rxjs';
import { LottieAnimationComponent } from '../lottie-animation/lottie-animation.component';

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [CommonModule, FormsModule, LottieAnimationComponent],
  templateUrl: './order-form.component.html',
})
export class OrderFormComponent {
  order = {
    size: '',
    coffeeType: '',
    milkType: '',
    sugarCount: 0,
    extraShot: false,
    temperature: 'Hot',
    notes: ''
  };

  loading = false;
  successMessage: string = '';
  toastMessage: string = '';
  toastTimer: any;
  trackingId: string = '';
  pollSub?: Subscription;
  isTrackingOrder: boolean = false;

  constructor(private http: HttpClient) { }

  submitOrder() {
    if (this.loading) return;

    this.loading = true;
    this.successMessage = '';
    this.toastMessage = '';

    this.http.post<any>('http://localhost:5120/orders', this.order).subscribe({
      next: (res) => {
        this.loading = false;
        this.toastMessage = 'Your Order placed. Waiting for preparation...';
        this.clearToastAfterDelay();

        this.trackingId = res.id;
        this.isTrackingOrder = true;
        this.pollOrderStatus(res.id);
        this.resetForm();
      },
      error: (err) => {
        this.loading = false;
        console.error('Order submission failed:', err);
        this.toastMessage = 'Oops something went wrong while you place your order please try later!';
        this.clearToastAfterDelay();
      }
    });
  }


  pollOrderStatus(id: string) {
    let lastStatus: string | null = null;

    this.pollSub?.unsubscribe();

    this.pollSub = interval(3000).subscribe(() => {
      this.http.get<any>(`http://localhost:5120/orders/${id}`).subscribe({
        next: (res) => {
          const status = res.status;

          if (status !== lastStatus) {
            lastStatus = status;

            switch (status) {
              //case 'Pending':
              //this.toastMessage = '🕐 Your order was submitted.';
              //this.clearToastAfterDelay();
              //break;

              case 'InProgress':
                this.toastMessage = 'Your order is in progress, please wait...';
                this.clearToastAfterDelay();
                break;

              case 'Ready':
                this.toastMessage = 'Your coffee is ready. Enjoy it!';
                this.clearToastAfterDelay();
                this.pollSub?.unsubscribe();
                setTimeout(() => location.reload(), 3000);
                break;

              case 'Failed':
                this.toastMessage = 'Your Order failed, Please try again later!.';
                this.clearToastAfterDelay();
                this.pollSub?.unsubscribe();
                setTimeout(() => location.reload(), 3000);
                break;
            }
          }
        },
        error: (err) => {
          console.error('Error checking order status:', err);
          this.toastMessage = 'SOmething wenr wrong please try later!';
          this.clearToastAfterDelay();
          this.pollSub?.unsubscribe();
          setTimeout(() => location.reload(), 3000);
        }
      });
    });
  }



  clearToastAfterDelay() {
    clearTimeout(this.toastTimer);
    this.toastTimer = setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }

  resetForm() {
    this.order = {
      size: '',
      coffeeType: '',
      milkType: '',
      sugarCount: 1,
      extraShot: false,
      temperature: 'Hot',
      notes: ''
    };
  }
}
