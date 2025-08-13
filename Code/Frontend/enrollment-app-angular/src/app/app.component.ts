import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerLocateComponent } from './components/customer-locate/customer-locate.component';
import { SetupAccountComponent } from './components/setup-account/setup-account.component';
import { ThankYouComponent } from './components/thank-you/thank-you.component';
import { MFADemoComponent } from './components/mfa-demo/mfa-demo.component';
import { Customer } from './services/customer.service';

interface FormData {
  accountNumber: string;
  ssn: string;
  birthdate: string;
  fullName: string;
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
  phoneNumber: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    CustomerLocateComponent,
    SetupAccountComponent,
    ThankYouComponent,
    MFADemoComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  currentStep = signal(1);
  showMFADemo = signal(false);
  foundCustomer = signal<Customer | null>(null);
  formData = signal<FormData>({
    accountNumber: '',
    ssn: '',
    birthdate: '',
    fullName: '',
    email: '',
    username: '',
    password: '',
    confirmPassword: '',
    phoneNumber: ''
  });

  handleNext(data: Partial<FormData>) {
    this.formData.update(prev => ({ ...prev, ...data }));
    this.currentStep.update(prev => prev + 1);
  }

  handleCustomerFound(customer: Customer) {
    this.foundCustomer.set(customer);
    // Pre-fill the form with found customer data (excluding email)
    this.formData.update(prev => ({
      ...prev,
      accountNumber: prev.accountNumber,
      ssn: prev.ssn,
      birthdate: prev.birthdate,
      fullName: `${customer.firstName} ${customer.lastName}`,
      phoneNumber: customer.phoneNumber
      // Note: email is not pre-filled - user will enter it manually
    }));
    this.currentStep.update(prev => prev + 1);
  }

  handleBack() {
    this.currentStep.update(prev => prev - 1);
  }

  handleShowMFADemo() {
    this.showMFADemo.set(true);
  }

  handleBackToEnrollment() {
    this.showMFADemo.set(false);
    this.currentStep.set(1);
    this.formData.set({
      accountNumber: '',
      ssn: '',
      birthdate: '',
      fullName: '',
      email: '',
      username: '',
      password: '',
      confirmPassword: '',
      phoneNumber: ''
    });
  }

  renderStep() {
    switch (this.currentStep()) {
      case 1:
        return 'customer-locate';
      case 2:
        return 'setup-account';
      case 3:
        return 'thank-you';
      default:
        return 'customer-locate';
    }
  }
}
